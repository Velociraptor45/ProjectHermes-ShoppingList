using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.DomainEventHandlers;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Contexts;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Entities;
using Item = ProjectHermes.ShoppingList.Api.Repositories.Items.Entities.Item;

namespace ProjectHermes.ShoppingList.Api.Repositories.Items.Adapters;

public class ItemRepository : IItemRepository
{
    private readonly ItemContext _dbContext;
    private readonly IToDomainConverter<Item, IItem> _toModelConverter;
    private readonly IToContractConverter<IItem, Item> _toContractConverter;
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly ILogger<ItemRepository> _logger;
    private readonly CancellationToken _cancellationToken;

    public ItemRepository(ItemContext dbContext,
        IToDomainConverter<Item, IItem> toModelConverter,
        IToContractConverter<IItem, Item> toContractConverter,
        IDomainEventDispatcher domainEventDispatcher,
        ILogger<ItemRepository> logger,
        CancellationToken cancellationToken)
    {
        _dbContext = dbContext;
        _toModelConverter = toModelConverter;
        _toContractConverter = toContractConverter;
        _domainEventDispatcher = domainEventDispatcher;
        _logger = logger;
        _cancellationToken = cancellationToken;
    }

    #region public methods

    public async Task<IEnumerable<IItem>> FindByAsync(IEnumerable<ItemId> itemIds)
    {
        var idList = itemIds.Select(id => id.Value).ToList();

        var entities = await GetItemQuery()
            .Where(item => idList.Contains(item.Id))
            .ToListAsync(_cancellationToken);

        return _toModelConverter.ToDomain(entities);
    }

    public async Task<IEnumerable<IItem>> FindPermanentByAsync(IEnumerable<StoreId> storeIds,
        IEnumerable<ItemCategoryId> itemCategoriesIds, IEnumerable<ManufacturerId> manufacturerIds)
    {
        var storeIdLists = storeIds.Select(id => id.Value).ToList();
        var itemCategoryIdLists = itemCategoriesIds.Select(id => id.Value).ToList();
        var manufacturerIdLists = manufacturerIds.Select(id => id.Value).ToList();

        var result = await GetItemQuery()
            .Where(item =>
                !item.IsTemporary
                && !item.Deleted
                && itemCategoryIdLists.Contains(item.ItemCategoryId!.Value)
                && ((!item.ManufacturerId.HasValue && !manufacturerIdLists.Any())
                    || manufacturerIdLists.Contains(item.ManufacturerId!.Value)))
            .ToListAsync(_cancellationToken);

        // filtering by store
        var filteredResultByStore = result
            .Where(item => (!item.AvailableAt.Any() && !storeIdLists.Any())
                           || storeIdLists.Intersect(item.AvailableAt.Select(av => av.StoreId)).Any())
            .ToList();

        return _toModelConverter.ToDomain(filteredResultByStore);
    }

    public async Task<IEnumerable<IItem>> FindActiveByAsync(ManufacturerId manufacturerId)
    {
        var entities = await GetItemQuery()
            .Where(i => i.ManufacturerId == manufacturerId.Value && !i.Deleted)
            .ToListAsync(_cancellationToken);

        return _toModelConverter.ToDomain(entities);
    }

    public async Task<IEnumerable<IItem>> FindActiveByAsync(string searchInput, StoreId storeId,
        IEnumerable<ItemId> excludedItemIds, int? limit)
    {
        var excludedRawItemIds = excludedItemIds.Select(id => id.Value).ToList();
        var query = GetItemQuery()
            .Where(item =>
                !item.Deleted
                && !item.IsTemporary
                && !excludedRawItemIds.Contains(item.Id)
                && item.Name.Contains(searchInput)
                && (item.AvailableAt.Any(map => map.StoreId == storeId)
                    || item.ItemTypes.Any(t => !t.IsDeleted && t.AvailableAt.Any(av => av.StoreId == storeId))));

        if (limit.HasValue)
            query = query.Take(limit.Value);

        var entities = await query.ToListAsync(_cancellationToken);

        return _toModelConverter.ToDomain(entities);
    }

    public async Task<IEnumerable<IItem>> FindActiveByAsync(StoreId storeId)
    {
        var entities = await GetItemQuery()
            .Where(item => !item.Deleted
                           && (item.AvailableAt.Any(av => av.StoreId == storeId)
                               || item.ItemTypes.Any(t => !t.IsDeleted && t.AvailableAt.Any(av => av.StoreId == storeId))))
            .ToListAsync(_cancellationToken);

        return _toModelConverter.ToDomain(entities);
    }

