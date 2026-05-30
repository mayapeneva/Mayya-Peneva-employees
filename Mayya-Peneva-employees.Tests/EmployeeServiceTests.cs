using Mayya_Peneva_employees.Client.Core.Entities;
using Mayya_Peneva_employees.Client.Core.Services;

namespace Mayya_Peneva_employees.Tests
{
    public class EmployeeServiceTests
    {
        private readonly EmployeeService _service;

        public EmployeeServiceTests()
        {
            _service = new EmployeeService();
        }

        [Fact]
        public void GetPairEmployeesWorkedLongest_ShouldReturnEmptyResultWhenNoProjectNoEmployees()
        {
            // Arrange
            var employees = new Dictionary<int, List<Employee>>();

            // Act
            var actualResult = _service.GetPairEmployeesWorkedLongest(employees);

            // Assert
            Assert.NotNull(actualResult);
            Assert.NotEmpty(actualResult.Errors);
            Assert.Empty(actualResult.EmployeesPerProject);
        }

        [Fact]
        public void GetPairEmployeesWorkedLongest_ShouldReturnEmptyResultWhenHasProjectButNoEmployees()
        {
            // Arrange
            var employees = new Dictionary<int, List<Employee>>
            {
                { 1, new List<Employee>() }
            };

            // Act
            var actualResult = _service.GetPairEmployeesWorkedLongest(employees);

            // Assert
            Assert.NotNull(actualResult);
            Assert.NotEmpty(actualResult.Errors);
            Assert.Empty(actualResult.EmployeesPerProject);
        }

        [Fact]
        public void GetPairEmployeesWorkedLongest_ShouldReturnEmptyResultWhenOneEmployeeOnly()
        {
            // Arrange
            var employees = new Dictionary<int, List<Employee>>
            {
                [1] = [new Employee { Id = 1, ProjectId = 1, DateFrom = new DateOnly(2020, 1, 1), DateTo = new DateOnly(2020, 12, 31) }]
            };

            // Act
            var actualResult = _service.GetPairEmployeesWorkedLongest(employees);

            // Assert
            Assert.NotNull(actualResult);
            Assert.NotEmpty(actualResult.Errors);
            Assert.Empty(actualResult.EmployeesPerProject);
        }

        [Fact]
        public void GetPairEmployeesWorkedLongest_ShouldReturnEmptyResultWhenNoOverlap()
        {
            // Arrange

            var employees = new Dictionary<int, List<Employee>>
            {
                [1] =
                [new Employee { Id = 1, ProjectId = 1, DateFrom = new DateOnly(2020, 1, 1), DateTo = new DateOnly(2020, 6, 30) },
                new Employee { Id = 2, ProjectId = 1, DateFrom = new DateOnly(2020, 7, 1), DateTo = new DateOnly(2020, 12, 31) }]
            };

            // Act
            var actualResult = _service.GetPairEmployeesWorkedLongest(employees);

            // Assert
            Assert.NotNull(actualResult);
            Assert.NotEmpty(actualResult.Errors);
            Assert.Empty(actualResult.EmployeesPerProject);
        }

        [Fact]
        public void GetPairEmployeesWorkedLongest_ShouldReturnEmptyResultWhenDuplicateEmployeeId()
        {
            var employees = new Dictionary<int, List<Employee>>
            {
                { 1, new List<Employee>
                    {
                        new Employee { Id = 1, ProjectId = 1, DateFrom = new DateOnly(2020, 1, 1), DateTo = new DateOnly(2020, 12, 31) },
                        new Employee { Id = 1, ProjectId = 1, DateFrom = new DateOnly(2020, 1, 1), DateTo = new DateOnly(2020, 12, 31) }
                    }
                }
            };

            // Act
            var actualResult = _service.GetPairEmployeesWorkedLongest(employees);

            // Assert
            Assert.NotNull(actualResult);
            Assert.NotEmpty(actualResult.Errors);
            Assert.Empty(actualResult.EmployeesPerProject);
        }

