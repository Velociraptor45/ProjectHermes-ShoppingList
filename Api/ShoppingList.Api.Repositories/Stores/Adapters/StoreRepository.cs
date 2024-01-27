using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.DomainEventHandlers;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Repositories.Stores.Contexts;
using Store = ProjectHermes.ShoppingList.Api.Repositories.Stores.Entities.Store;

namespace ProjectHermes.ShoppingList.Api.Repositories.Stores.Adapters;

public class StoreRepository : IStoreRepository
{
    private readonly StoreContext _dbContext;
    private readonly IToDomainConverter<Store, IStore> _toDomainConverter;
    private readonly IToContractConverter<IStore, Store> _toContractConverter;
    private readonly ILogger<StoreRepository> _logger;
    private readonly CancellationToken _cancellationToken;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    public StoreRepository(StoreContext dbContext,
        IToDomainConverter<Store, IStore> toDomainConverter,
        IToContractConverter<IStore, Store> toContractConverter,
        Func<CancellationToken, IDomainEventDispatcher> domainEventDispatcherDelegate,
        ILogger<StoreRepository> logger,
        CancellationToken cancellationToken)
    {
        _dbContext = dbContext;
        _toDomainConverter = toDomainConverter;
        _toContractConverter = toContractConverter;
        _domainEventDispatcher = domainEventDispatcherDelegate(cancellationToken);
        _logger = logger;
        _cancellationToken = cancellationToken;
    }

    public async Task<IStore?> FindByAsync(StoreId id)
    {
        var entity = await GetStoreQuery()
            .FirstOrDefaultAsync(store => store.Id == id, _cancellationToken);

        return entity == null ? null : _toDomainConverter.ToDomain(entity);
    }

    public async Task<IEnumerable<IStore>> GetActiveAsync()
    {
        var storeEntities = await GetStoreQuery()
            .Where(store => !store.Deleted)
            .ToListAsync(_cancellationToken);

        return _toDomainConverter.ToDomain(storeEntities);
    }

    public async Task<IStore?> FindActiveByAsync(SectionId sectionId)
    {
        var entity = await GetStoreQuery()
            .Where(store => !store.Deleted)
            .FirstOrDefaultAsync(store => store.Sections.Any(s => s.Id == sectionId), _cancellationToken);

        if (entity == null)
            return null;

        return _toDomainConverter.ToDomain(entity);
    }

    public async Task<IStore?> FindActiveByAsync(StoreId id)
    {
        var entity = await GetStoreQuery()
            .Where(store => !store.Deleted)
            .FirstOrDefaultAsync(store => store.Id == id, _cancellationToken);

        if (entity == null)
            return null;

        return _toDomainConverter.ToDomain(entity);
    }

    public async Task<IEnumerable<IStore>> FindActiveByAsync(IEnumerable<StoreId> ids)
    {
        var idsList = ids.Select(id => id.Value).ToList();

        var query = GetStoreQuery()
            .Where(store => !store.Deleted && idsList.Contains(store.Id));

        var entities = await query.ToListAsync(_cancellationToken);

        return _toDomainConverter.ToDomain(entities);
    }

    public async Task<IStore> StoreAsync(IStore store)
    {
        var existingEntity = await FindTrackedEntityById(store.Id);

        if (existingEntity is null)
        {
            return await StoreAsNew(store);
        }

        var updatedEntity = _toContractConverter.ToContract(store);

        var existingRowVersion = existingEntity.RowVersion;
        _dbContext.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
        _dbContext.Entry(existingEntity).Property(r => r.RowVersion).OriginalValue = existingRowVersion;
        _dbContext.Entry(existingEntity).State = EntityState.Modified;

        AddOrUpdateSections(existingEntity, updatedEntity);
        DeleteSections(existingEntity, updatedEntity);

        try
        {
            await _dbContext.SaveChangesAsync(_cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogInformation(ex, () => "Saving store '{StoreId}' failed due to concurrency violation",
                store.Id.Value);
            throw new DomainException(new ModelOutOfDateReason());
        }
        await ((AggregateRoot)store).DispatchDomainEvents(_domainEventDispatcher);

        return _toDomainConverter.ToDomain(existingEntity);
    }

    #region private methods

    private void AddOrUpdateSections(Store existingStore, Store updatedStore)
    {
        foreach (var updatedSection in updatedStore.Sections)
        {
            var existingSection = existingStore.Sections
                .FirstOrDefault(t => t.Id == updatedSection.Id);

            if (existingSection == null)
            {
                existingStore.Sections.Add(updatedSection);
            }
            else
            {
                _dbContext.Entry(existingSection).CurrentValues.SetValues(updatedSection);
            }
        }
    }

    private void DeleteSections(Store existingStore, Store updatedStore)
    {
        var deletedSections = existingStore.Sections
            .Where(t => updatedStore.Sections.All(s => s.Id != t.Id))
            .ToList();

        foreach (var deletedSection in deletedSections)
        {
            _dbContext.Remove(deletedSection);
        }
    }

    private async Task<IStore> StoreAsNew(IStore store)
    {
        var entity = _toContractConverter.ToContract(store);
        _dbContext.Entry(entity).State = EntityState.Added;

        foreach (var section in entity.Sections)
        {
            _dbContext.Entry(section).State = EntityState.Added;
        }

        await _dbContext.SaveChangesAsync(_cancellationToken);

        return _toDomainConverter.ToDomain(entity);
    }

    private async Task<Store?> FindTrackedEntityById(Guid id)
    {
        return await _dbContext.Stores
            .Include(s => s.Sections)
            .FirstOrDefaultAsync(store => store.Id == id, _cancellationToken);
    }

    private IQueryable<Store> GetStoreQuery()
    {
        return _dbContext.Stores.AsNoTracking()
            .Include(s => s.Sections);
    }

    #endregion private methods
}