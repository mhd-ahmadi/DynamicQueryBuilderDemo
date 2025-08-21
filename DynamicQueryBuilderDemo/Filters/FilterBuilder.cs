using System.Collections;
using System.Linq.Expressions;

namespace DynamicQueryBuilderDemo.Filters;

public static class FilterBuilder
{
    public static Expression<Func<T, bool>> BuildFilter<T>(List<Filter> filters)
    {
        var param = Expression.Parameter(typeof(T), "x");
        var body = BuildExpressionBody<T>(filters, param);

        return Expression.Lambda<Func<T, bool>>(body, param);
    }

    private static Expression BuildExpressionBody<T>(List<Filter> filters, ParameterExpression param)
    {
        Expression? expr = null;

        foreach (var filter in filters)
        {
            Expression currentExpr;

            // اگر Nested Filter دارد
            if (filter.NestedFilters != null && filter.NestedFilters.Any())
            {
                currentExpr = BuildExpressionBody<T>(filter.NestedFilters, param);
            }
            else
            {
                if (string.IsNullOrEmpty(filter.PropertyName))
                    throw new ArgumentNullException(nameof(filter.PropertyName),
                        "PropertyName cannot be null or empty for non-nested filter.");

                currentExpr = BuildSingleFilterExpression<T>(param, filter);
            }

            // ترکیب با AND یا OR سطح بالا
            expr = expr == null
                ? currentExpr
                : filter.LogicalOperator == LogicalOperator.And
                    ? Expression.AndAlso(expr, currentExpr)
                    : Expression.OrElse(expr, currentExpr);
        }

        return expr ?? Expression.Constant(true);
    }

    private static Expression BuildSingleFilterExpression<T>(ParameterExpression param, Filter filter)
    {
        var property = Expression.Property(param, filter.PropertyName);
        var propertyType = property.Type;

        Expression comparison = filter.Operator switch
        {
            FilterOperator.Equals => Expression.Equal(
                property,
                Expression.Constant(Convert.ChangeType(filter.Value, propertyType))
            ),
            FilterOperator.GreaterThanOrEqual => Expression.GreaterThanOrEqual(
                property,
                Expression.Constant(Convert.ChangeType(filter.Value, propertyType))
            ),
            FilterOperator.LessThanOrEqual => Expression.LessThanOrEqual(
                property,
                Expression.Constant(Convert.ChangeType(filter.Value, propertyType))
            ),
            FilterOperator.Contains when propertyType == typeof(string) =>
                Expression.Call(
                    property,
                    typeof(string).GetMethod("Contains", new[] { typeof(string) })!,
                    Expression.Constant(filter.Value)
                ),
            FilterOperator.In => BuildInExpression(property, filter.Value),
            _ => throw new NotSupportedException($"Operator {filter.Operator} not supported for type {propertyType}")
        };

        return comparison;
    }

    private static Expression BuildInExpression(MemberExpression property, object value)
    {
        var propertyType = property.Type;
        var listType = typeof(List<>).MakeGenericType(propertyType);

        if (!(value is IEnumerable enumerable))
            throw new ArgumentException("Value for 'In' operator must be IEnumerable");

        var typedList = (IList)Activator.CreateInstance(listType)!;
        foreach (var item in enumerable)
        {
            typedList.Add(Convert.ChangeType(item, propertyType)!);
        }

        var constant = Expression.Constant(typedList);
        var containsMethod = listType.GetMethod("Contains", new[] { propertyType })!;
        return Expression.Call(constant, containsMethod, property);
    }
}