        [Fact]
        public void GetPairEmployeesWorkedLongest_ShouldReturnCorrectResultWhenOneOverlap()
        {
            // Arrange
            var employees = new Dictionary<int, List<Employee>>
            {
                [1] =
                [new Employee { Id = 1, ProjectId = 1, DateFrom = new DateOnly(2020, 1, 1), DateTo = new DateOnly(2020, 12, 31) },
                new Employee { Id = 2, ProjectId = 1, DateFrom = new DateOnly(2020, 6, 1), DateTo = new DateOnly(2020, 12, 31) }]
            };
            var actualDaysWorked = 214;

            // Act
            var actualResult = _service.GetPairEmployeesWorkedLongest(employees);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Empty(actualResult.Errors);
            Assert.Single(actualResult.EmployeesPerProject);

            var employeePair = actualResult.EmployeesPerProject.First();
            Assert.Equal(1, employeePair.EmployeeOneId);
            Assert.Equal(2, employeePair.EmployeeTwoId);
            Assert.Equal(1, employeePair.ProjectId);
            Assert.Equal(actualDaysWorked, employeePair.DaysWorked);
        }

        [Fact]
        public void GetPairEmployeesWorkedLongest_ShouldReturnCorrectResultWhenMoreOverlaps()
        {
            // Arrange
            var employees = new Dictionary<int, List<Employee>>
            {
                [1] =
                [new Employee { Id = 1, ProjectId = 1, DateFrom = new DateOnly(2020, 1, 1), DateTo = new DateOnly(2020, 12, 31) },
                new Employee { Id = 2, ProjectId = 1, DateFrom = new DateOnly(2020, 6, 1), DateTo = new DateOnly(2020, 12, 31) },
                new Employee { Id = 3, ProjectId = 1, DateFrom = new DateOnly(2020, 1, 1), DateTo = new DateOnly(2020, 3, 31) }]
            };
            var actualDaysWorked = 214;

            // Act
            var actualResult = _service.GetPairEmployeesWorkedLongest(employees);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Empty(actualResult.Errors);
            Assert.Single(actualResult.EmployeesPerProject);

            var employeePair = actualResult.EmployeesPerProject.First();
            Assert.Equal(1, employeePair.EmployeeOneId);
            Assert.Equal(2, employeePair.EmployeeTwoId);
            Assert.Equal(1, employeePair.ProjectId);
            Assert.Equal(actualDaysWorked, employeePair.DaysWorked);
        }

        [Fact]
        public void GetPairEmployeesWorkedLongest_ShouldReturnCorrectPairOrderWhenReversedPairOrder()
        {
            var employees = new Dictionary<int, List<Employee>>
            {
                { 1, new List<Employee>
                    {
                        new Employee { Id = 2, ProjectId = 1, DateFrom = new DateOnly(2020, 1, 1), DateTo = new DateOnly(2020, 12, 31) },
                        new Employee { Id = 1, ProjectId = 1, DateFrom = new DateOnly(2020, 1, 1), DateTo = new DateOnly(2020, 12, 31) }
                    }
                }
            };

            // Act
            var actualResult = _service.GetPairEmployeesWorkedLongest(employees);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Empty(actualResult.Errors);
            Assert.Single(actualResult.EmployeesPerProject);

            var employeePair = actualResult.EmployeesPerProject.First();
            Assert.Equal(1, employeePair.EmployeeOneId);
            Assert.Equal(2, employeePair.EmployeeTwoId);
        }

        [Fact]
        public void GetPairEmployeesWorkedLongest_ShouldReturnCorrectResultWhenMoreProjects()
        {
            // Arrange
            var employees = new Dictionary<int, List<Employee>>
            {
                [1] =
                [new Employee { Id = 1, ProjectId = 1, DateFrom = new DateOnly(2020, 1, 1), DateTo = new DateOnly(2020, 12, 31) },
                new Employee { Id = 2, ProjectId = 1, DateFrom = new DateOnly(2020, 6, 1), DateTo = new DateOnly(2020, 12, 31) }],
                [2] =
                [new Employee { Id = 3, ProjectId = 2, DateFrom = new DateOnly(2020, 1, 1), DateTo = new DateOnly(2020, 12, 31) },
                new Employee { Id = 4, ProjectId = 2, DateFrom = new DateOnly(2020, 1, 1), DateTo = new DateOnly(2020, 3, 31) }]
            };
            var actualDaysWorked = 214;

            // Act
            var actualResult = _service.GetPairEmployeesWorkedLongest(employees);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Empty(actualResult.Errors);
            Assert.Single(actualResult.EmployeesPerProject);

            var employeePair = actualResult.EmployeesPerProject.First();
            Assert.Equal(1, employeePair.EmployeeOneId);
            Assert.Equal(2, employeePair.EmployeeTwoId);
            Assert.Equal(1, employeePair.ProjectId);
            Assert.Equal(actualDaysWorked, employeePair.DaysWorked);
        }

