namespace BeHealthy.Application.Common.Helpers;

public static class OrderByHelper
{
    public static Expression<Func<T, object>>? GetOrderByExpression<T>(string? propertyName)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
            return null;

        var parameter = Expression.Parameter(typeof(T), "x");
        
        try
        {
            var propertyParts = propertyName.Split('.');
            Expression propertyAccess = parameter;
            
            foreach (var part in propertyParts)
            {
                propertyAccess = Expression.PropertyOrField(propertyAccess, part);
            }
            
            // Convert to object for compatibility
            var conversion = Expression.Convert(propertyAccess, typeof(object));
            
            return Expression.Lambda<Func<T, object>>(conversion, parameter);
        }
        catch
        {
            // If property doesn't exist, return null
            return null;
        }
    }
}