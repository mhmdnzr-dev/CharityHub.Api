﻿namespace CharityHub.Infra.Sql.Primitives;

using CharityHub.Core.Contract.Primitives.Repositories;
using CharityHub.Infra.Sql.Data.DbContexts;

using Core.Domain.Entities.Common;

using EFCore.BulkExtensions;

using Microsoft.EntityFrameworkCore;

public class CommandRepository<T>(CharityHubCommandDbContext commandDbContext)
    : ICommandRepository<T> where T : BaseEntity
{
    protected readonly CharityHubCommandDbContext _commandDbContext = commandDbContext;

    #region Sync Methods

    public void InsertRange(IEnumerable<T> entities)
    {
        _commandDbContext.BulkInsert(entities);
    }


    public void Insert(T entity)
    {
        _commandDbContext.Set<T>().Attach(entity);
        _commandDbContext.SaveChanges();
    }

    public void Update(T entity)
    {
        _commandDbContext.Set<T>().Update(entity);
        _commandDbContext.SaveChanges();
    }

    public void Delete(int id)
    {
        var dbSet = _commandDbContext.Set<T>();
        var entity = dbSet.Find(id);

        var entityType = typeof(T);

        // Ensure the entity has an "IsActive" property before updating
        var isActiveProperty = entityType.GetProperty("IsActive");
        if (isActiveProperty == null)
            throw new InvalidOperationException($"Entity {entityType.Name} does not have an 'IsActive' property.");

        // Set "IsActive" to false
        isActiveProperty.SetValue(entity, false);

        // Perform soft delete
        _commandDbContext.Update(entity);
        _commandDbContext.SaveChanges();
    }

    #endregion

    #region Async Methods

    public async Task InsertRangeAsync(IEnumerable<T> entities)
    {
        await _commandDbContext.BulkInsertAsync(entities);
    }


    public async Task InsertAsync(T entity)
    {
        await _commandDbContext.Set<T>().AddAsync(entity);
        await _commandDbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _commandDbContext.Set<T>().Update(entity);
        await _commandDbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var dbSet = _commandDbContext.Set<T>();
        var entity = dbSet.Find(id);

        var entityType = typeof(T);

        var isActiveProperty = entityType.GetProperty("IsActive");
        if (isActiveProperty == null)
            throw new InvalidOperationException($"Entity {entityType.Name} does not have an 'IsActive' property.");


        isActiveProperty.SetValue(entity, false);


        _commandDbContext.Update(entity);
        await _commandDbContext.SaveChangesAsync();
    }

    #endregion
}