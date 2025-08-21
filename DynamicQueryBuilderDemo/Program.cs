using DynamicQueryBuilderDemo.Data;
using DynamicQueryBuilderDemo.Filters;
using DynamicQueryBuilderDemo.Models;

Console.WriteLine("Wellcome to my application about dynmaic filter...");
Console.WriteLine("--------------------------------------------------");

using var context = new AppDbContext();

// داده نمونه
context.Users.AddRange(
    new User { Name = "Ali", Age = 25, IsActive = true },
    new User { Name = "Reza", Age = 30, IsActive = false },
    new User { Name = "Mohammad", Age = 35, IsActive = true },
    new User { Name = "Amin", Age = 28, IsActive = true }
);
context.SaveChanges();

// فیلترهای پیچیده (Nested + AND/OR)
var filters = new List<Filter>
{
    new()
    {
        PropertyName = nameof(User.IsActive),
        Operator = FilterOperator.Equals,
        Value = true
    },
    new()
    {
        LogicalOperator = LogicalOperator.And,
        NestedFilters =
        [
            new Filter 
            { 
                PropertyName = nameof(User.Name), 
                Operator = FilterOperator.Contains, 
                LogicalOperator = LogicalOperator.Or,
                Value = "a" },
            new Filter 
            { 
                PropertyName = nameof(User.Age), 
                Operator = FilterOperator.GreaterThanOrEqual, 
                LogicalOperator = LogicalOperator.Or,
                Value = 28 
            }
        ]
    },
   
};

var dynamicFilter = FilterBuilder.BuildFilter<User>(filters);
Console.WriteLine("Search with this expression :=> {0}", dynamicFilter.Body);

var result = context.Users.Where(dynamicFilter).ToList();

if (result.Any())
{
    foreach (var user in result)
        Console.WriteLine($"- {user.Name}, {user.Age}, {user.IsActive}");
}
else
{
    Console.WriteLine("- No users found with the specified filters.");
}

Console.WriteLine("End.");