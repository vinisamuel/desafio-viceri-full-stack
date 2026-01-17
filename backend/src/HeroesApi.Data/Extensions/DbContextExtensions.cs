namespace HeroesApi.Data.Extensions;

public static class DbContextExtensions
{
    public static IQueryable<TEntity> AddQueryIncludes<TEntity>(this IQueryable<TEntity> query,
        Expression<Func<TEntity, object>>[]? includes)
        where TEntity : class
    {
        ArgumentNullException.ThrowIfNull(query);

        if (includes?.Length > 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return query;
    }
}