        [Fact]
        public void GetPairEmployeesWorkedLongest_ShouldReturnLastProjectWhenSamePairInMultipleProjects()
        {
            // Arrange
            var employees = new Dictionary<int, List<Employee>>
            {
                { 1, new List<Employee>
                    {
                        new Employee { Id = 1, ProjectId = 1, DateFrom = new DateOnly(2020, 1, 1), DateTo = new DateOnly(2020, 6, 30) },
                        new Employee { Id = 2, ProjectId = 1, DateFrom = new DateOnly(2020, 1, 1), DateTo = new DateOnly(2020, 6, 30) }
                    }
                },
                { 2, new List<Employee>
                    {
                        new Employee { Id = 1, ProjectId = 2, DateFrom = new DateOnly(2020, 1, 1), DateTo = new DateOnly(2020, 6, 30) },
                        new Employee { Id = 2, ProjectId = 2, DateFrom = new DateOnly(2020, 1, 1), DateTo = new DateOnly(2020, 6, 30) }
                    }
                }
            };

            // Act
            var actualResult = _service.GetPairEmployeesWorkedLongest(employees);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Empty(actualResult.Errors);
            Assert.Single(actualResult.EmployeesPerProject);

            var employeePair = actualResult.EmployeesPerProject.First();
            Assert.Equal(2, employeePair.ProjectId);
        }

        [Fact]
        public void GetPairEmployeesWorkedLongest_ShouldReturnCorrectResultWhenPartialOverlap()
        {
            // Arrange
            var employees = new Dictionary<int, List<Employee>>
            {
                [1] =
                [new Employee { Id = 1, ProjectId = 1, DateFrom = new DateOnly(2020, 1, 1), DateTo = new DateOnly(2020, 6, 30) },
                new Employee { Id = 2, ProjectId = 1, DateFrom = new DateOnly(2020, 5, 1), DateTo = new DateOnly(2020, 8, 31) }]
            };
            var actualDaysWorked = 61;

            // Act
            var actualResult = _service.GetPairEmployeesWorkedLongest(employees);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Empty(actualResult.Errors);
            Assert.Single(actualResult.EmployeesPerProject);

            var employeePair = actualResult.EmployeesPerProject.First();
            Assert.Equal(1, employeePair.EmployeeOneId);
            Assert.Equal(2, employeePair.EmployeeTwoId);
            Assert.Equal(1, employeePair.ProjectId);
            Assert.Equal(actualDaysWorked, employeePair.DaysWorked);
        }

        [Fact]
        public void GetPairEmployeesWorkedLongest_ShouldReturnAllValidPairsWhenMoreEmployeesSameProjectSameLenght()
        {
            // Arrange
            var employees = new Dictionary<int, List<Employee>>
            {
                [1] =
                [new Employee { Id = 1, ProjectId = 1, DateFrom = new DateOnly(2020, 1, 1), DateTo = new DateOnly(2020, 12, 31) },
                new Employee { Id = 2, ProjectId = 1, DateFrom = new DateOnly(2020, 1, 1), DateTo = new DateOnly(2020, 12, 31) },
                new Employee { Id = 3, ProjectId = 1, DateFrom = new DateOnly(2020, 1, 1), DateTo = new DateOnly(2020, 12, 31) }]
            };
            var actualDaysWorked = 366;

            // Act
            var actualResult = _service.GetPairEmployeesWorkedLongest(employees);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Empty(actualResult.Errors);
            Assert.Equal(3, actualResult.EmployeesPerProject.Count());

            var employeePair = actualResult.EmployeesPerProject.First();
            Assert.Equal(1, employeePair.EmployeeOneId);
            Assert.Equal(2, employeePair.EmployeeTwoId);
            Assert.Equal(1, employeePair.ProjectId);
            Assert.Equal(actualDaysWorked, employeePair.DaysWorked);
        }