    public async Task<IItem?> FindActiveByAsync(TemporaryItemId temporaryItemId)
    {
        var itemEntity = await GetItemQuery()
            .FirstOrDefaultAsync(item =>
                    !item.Deleted && item.CreatedFrom.HasValue && item.CreatedFrom == temporaryItemId,
                _cancellationToken);

        if (itemEntity == null)
            return null;

        return _toModelConverter.ToDomain(itemEntity);
    }

    public async Task<IEnumerable<IItem>> FindActiveByAsync(string searchInput)
    {
        var entities = await GetItemQuery()
            .Where(item =>
                !item.Deleted
                && !item.IsTemporary
                && item.Name.Contains(searchInput))
            .ToListAsync(_cancellationToken);

        return _toModelConverter.ToDomain(entities);
    }

    public async Task<IEnumerable<IItem>> FindActiveByAsync(ItemCategoryId itemCategoryId)
    {
        var entities = await GetItemQuery()
            .Where(item => item.ItemCategoryId.HasValue
                           && item.ItemCategoryId == itemCategoryId
                           && !item.Deleted)
            .ToListAsync(_cancellationToken);

        return _toModelConverter.ToDomain(entities);
    }

    public async Task<IItem?> FindActiveByAsync(ItemId itemId)
    {
        var itemEntity = await GetItemQuery()
            .FirstOrDefaultAsync(item => !item.Deleted && item.Id == itemId, _cancellationToken);

        if (itemEntity == null)
            return null;

        return _toModelConverter.ToDomain(itemEntity);
    }

    public async Task<IItem?> FindActiveByAsync(ItemId itemId, ItemTypeId? itemTypeId)
    {
        var query = GetItemQuery()
            .Where(item => item.ItemCategoryId.HasValue
                           && item.Id == itemId
                           && !item.Deleted);

        if (itemTypeId is null)
            query = query.Where(i => !i.ItemTypes.Any());
        else
            query = query.Where(i => i.ItemTypes.Any(t => t.Id == itemTypeId.Value));

        var entity = await query.FirstOrDefaultAsync(_cancellationToken);

        if (entity is null)
            return null;

        return _toModelConverter.ToDomain(entity);
    }

    public async Task<IEnumerable<IItem>> FindActiveByAsync(SectionId sectionId)
    {
        var items = await GetItemQuery()
            .Where(item => !item.Deleted
                           && (item.AvailableAt.Any(av => av.DefaultSectionId == sectionId)
                               || item.ItemTypes.Any(t =>
                                   t.AvailableAt.Any(av => av.DefaultSectionId == sectionId))))
            .ToListAsync(_cancellationToken);

        return _toModelConverter.ToDomain(items);
    }

    public async Task<IEnumerable<IItem>> FindActiveByAsync(IEnumerable<ItemId> itemIds)
    {
        var rawItemIds = itemIds.Select(id => id.Value).ToArray();

        var items = await GetItemQuery()
            .Where(item => item.ItemCategoryId.HasValue
                           && rawItemIds.Contains(item.Id)
                           && !item.Deleted)
            .ToListAsync(_cancellationToken);

        return _toModelConverter.ToDomain(items);
    }

    public async Task<IEnumerable<IItem>> FindActiveByAsync(IEnumerable<ItemCategoryId> itemCategoryIds,
        StoreId storeId, IEnumerable<ItemId> excludedItemIds)
    {
        var excludedRawItemIds = excludedItemIds.Select(id => id.Value).ToList();
        var rawItemCategoryIds = itemCategoryIds.Select(id => id.Value).ToArray();
        var items = await GetItemQuery()
            .Where(item => item.ItemCategoryId.HasValue
                           && rawItemCategoryIds.Contains(item.ItemCategoryId.Value)
                           && !excludedRawItemIds.Contains(item.Id)
                           && !item.Deleted
                           && (item.AvailableAt.Any(av => av.StoreId == storeId)
                               || item.ItemTypes.Any(t => !t.IsDeleted && t.AvailableAt.Any(av => av.StoreId == storeId))))
            .ToListAsync(_cancellationToken);

        return _toModelConverter.ToDomain(items);
    }

