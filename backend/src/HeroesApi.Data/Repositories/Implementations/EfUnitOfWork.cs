using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using HeroesApi.Data.Extensions;

namespace HeroesApi.Data.Repositories.Implementations;

public class EfUnitOfWork<TContext>(TContext dbContext) : IUnitOfWork<TContext>
    where TContext : BaseContext<TContext>
{
    private readonly TContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    private IDbContextTransaction? _dbContextTransaction = null;
    private bool _disposed = false;

    #region Controle de Transação

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (_dbContextTransaction != null)
            throw new InvalidOperationException("Já existe uma transação ativa. Confirme ou reverta a transação atual antes de iniciar uma nova.");

        _dbContextTransaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (_dbContextTransaction is null)
            throw new InvalidOperationException("Não há transação ativa para confirmar. Chame BeginTransactionAsync() primeiro.");

        try
        {
            await _dbContextTransaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (_dbContextTransaction is null)
            return;

        try
        {
            await _dbContextTransaction.RollbackAsync(cancellationToken);
        }
        catch
        {
            // Ignora erros no rollback
        }
        finally
        {
            await DisposeTransactionAsync();
        }
    }

    private async Task DisposeTransactionAsync()
    {
        if (_dbContextTransaction != null)
        {
            await _dbContextTransaction.DisposeAsync();
            _dbContextTransaction = null;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            try
            {
                if (_dbContextTransaction != null)
                {
                    try
                    {
                        _dbContextTransaction.Rollback();
                    }
                    catch
                    {
                        // Ignora erros
                    }

                    try
                    {
                        _dbContextTransaction.Dispose();
                    }
                    catch
                    {
                        // Ignora erros
                    }
                    finally
                    {
                        _dbContextTransaction = null;
                    }
                }
            }
            catch
            {
                // Garante que o dispose sempre complete
            }
        }

        _disposed = true;
    }

    #endregion

    #region Operações de Leitura

    public async Task<TEntity?> FindByIdAsync<TEntity>(
        long id,
        Expression<Func<TEntity, TEntity>>? selector = null,
        Expression<Func<TEntity, object>>[]? includes = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        CancellationToken cancellationToken = default)
        where TEntity : BaseEntity
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        return await GetFirstAsync(
            condition: x => x.Id!.Equals(id),
            selector,
            includes,
            orderBy,
            cancellationToken);
    }

    public async Task<TEntity?> GetFirstAsync<TEntity>(
        Expression<Func<TEntity, bool>> condition,
        Expression<Func<TEntity, TEntity>>? selector = null,
        Expression<Func<TEntity, object>>[]? includes = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        CancellationToken cancellationToken = default)
        where TEntity : BaseEntity
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentNullException.ThrowIfNull(condition);

        IQueryable<TEntity> query = _dbContext.Set<TEntity>()
            .AddQueryIncludes(includes)
            .Where(condition);

        if (selector != null)
            query = query.Select(selector);

        if (orderBy != null)
            query = orderBy(query);

        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<TEntity?> GetSingleAsync<TEntity>(
        Expression<Func<TEntity, bool>> condition,
        Expression<Func<TEntity, TEntity>>? selector = null,
        Expression<Func<TEntity, object>>[]? includes = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        CancellationToken cancellationToken = default)
        where TEntity : BaseEntity
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentNullException.ThrowIfNull(condition);

        IQueryable<TEntity> query = _dbContext.Set<TEntity>()
            .AddQueryIncludes(includes)
            .Where(condition);

        if (selector != null)
            query = query.Select(selector);

        if (orderBy != null)
            query = orderBy(query);

        return await query.SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IList<TEntity>> GetAllAsync<TEntity>(
        Expression<Func<TEntity, TEntity>>? selector = null,
        Expression<Func<TEntity, object>>[]? includes = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        CancellationToken cancellationToken = default)
        where TEntity : BaseEntity
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        IQueryable<TEntity> query = _dbContext.Set<TEntity>()
            .AddQueryIncludes(includes);

        if (selector != null)
            query = query.Select(selector);

        if (orderBy != null)
            query = orderBy(query);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<IList<TEntity>> GetListAsync<TEntity>(
        Expression<Func<TEntity, bool>> condition,
        Expression<Func<TEntity, TEntity>>? selector = null,
        Expression<Func<TEntity, object>>[]? includes = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        CancellationToken cancellationToken = default)
        where TEntity : BaseEntity
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentNullException.ThrowIfNull(condition);

        IQueryable<TEntity> query = _dbContext.Set<TEntity>()
            .AddQueryIncludes(includes)
            .Where(condition);

        if (selector != null)
            query = query.Select(selector);

        if (orderBy != null)
            query = orderBy(query);

        return await query.ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync<TEntity>(
        Expression<Func<TEntity, bool>> condition,
        CancellationToken cancellationToken = default)
        where TEntity : BaseEntity
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentNullException.ThrowIfNull(condition);

        return await _dbContext.Set<TEntity>()
            .AnyAsync(condition, cancellationToken);
    }

    public async Task<bool> ExistsByIdAsync<TEntity>(
        long id,
        CancellationToken cancellationToken = default)
        where TEntity : BaseEntity
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        return await ExistsAsync<TEntity>(
            condition: e => e.Id!.Equals(id), 
            cancellationToken);
    }

    public async Task<int> CountAsync<TEntity>(
        Expression<Func<TEntity, bool>> condition,
        CancellationToken cancellationToken = default)
        where TEntity : BaseEntity
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentNullException.ThrowIfNull(condition);

        return await _dbContext.Set<TEntity>()
            .CountAsync(condition, cancellationToken);
    }

    public async Task<long> CountLongAsync<TEntity>(
        Expression<Func<TEntity, bool>> condition,
        CancellationToken cancellationToken = default)
        where TEntity : BaseEntity
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentNullException.ThrowIfNull(condition);

        return await _dbContext.Set<TEntity>()
            .LongCountAsync(condition, cancellationToken);
    }

    #endregion

    #region Operações de Escrita

    public async Task<int> AddAsync<TEntity>(
        TEntity entity,
        CancellationToken cancellationToken = default) 
        where TEntity : BaseEntity
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentNullException.ThrowIfNull(entity);

        await _dbContext.Set<TEntity>().AddAsync(entity, cancellationToken);
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> AddMultipleAsync<TEntity>(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default) 
        where TEntity : BaseEntity
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentNullException.ThrowIfNull(entities);

        var entitiesList = entities.ToList();
        if (entitiesList.Count == 0)
            throw new ArgumentOutOfRangeException(nameof(entities), "A coleção de entidades não pode estar vazia.");

        await _dbContext.Set<TEntity>().AddRangeAsync(entitiesList, cancellationToken);
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> UpdateAsync<TEntity>(
        Expression<Func<TEntity, bool>> condition,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> fields,
        CancellationToken cancellationToken = default) 
        where TEntity : BaseEntity
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentNullException.ThrowIfNull(condition);
        ArgumentNullException.ThrowIfNull(fields);

        return await _dbContext.Set<TEntity>()
            .Where(condition)
            .ExecuteUpdateAsync(fields, cancellationToken);
    }

    public async Task<int> UpdateByIdAsync<TEntity>(
        long id,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> fields,
        CancellationToken cancellationToken = default)
        where TEntity : BaseEntity
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentNullException.ThrowIfNull(fields);

        return await UpdateAsync(
            condition: e => e.Id!.Equals(id),
            fields,
            cancellationToken);
    }

    public async Task<int> DeleteAsync<TEntity>(
        Expression<Func<TEntity, bool>> condition,
        CancellationToken cancellationToken = default) 
        where TEntity : BaseEntity
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        ArgumentNullException.ThrowIfNull(condition);

        return await _dbContext.Set<TEntity>()
            .Where(condition)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<int> DeleteByIdAsync<TEntity>(
        long id,
        CancellationToken cancellationToken = default)
        where TEntity : BaseEntity
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        return await DeleteAsync<TEntity>(
            condition: e => e.Id!.Equals(id),
            cancellationToken);
    }

    #endregion
}