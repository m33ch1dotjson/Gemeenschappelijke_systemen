using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Comparers;
using Xunit;

public class EmployeeComparerTests
{
    [Fact]
    public void Compare_SortsEmployeesAscending_ByFullName()
    {
        // Arrange: create a list of employees with unordered full names
        var employees = new List<Employee>
        {
            new Employee("alice", "Alice Zwart"),
            new Employee("bob", "Bob Appel"),
            new Employee("charlie", "Charlie Maan")
        };

        // Create a comparer instance with ascending order (default)
        var comparer = new EmployeeComparer(); // ascending by default

        // Act: sort the employees using the comparer
        var sorted = employees.OrderBy(e => e, comparer).ToList();

        // Assert: verify the order is A-Z by FullName
        Assert.Equal("Alice Zwart", sorted[0].GetFullName());
        Assert.Equal("Bob Appel", sorted[1].GetFullName());
        Assert.Equal("Charlie Maan", sorted[2].GetFullName());
    }

    [Fact]
    public void Compare_SortsEmployeesDescending_ByFullName()
    {
        // Arrange: create a list of employees with unordered full names
        var employees = new List<Employee>
        {
            new Employee("alice", "Alice Zwart"),
            new Employee("bob", "Bob Appel"),
            new Employee("charlie", "Charlie Maan")
        };

        // Create a comparer instance with descending order
        var comparer = new EmployeeComparer(descending: true);

        // Act: sort the employees using the comparer
        var sorted = employees.OrderBy(e => e, comparer).ToList();

        // Assert: verify the order is Z-A by FullName
        Assert.Equal("Charlie Maan", sorted[0].GetFullName());
        Assert.Equal("Bob Appel", sorted[1].GetFullName());
        Assert.Equal("Alice Zwart", sorted[2].GetFullName());
    }

    [Fact]
    public void Compare_ReturnsZero_WhenAnyEmployeeIsNull()
    {
        // Arrange: create a valid employee and a comparer instance
        var comparer = new EmployeeComparer();
        var validEmployee = new Employee("mika", "Mika Kleinbergen");

        // Act & Assert: comparisons with null should return 0 (no sorting preference)
        Assert.Equal(0, comparer.Compare(null, validEmployee));
        Assert.Equal(0, comparer.Compare(validEmployee, null));
        Assert.Equal(0, comparer.Compare(null, null));
    }
}
