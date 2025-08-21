# DynamicQueryBuilderDemo

A simple demo project that showcases how to build **dynamic LINQ queries** in C# using a reusable and flexible `DynamicQueryBuilder`. This approach allows developers to build `IQueryable` filters at runtime without hardcoding conditions, making it useful for search pages, advanced filters, and admin dashboards.

---

## ✨ Features
- Build queries dynamically at runtime.
- Combine multiple conditions with **AND** / **OR**.
- Supports different comparison operators (`Equals`, `Contains`, `GreaterThan`, `LessThan`, etc.).
- Works seamlessly with **Entity Framework Core** and LINQ.
- Extensible design — you can easily add new operators.

---

## 📂 Project Structure
```
DynamicQueryBuilderDemo/
│── DynamicQueryBuilderDemo.csproj     # Project file
│── Program.cs                         # Demo runner (console app)
│
├── Models/
│   └── Product.cs                     # Example entity model
│
├── QueryBuilder/
│   ├── DynamicQueryBuilder.cs         # Main query builder
│   ├── FilterCondition.cs             # Condition model
│   └── FilterOperator.cs              # Supported operators (enum)
│
└── Data/
    └── AppDbContext.cs                # EF Core DbContext (InMemory demo)
```

---

## 🚀 Getting Started

### 1. Clone the Repository
```bash
git clone https://github.com/your-username/DynamicQueryBuilderDemo.git
cd DynamicQueryBuilderDemo
```

### 2. Build the Project
```bash
dotnet build
```

### 3. Run the Demo
```bash
dotnet run
```

This will execute the sample query builder against in-memory data.

---

## 🔧 Usage Example

### Define Model
```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
}
```

### Define Filter Conditions
```csharp
var filters = new List<FilterCondition>
{
    new FilterCondition { Field = "Name", Operator = FilterOperator.Contains, Value = "Laptop" },
    new FilterCondition { Field = "Price", Operator = FilterOperator.GreaterThan, Value = 1000M }
};
```

### Build Dynamic Query
```csharp
var query = DynamicQueryBuilder.Build(products.AsQueryable(), filters);
var result = query.ToList();
```

This will generate a query equivalent to:
```sql
WHERE Name.Contains("Laptop") AND Price > 1000
```

---

## 🧩 Core Components

### `FilterCondition`
Represents a single filter rule.
```csharp
public class FilterCondition
{
    public string Field { get; set; } = string.Empty;
    public FilterOperator Operator { get; set; }
    public object? Value { get; set; }
}
```

### `FilterOperator`
Enumeration of supported operators.
```csharp
public enum FilterOperator
{
    Equals,
    Contains,
    GreaterThan,
    LessThan,
    GreaterThanOrEqual,
    LessThanOrEqual
}
```

### `DynamicQueryBuilder`
Builds an `Expression<Func<T,bool>>` from a list of `FilterCondition`.
```csharp
public static class DynamicQueryBuilder
{
    public static IQueryable<T> Build<T>(IQueryable<T> source, List<FilterCondition> filters)
    {
        // Implementation...
    }
}
```

---

## 📖 Example Output
If the dataset is:
```json
[
  { "Id": 1, "Name": "Gaming Laptop", "Price": 1500, "Category": "Electronics" },
  { "Id": 2, "Name": "Office Chair", "Price": 200, "Category": "Furniture" },
  { "Id": 3, "Name": "Ultrabook Laptop", "Price": 1200, "Category": "Electronics" }
]
```

With the filters above, the result will be:
```json
[
  { "Id": 1, "Name": "Gaming Laptop", "Price": 1500, "Category": "Electronics" },
  { "Id": 3, "Name": "Ultrabook Laptop", "Price": 1200, "Category": "Electronics" }
]
```

---

## 🛠️ Extending
To add new operators:
1. Extend `FilterOperator` enum.
2. Update the `DynamicQueryBuilder` to handle the new operator.

Example:
```csharp
public enum FilterOperator
{
    Equals,
    Contains,
    GreaterThan,
    LessThan,
    StartsWith,
    EndsWith
}
```

---

## 🏗️ Technologies Used
- **.NET 9.0**
- **Entity Framework Core (InMemory)**
- **LINQ Expressions**

---

## 🤝 Contributing
Contributions are welcome! Feel free to fork the repo and submit PRs.

---

## 📬 Contact
Created by [Mohammad Ahmadi](mailto:mhd.ahmadi.dev@gmail.com)