        [Fact]
        public void GetPairEmployeesWorkedLongest_ShouldReturnCorrectResultWhenIdenticalDateRanges()
        {
            // Arrange
            var employees = new Dictionary<int, List<Employee>>
            {
                [1] =
                [new Employee { Id = 1, ProjectId = 1, DateFrom = new DateOnly(2020, 1, 1), DateTo = new DateOnly(2020, 12, 31) },
                new Employee { Id = 2, ProjectId = 1, DateFrom = new DateOnly(2020, 1, 1), DateTo = new DateOnly(2020, 12, 31) }]
            };
            var actualDaysWorked = 366;

            // Act
            var actualResult = _service.GetPairEmployeesWorkedLongest(employees);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Empty(actualResult.Errors);
            Assert.Single(actualResult.EmployeesPerProject);

            var employeePair = actualResult.EmployeesPerProject.First();
            Assert.Equal(1, employeePair.EmployeeOneId);
            Assert.Equal(2, employeePair.EmployeeTwoId);
            Assert.Equal(1, employeePair.ProjectId);
            Assert.Equal(actualDaysWorked, employeePair.DaysWorked);
        }

        [Fact]
        public void GetPairEmployeesWorkedLongest_ShouldReturnCorrectResultWhenSingleDayOverlap()
        {
            // Arrange
            var employees = new Dictionary<int, List<Employee>>
            {
                [1] =
                [new Employee { Id = 1, ProjectId = 1, DateFrom = new DateOnly(2020, 1, 1), DateTo = new DateOnly(2020, 1, 15) },
                new Employee { Id = 2, ProjectId = 1, DateFrom = new DateOnly(2020, 1, 15), DateTo = new DateOnly(2020, 1, 31) }]
            };

            // Act
            var actualResult = _service.GetPairEmployeesWorkedLongest(employees);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Empty(actualResult.Errors);
            Assert.Single(actualResult.EmployeesPerProject);

            var employeePair = actualResult.EmployeesPerProject.First();
            Assert.Equal(1, employeePair.EmployeeOneId);
            Assert.Equal(2, employeePair.EmployeeTwoId);
            Assert.Equal(1, employeePair.ProjectId);
            Assert.Equal(1, employeePair.DaysWorked);
        }

        [Fact]
        public void GetPairEmployeesWorkedLongest_ShouldReturnCorrectNumberOfCombinationsWhenHavingLargeDataset()
        {
            // Arrange
            var employees = new Dictionary<int, List<Employee>> { [1] = [] };
            for (int i = 1; i <= 100; i++)
            {
                employees[1].Add(new Employee
                {
                    Id = i,
                    ProjectId = 1,
                    DateFrom = new DateOnly(2020, 1, 1),
                    DateTo = new DateOnly(2020, 12, 31)
                });
            }
            var totalNumberOfCombinations = (100 * (100 - 1)) / 2;

            // Act
            var actualResult = _service.GetPairEmployeesWorkedLongest(employees);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Empty(actualResult.Errors);
            Assert.Equal(totalNumberOfCombinations, actualResult.EmployeesPerProject.Count());
        }

        [Fact]
        public void GetPairEmployeesWorkedLongest_ShouldReturnCorrectResultWhenMultiYearOverlap()
        {
            // Arrange
            var employees = new Dictionary<int, List<Employee>>
            {
                [1] =
                [new Employee { Id = 1, ProjectId = 1, DateFrom = new DateOnly(2018, 1, 1), DateTo = new DateOnly(2022, 12, 31) },
                new Employee { Id = 2, ProjectId = 1, DateFrom = new DateOnly(2019, 6, 1), DateTo = new DateOnly(2021, 6, 30) }]
            };
            var actualDaysWorked = 761;

            // Act
            var actualResult = _service.GetPairEmployeesWorkedLongest(employees);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Empty(actualResult.Errors);
            Assert.Single(actualResult.EmployeesPerProject);

            var employeePair = actualResult.EmployeesPerProject.First();
            Assert.Equal(1, employeePair.EmployeeOneId);
            Assert.Equal(2, employeePair.EmployeeTwoId);
            Assert.Equal(1, employeePair.ProjectId);
            Assert.Equal(actualDaysWorked, employeePair.DaysWorked);
        }

