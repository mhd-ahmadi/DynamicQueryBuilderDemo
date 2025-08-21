namespace DynamicQueryBuilderDemo.Filters;

public class Filter
{
    public string PropertyName { get; set; } = null!;
    public FilterOperator Operator { get; set; }
    public object Value { get; set; } = null!;
    public LogicalOperator LogicalOperator { get; set; } = LogicalOperator.And;
    public List<Filter>? NestedFilters { get; set; }
}