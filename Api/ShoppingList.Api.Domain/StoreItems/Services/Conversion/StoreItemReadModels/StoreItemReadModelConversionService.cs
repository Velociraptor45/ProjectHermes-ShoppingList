﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.SharedModels;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Shared;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Queries.Quantities;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Conversion.StoreItemReadModels;

public class StoreItemReadModelConversionService : IStoreItemReadModelConversionService
{
    private readonly IItemCategoryRepository _itemCategoryRepository;
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IStoreRepository _storeRepository;

    public StoreItemReadModelConversionService(IItemCategoryRepository itemCategoryRepository,
        IManufacturerRepository manufacturerRepository, IStoreRepository storeRepository)
    {
        _itemCategoryRepository = itemCategoryRepository;
        _manufacturerRepository = manufacturerRepository;
        _storeRepository = storeRepository;
    }

    public async Task<StoreItemReadModel> ConvertAsync(IStoreItem item, CancellationToken cancellationToken)
    {
        if (item is null)
            throw new ArgumentNullException(nameof(item));

        IItemCategory? itemCategory = null;
        IManufacturer? manufacturer = null;

        if (item.ItemCategoryId != null)
        {
            itemCategory = await _itemCategoryRepository.FindByAsync(item.ItemCategoryId.Value, cancellationToken);
            if (itemCategory == null)
                throw new DomainException(new ItemCategoryNotFoundReason(item.ItemCategoryId.Value));
        }
        if (item.ManufacturerId != null)
        {
            manufacturer = await _manufacturerRepository.FindByAsync(item.ManufacturerId.Value, cancellationToken);
            if (manufacturer == null)
                throw new DomainException(new ManufacturerNotFoundReason(item.ManufacturerId.Value));
        }

        var storeIds = item.Availabilities.Select(av => av.StoreId).ToList();
        storeIds.AddRange(item.ItemTypes.SelectMany(t => t.Availabilities.Select(av => av.StoreId)));
        var storeDict = (await _storeRepository.FindByAsync(storeIds, cancellationToken))
            .ToDictionary(store => store.Id);

        return ToReadModel(item, itemCategory, manufacturer, storeDict);
    }

    public StoreItemReadModel ToReadModel(IStoreItem model, IItemCategory? itemCategory,
        IManufacturer? manufacturer, Dictionary<StoreId, IStore> stores)
    {
        var availabilityReadModels = ToAvailabilityReadModel(model.Availabilities, stores).ToList();

        var itemTypeReadModels = new List<ItemTypeReadModel>();
        foreach (var itemType in model.ItemTypes)
        {
            var itemTypeReadModel = new ItemTypeReadModel(itemType.Id, itemType.Name,
                ToAvailabilityReadModel(itemType.Availabilities, stores));
            itemTypeReadModels.Add(itemTypeReadModel);
        }

        return new StoreItemReadModel(
            model.Id,
            model.Name,
            model.IsDeleted,
            model.Comment,
            model.IsTemporary,
            new QuantityTypeReadModel(model.QuantityType),
            model.QuantityInPacket,
            new QuantityTypeInPacketReadModel(model.QuantityTypeInPacket),
            itemCategory is null ?
                null :
                new ItemCategoryReadModel(itemCategory),
            manufacturer is null ?
                null :
                new ManufacturerReadModel(manufacturer),
            availabilityReadModels,
            itemTypeReadModels);
    }

    private static IEnumerable<StoreItemAvailabilityReadModel> ToAvailabilityReadModel(
        IEnumerable<IStoreItemAvailability> availabilities, Dictionary<StoreId, IStore> stores)
    {
        foreach (var av in availabilities)
        {
            var store = stores[av.StoreId];
            var section = store.Sections.First(s => s.Id == av.DefaultSectionId);

            yield return new StoreItemAvailabilityReadModel(av, store, section);
        }
    }
}