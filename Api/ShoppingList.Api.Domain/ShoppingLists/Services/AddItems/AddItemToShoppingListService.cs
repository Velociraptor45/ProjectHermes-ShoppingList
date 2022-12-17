﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.Shared;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.AddItems;

public class AddItemToShoppingListService : IAddItemToShoppingListService
{
    private readonly IShoppingListSectionFactory _shoppingListSectionFactory;
    private readonly IStoreRepository _storeRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IShoppingListItemFactory _shoppingListItemFactory;
    private readonly IShoppingListRepository _shoppingListRepository;

    public AddItemToShoppingListService(IShoppingListSectionFactory shoppingListSectionFactory,
        IStoreRepository storeRepository, IItemRepository itemRepository,
        IShoppingListItemFactory shoppingListItemFactory, IShoppingListRepository shoppingListRepository)
    {
        _shoppingListSectionFactory = shoppingListSectionFactory;
        _storeRepository = storeRepository;
        _itemRepository = itemRepository;
        _shoppingListItemFactory = shoppingListItemFactory;
        _shoppingListRepository = shoppingListRepository;
    }

    public async Task AddAsync(ShoppingListId shoppingListId, OfflineTolerantItemId itemId, SectionId? sectionId,
        QuantityInBasket quantity, CancellationToken cancellationToken)
    {
        var list = await _shoppingListRepository.FindByAsync(shoppingListId, cancellationToken);
        if (list == null)
            throw new DomainException(new ShoppingListNotFoundReason(shoppingListId));

        cancellationToken.ThrowIfCancellationRequested();

        if (itemId.IsActualId)
        {
            var actualId = new ItemId(itemId.ActualId!.Value);
            await AddItemToShoppingListAsync(list, actualId, sectionId, quantity,
                cancellationToken);
        }
        else
        {
            var temporaryId = new TemporaryItemId(itemId.OfflineId!.Value);
            await AddItemToShoppingListAsync(list, temporaryId, sectionId,
                quantity, cancellationToken);
        }

        cancellationToken.ThrowIfCancellationRequested();

        await _shoppingListRepository.StoreAsync(list, cancellationToken);
    }

    public async Task AddItemWithTypeToShoppingListAsync(ShoppingListId shoppingListId, ItemId itemId, ItemTypeId itemTypeId,
        SectionId? sectionId, QuantityInBasket quantity, CancellationToken cancellationToken)
    {
        var shoppingList = await LoadShoppingListAsync(shoppingListId, cancellationToken);
        var item = await LoadItemAsync(itemId, cancellationToken);

        await AddItemToShoppingListAsync(shoppingList, item, itemTypeId, sectionId, quantity, cancellationToken);

        await _shoppingListRepository.StoreAsync(shoppingList, cancellationToken);
    }

    public async Task AddItemWithTypeToShoppingList(IShoppingList shoppingList, IItem item,
        ItemTypeId itemTypeId, SectionId? sectionId, QuantityInBasket quantity, CancellationToken cancellationToken)
    {
        await AddItemToShoppingListAsync(shoppingList, item, itemTypeId, sectionId, quantity, cancellationToken);

        await _shoppingListRepository.StoreAsync(shoppingList, cancellationToken);
    }

    public async Task AddItemToShoppingListAsync(IShoppingList shoppingList, ItemId itemId, SectionId? sectionId,
        QuantityInBasket quantity, CancellationToken cancellationToken)
    {
        IItem item = await LoadItemAsync(itemId, cancellationToken);
        await AddItemToShoppingListAsync(shoppingList, item, sectionId, quantity, cancellationToken);
    }

    public async Task AddItemToShoppingListAsync(IShoppingList shoppingList, TemporaryItemId temporaryItemId,
        SectionId? sectionId, QuantityInBasket quantity, CancellationToken cancellationToken)
    {
        IItem item = await LoadItemAsync(temporaryItemId, cancellationToken);
        await AddItemToShoppingListAsync(shoppingList, item, sectionId, quantity, cancellationToken);
    }

    private async Task<IShoppingList> LoadShoppingListAsync(ShoppingListId shoppingListId,
        CancellationToken cancellationToken)
    {
        var shoppingList = await _shoppingListRepository.FindByAsync(shoppingListId, cancellationToken);
        if (shoppingList is null)
            throw new DomainException(new ShoppingListNotFoundReason(shoppingListId));

        return shoppingList;
    }

