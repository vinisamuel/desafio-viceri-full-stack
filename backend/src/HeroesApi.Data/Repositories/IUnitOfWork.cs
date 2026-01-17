using Microsoft.EntityFrameworkCore.Query;

namespace HeroesApi.Data.Repositories;

/// <summary>
/// Interface para implementação do padrão Unit of Work (Unidade de Trabalho) com Entity Framework Core.
/// </summary>
/// <typeparam name="TContext">O tipo do DbContext que herda de <see cref="BaseContext{TContext}"/>.</typeparam>
public interface IUnitOfWork<TContext> : IDisposable
    where TContext : BaseContext<TContext>
{
    #region Controle de Transação

    /// <summary>
    /// Inicia uma transação no banco de dados.
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Confirma (commit) a transação atual, persistindo todas as mudanças no banco de dados.
    /// </summary>
    Task CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Reverte (rollback) a transação atual, descartando todas as mudanças não confirmadas.
    /// </summary>
    Task RollbackAsync(CancellationToken cancellationToken = default);

    #endregion

    #region Operações de Leitura

    /// <summary>
    /// Busca uma entidade por seu ID (chave primária).
    /// </summary>
    Task<TEntity?> FindByIdAsync<TEntity>(
        long id,
        Expression<Func<TEntity, TEntity>>? selector = null,
        Expression<Func<TEntity, object>>[]? includes = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        CancellationToken cancellationToken = default)
        where TEntity : BaseEntity;

    /// <summary>
    /// Busca o primeiro registro que satisfaz a condição especificada.
    /// </summary>
    Task<TEntity?> GetFirstAsync<TEntity>(
        Expression<Func<TEntity, bool>> condition,
        Expression<Func<TEntity, TEntity>>? selector = null,
        Expression<Func<TEntity, object>>[]? includes = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        CancellationToken cancellationToken = default)
        where TEntity : BaseEntity;

    /// <summary>
    /// Busca um único registro que satisfaz a condição especificada. Lança exceção se encontrar mais de um resultado.
    /// </summary>
    Task<TEntity?> GetSingleAsync<TEntity>(
        Expression<Func<TEntity, bool>> condition,
        Expression<Func<TEntity, TEntity>>? selector = null,
        Expression<Func<TEntity, object>>[]? includes = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        CancellationToken cancellationToken = default)
        where TEntity : BaseEntity;

    /// <summary>
    /// Busca TODOS os registros de uma entidade, sem filtros.
    /// </summary>
    Task<IList<TEntity>> GetAllAsync<TEntity>(
        Expression<Func<TEntity, TEntity>>? selector = null,
        Expression<Func<TEntity, object>>[]? includes = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        CancellationToken cancellationToken = default)
        where TEntity : BaseEntity;

    /// <summary>
    /// Busca uma lista de registros que satisfazem a condição especificada.
    /// </summary>
    Task<IList<TEntity>> GetListAsync<TEntity>(
        Expression<Func<TEntity, bool>> condition,
        Expression<Func<TEntity, TEntity>>? selector = null,
        Expression<Func<TEntity, object>>[]? includes = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        CancellationToken cancellationToken = default)
        where TEntity : BaseEntity;

    /// <summary>
    /// Verifica se existe pelo menos um registro que satisfaz a condição especificada.
    /// </summary>
    Task<bool> ExistsAsync<TEntity>(
        Expression<Func<TEntity, bool>> condition,
        CancellationToken cancellationToken = default)
        where TEntity : BaseEntity;

    /// <summary>
    /// Verifica se existe pelo menos um registro que satisfaz a condição especificada.
    /// </summary>
    Task<bool> ExistsByIdAsync<TEntity>(
        long id,
        CancellationToken cancellationToken = default)
        where TEntity : BaseEntity;

    /// <summary>
    /// Conta o número de registros que satisfazem a condição especificada. Retorna <c>int</c> (máximo ~2 bilhões).
    /// </summary>
    Task<int> CountAsync<TEntity>(
        Expression<Func<TEntity, bool>> condition,
        CancellationToken cancellationToken = default)
        where TEntity : BaseEntity;

    /// <summary>
    /// Conta o número de registros que satisfazem a condição especificada. Retorna <c>long</c> (máximo ~9 quintilhões).
    /// </summary>
    Task<long> CountLongAsync<TEntity>(
        Expression<Func<TEntity, bool>> condition,
        CancellationToken cancellationToken = default)
        where TEntity : BaseEntity;

    #endregion

    #region Operações de Escrita

    /// <summary>
    /// Adiciona uma nova entidade ao banco de dados.
    /// </summary>
    Task<int> AddAsync<TEntity>(
        TEntity entity,
        CancellationToken cancellationToken = default)
        where TEntity : BaseEntity;

    /// <summary>
    /// Adiciona múltiplas entidades ao banco de dados em uma única operação (bulk insert).
    /// </summary>
    Task<int> AddMultipleAsync<TEntity>(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default) 
        where TEntity : BaseEntity;

    /// <summary>
    /// Atualiza registros que satisfazem a condição especificada usando ExecuteUpdate (bulk update).
    /// </summary>
    Task<int> UpdateAsync<TEntity>(
        Expression<Func<TEntity, bool>> condition,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> fields,
        CancellationToken cancellationToken = default) 
        where TEntity : BaseEntity;

    /// <summary>
    /// Atualiza um registro específico por seu ID usando ExecuteUpdate (bulk update).
    /// </summary>
    Task<int> UpdateByIdAsync<TEntity>(
        long id,
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> fields,
        CancellationToken cancellationToken = default)
        where TEntity : BaseEntity;

    /// <summary>
    /// Deleta (remove) registros que satisfazem a condição especificada usando ExecuteDelete (bulk delete).
    /// </summary>
    Task<int> DeleteAsync<TEntity>(
        Expression<Func<TEntity, bool>> condition,
        CancellationToken cancellationToken = default) 
        where TEntity : BaseEntity;

    /// <summary>
    /// Deleta (remove) um registro específico por seu ID.
    /// </summary>
    Task<int> DeleteByIdAsync<TEntity>(
        long id,
        CancellationToken cancellationToken = default)
        where TEntity : BaseEntity;

    #endregion
}