        [Fact]
        public void GetPairEmployeesWorkedLongest_ShouldReturnCorrectResultWhentLeapYear()
        {
            // Arrange
            var employees = new Dictionary<int, List<Employee>>
            {
                { 1, new List<Employee>
                    {
                        new Employee { Id = 1, ProjectId = 1, DateFrom = new DateOnly(2020, 2, 1), DateTo = new DateOnly(2020, 3, 15) },
                        new Employee { Id = 2, ProjectId = 1, DateFrom = new DateOnly(2020, 2, 15), DateTo = new DateOnly(2020, 3, 15) }
                    }
                }
            };
            var actualDaysWorked = 30;

            // Act
            var actualResult = _service.GetPairEmployeesWorkedLongest(employees);

            // Assert
            Assert.True(actualResult.IsSuccessful());
            Assert.Equal(actualDaysWorked, actualResult.EmployeesPerProject.First().DaysWorked);
        }

        [Fact]
        public void GetPairEmployeesWorkedLongest_ShouldReturnCorrectResultWhenNotLeapYear()
        {
            // Arrange
            var employees = new Dictionary<int, List<Employee>>
            {
                { 1, new List<Employee>
                    {
                        new Employee { Id = 1, ProjectId = 1, DateFrom = new DateOnly(2021, 2, 1), DateTo = new DateOnly(2021, 3, 15) },
                        new Employee { Id = 2, ProjectId = 1, DateFrom = new DateOnly(2021, 2, 15), DateTo = new DateOnly(2021, 3, 15) }
                    }
                }
            };
            var actualDaysWorked = 29;

            // Act
            var actualResult = _service.GetPairEmployeesWorkedLongest(employees);

            // Assert
            Assert.True(actualResult.IsSuccessful());
            Assert.Equal(actualDaysWorked, actualResult.EmployeesPerProject.First().DaysWorked);
        }

        [Fact]
        public void GetPairEmployeesWorkedLongest_ShouldReturnCorrectResultWhenHavingMultipleProjectsComplexScenario()
        {
            // Arrange
            var employees = new Dictionary<int, List<Employee>>
            {
                [1] =
                [new Employee { Id = 1, ProjectId = 1, DateFrom = new DateOnly(2020, 1, 1), DateTo = new DateOnly(2020, 4, 10) },
                new Employee { Id = 2, ProjectId = 1, DateFrom = new DateOnly(2020, 1, 1), DateTo = new DateOnly(2020, 4, 10) },
                new Employee { Id = 3, ProjectId = 1, DateFrom = new DateOnly(2020, 1, 1), DateTo = new DateOnly(2020, 2, 20) }],
                [2] =
                [new Employee { Id = 4, ProjectId = 2, DateFrom = new DateOnly(2020, 1, 1), DateTo = new DateOnly(2020, 6, 30) },
                new Employee { Id = 5, ProjectId = 2, DateFrom = new DateOnly(2020, 1, 1), DateTo = new DateOnly(2020, 6, 30) }]
            };
            var actualDaysWorked = 182;

            // Act
            var actualResult = _service.GetPairEmployeesWorkedLongest(employees);

            // Assert
            Assert.NotNull(actualResult);
            Assert.Empty(actualResult.Errors);
            Assert.Single(actualResult.EmployeesPerProject);

            var employeePair = actualResult.EmployeesPerProject.First();
            Assert.Equal(4, employeePair.EmployeeOneId);
            Assert.Equal(5, employeePair.EmployeeTwoId);
            Assert.Equal(2, employeePair.ProjectId);
            Assert.Equal(actualDaysWorked, employeePair.DaysWorked);
        }
    }
}