    private async Task<IItem> LoadItemAsync(ItemId itemId, CancellationToken cancellationToken)
    {
        IItem? item = await _itemRepository.FindActiveByAsync(itemId, cancellationToken);
        if (item == null)
            throw new DomainException(new ItemNotFoundReason(itemId));

        return item;
    }

    private async Task<IItem> LoadItemAsync(TemporaryItemId temporaryItemId, CancellationToken cancellationToken)
    {
        IItem? item = await _itemRepository.FindActiveByAsync(temporaryItemId, cancellationToken);
        if (item == null)
            throw new DomainException(new ItemNotFoundReason(temporaryItemId));

        return item;
    }

    private IShoppingListItem CreateShoppingListItem(ItemId itemId, ItemTypeId? itemTypeId, QuantityInBasket quantity)
    {
        return _shoppingListItemFactory.Create(itemId, itemTypeId, false, quantity);
    }

    private void ValidateItemIsAvailableAtStore(IItem item, StoreId storeId,
        out IItemAvailability availability)
    {
        var av = item.Availabilities.FirstOrDefault(av => av.StoreId == storeId);

        availability = av ?? throw new DomainException(new ItemAtStoreNotAvailableReason(item.Id, storeId));
    }

    private void ValidateItemTypeIsAvailableAtStore(IItemType itemType, StoreId storeId,
        out IItemAvailability availability)
    {
        var av = itemType.Availabilities.FirstOrDefault(av => av.StoreId == storeId);

        availability = av ?? throw new DomainException(new ItemTypeAtStoreNotAvailableReason(itemType.Id, storeId));
    }

    internal async Task AddItemToShoppingListAsync(IShoppingList shoppingList, IItem item,
        ItemTypeId itemTypeId, SectionId? sectionId, QuantityInBasket quantity, CancellationToken cancellationToken)
    {
        if (!item.TryGetType(itemTypeId, out var itemType))
            throw new DomainException(new ItemTypeNotPartOfItemReason(item.Id, itemTypeId));

        ValidateItemTypeIsAvailableAtStore(itemType!, shoppingList.StoreId, out var availability);

        sectionId ??= availability.DefaultSectionId;

        cancellationToken.ThrowIfCancellationRequested();

        IShoppingListItem shoppingListItem = CreateShoppingListItem(item.Id, itemTypeId, quantity);
        await AddItemToShoppingListAsync(shoppingList, shoppingListItem, sectionId.Value, cancellationToken);
    }

    internal async Task AddItemToShoppingListAsync(IShoppingList shoppingList, IItem item,
        SectionId? sectionId, QuantityInBasket quantity, CancellationToken cancellationToken)
    {
        if (item.HasItemTypes)
            throw new DomainException(new CannotAddTypedItemToShoppingListWithoutTypeIdReason(item.Id));

        ValidateItemIsAvailableAtStore(item, shoppingList.StoreId, out var availability);

        sectionId ??= availability.DefaultSectionId;

        cancellationToken.ThrowIfCancellationRequested();

        IShoppingListItem shoppingListItem = CreateShoppingListItem(item.Id, null, quantity);
        await AddItemToShoppingListAsync(shoppingList, shoppingListItem, sectionId.Value, cancellationToken);
    }

    internal async Task AddItemToShoppingListAsync(IShoppingList shoppingList, IShoppingListItem item,
        SectionId sectionId, CancellationToken cancellationToken)
    {
        var store = await _storeRepository.FindActiveByAsync(shoppingList.StoreId, cancellationToken);
        if (store == null)
            throw new DomainException(new StoreNotFoundReason(shoppingList.StoreId));

        if (!store.ContainsSection(sectionId))
            throw new DomainException(new SectionInStoreNotFoundReason(sectionId, store.Id));

        cancellationToken.ThrowIfCancellationRequested();

        if (shoppingList.Sections.All(s => s.Id != sectionId))
        {
            var section = _shoppingListSectionFactory.CreateEmpty(sectionId);
            shoppingList.AddSection(section);
        }

        shoppingList.AddItem(item, sectionId);
    }
}