    public async Task<IItem> StoreAsync(IItem item)
    {
        var existingEntity = await FindTrackedEntityBy(item.Id);

        Guid entityIdToLoad;
        if (existingEntity == null)
        {
            var newEntity = _toContractConverter.ToContract(item);
            _dbContext.Add(newEntity);

            await _dbContext.SaveChangesAsync(_cancellationToken);
            entityIdToLoad = newEntity.Id;
        }
        else
        {
            var updatedEntity = _toContractConverter.ToContract(item);
            updatedEntity.Id = existingEntity.Id;

            var existingRowVersion = existingEntity.RowVersion;

            _dbContext.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
            _dbContext.Entry(existingEntity).Property(e => e.RowVersion).OriginalValue = existingRowVersion;
            _dbContext.Entry(existingEntity).State = EntityState.Modified;

            UpdateOrAddAvailabilities(existingEntity, updatedEntity);
            DeleteAvailabilities(existingEntity, updatedEntity);
            UpdateOrAddItemTypes(existingEntity, updatedEntity);
            DeleteItemTypes(existingEntity, updatedEntity);

            try
            {
                await _dbContext.SaveChangesAsync(_cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogInformation(ex, () => "Saving item '{ItemId}' failed due to concurrency violation", item.Id.Value);
                throw new DomainException(new ModelOutOfDateReason());
            }
            entityIdToLoad = updatedEntity.Id;
        }

        await ((AggregateRoot)item).DispatchDomainEvents(_domainEventDispatcher);

        var e = GetItemQuery().First(i => i.Id == entityIdToLoad);
        return _toModelConverter.ToDomain(e);
    }

    #endregion public methods

    #region private methods

    private IQueryable<Item> GetItemQuery()
    {
        return _dbContext.Items.AsNoTracking()
            .Include(item => item.AvailableAt)
            .Include(item => item.ItemTypes)
            .ThenInclude(itemType => itemType.AvailableAt)
            .Include(item => item.ItemTypes);
    }

    private async Task<Item?> FindTrackedEntityBy(ItemId id)
    {
        return await _dbContext.Items
            .Include(item => item.AvailableAt)
            .Include(item => item.ItemTypes)
            .ThenInclude(itemType => itemType.AvailableAt)
            .FirstOrDefaultAsync(i => i.Id == id, _cancellationToken);
    }

    private void UpdateOrAddAvailabilities(Item existing, Item updated)
    {
        foreach (var availability in updated.AvailableAt)
        {
            var existingAvailability = existing.AvailableAt
                .FirstOrDefault(av => MatchesKey(av, availability));

            if (existingAvailability == null)
            {
                existing.AvailableAt.Add(availability);
            }
            else
            {
                _dbContext.Entry(existingAvailability).CurrentValues.SetValues(availability);
            }
        }
    }

    private void DeleteAvailabilities(Item existing, Item updated)
    {
        foreach (var availability in existing.AvailableAt)
        {
            bool hasExistingAvailability = updated.AvailableAt.Any(av => MatchesKey(av, availability));
            if (!hasExistingAvailability)
            {
                _dbContext.Remove(availability);
            }
        }
    }

    private void DeleteItemTypes(Item existing, Item updated)
    {
        foreach (var type in existing.ItemTypes)
        {
            bool hasExistingAvailability = updated.ItemTypes.Any(t => t.Id == type.Id);
            if (!hasExistingAvailability)
            {
                _dbContext.Remove(type);
            }
        }
    }

    private void UpdateOrAddItemTypes(Item existing, Item updated)
    {
        foreach (var updatedType in updated.ItemTypes)
        {
            var existingType = existing.ItemTypes
                .FirstOrDefault(t => t.Id == updatedType.Id);

            if (existingType == null)
            {
                existing.ItemTypes.Add(updatedType);
            }
            else
            {
                _dbContext.Entry(existingType).CurrentValues.SetValues(updatedType);
                UpdateOrAddItemAvailability(existingType, updatedType);
                DeleteItemAvailability(existingType, updatedType);
            }
        }
    }

    private void UpdateOrAddItemAvailability(Entities.ItemType existing, Entities.ItemType updated)
    {
        foreach (var availability in updated.AvailableAt)
        {
            var existingAvailability = existing.AvailableAt.FirstOrDefault(av => MatchesKey(av, availability));

            if (existingAvailability == null)
            {
                existing.AvailableAt.Add(availability);
            }
            else
            {
                _dbContext.Entry(existingAvailability).CurrentValues.SetValues(availability);
            }
        }
    }

    private void DeleteItemAvailability(Entities.ItemType existing, Entities.ItemType updated)
    {
        foreach (var availability in existing.AvailableAt)
        {
            bool hasExistingAvailability = updated.AvailableAt.Any(av => MatchesKey(av, availability));
            if (!hasExistingAvailability)
            {
                _dbContext.Remove(availability);
            }
        }
    }

    private static bool MatchesKey(AvailableAt left, AvailableAt right)
    {
        return left.StoreId == right.StoreId && left.ItemId == right.ItemId;
    }

    private static bool MatchesKey(ItemTypeAvailableAt left, ItemTypeAvailableAt right)
    {
        return left.StoreId == right.StoreId && left.ItemTypeId == right.ItemTypeId;
    }

    #endregion private methods
}