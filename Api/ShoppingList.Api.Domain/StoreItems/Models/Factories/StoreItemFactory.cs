﻿using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Updates;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;

public class StoreItemFactory : IStoreItemFactory
{
    private readonly IItemTypeFactory _itemTypeFactory;

    public StoreItemFactory(IItemTypeFactory itemTypeFactory)
    {
        _itemTypeFactory = itemTypeFactory;
    }

    public IStoreItem Create(ItemId id, ItemName name, bool isDeleted, Comment comment, bool isTemporary,
        QuantityType quantityType, float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket,
        ItemCategoryId? itemCategoryId, ManufacturerId? manufacturerId, IStoreItem? predecessor,
        IEnumerable<IStoreItemAvailability> availabilities, TemporaryItemId? temporaryId)
    {
        var item = new StoreItem(
            id,
            name,
            isDeleted,
            comment,
            isTemporary,
            quantityType,
            quantityInPacket,
            quantityTypeInPacket,
            itemCategoryId,
            manufacturerId,
            availabilities,
            temporaryId);

        if (predecessor != null)
            item.SetPredecessor(predecessor);

        return item;
    }

    public IStoreItem Create(ItemId id, ItemName name, bool isDeleted, Comment comment,
        QuantityType quantityType, float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket,
        ItemCategoryId itemCategoryId, ManufacturerId? manufacturerId, IStoreItem? predecessor,
        IEnumerable<IItemType> itemTypes)
    {
        var item = new StoreItem(
            id,
            name,
            isDeleted,
            comment,
            quantityType,
            quantityInPacket,
            quantityTypeInPacket,
            itemCategoryId,
            manufacturerId,
            new ItemTypes(itemTypes, _itemTypeFactory));

        if (predecessor != null)
            item.SetPredecessor(predecessor);

        return item;
    }

    public IStoreItem Create(ItemCreation itemCreation)
    {
        return new StoreItem(
            ItemId.New,
            itemCreation.Name,
            false,
            itemCreation.Comment,
            false,
            itemCreation.QuantityType,
            itemCreation.QuantityInPacket,
            itemCreation.QuantityTypeInPacket,
            itemCreation.ItemCategoryId,
            itemCreation.ManufacturerId,
            itemCreation.Availabilities,
            null);
    }

    public IStoreItem Create(TemporaryItemCreation model)
    {
        return new StoreItem(
            ItemId.New,
            model.Name,
            false,
            Comment.Empty,
            true,
            QuantityType.Unit,
            1,
            QuantityTypeInPacket.Unit,
            null,
            null,
            model.Availability.ToMonoList(),
            new TemporaryItemId(model.ClientSideId));
    }

    public IStoreItem Create(ItemUpdate itemUpdate, IStoreItem predecessor)
    {
        var model = new StoreItem(
            ItemId.New,
            itemUpdate.Name,
            isDeleted: false,
            itemUpdate.Comment,
            isTemporary: false,
            itemUpdate.QuantityType,
            itemUpdate.QuantityInPacket,
            itemUpdate.QuantityTypeInPacket,
            itemUpdate.ItemCategoryId,
            itemUpdate.ManufacturerId,
            itemUpdate.Availabilities,
            null);

        model.SetPredecessor(predecessor);
        return model;
    }

    public IStoreItem CreateNew(ItemName name, Comment comment,
        QuantityType quantityType, float quantityInPacket, QuantityTypeInPacket quantityTypeInPacket,
        ItemCategoryId itemCategoryId, ManufacturerId? manufacturerId, IStoreItem? predecessor,
        IEnumerable<IItemType> itemTypes)
    {
        var item = new StoreItem(
            ItemId.New,
            name,
            isDeleted: false,
            comment,
            quantityType,
            quantityInPacket,
            quantityTypeInPacket,
            itemCategoryId,
            manufacturerId,
            new ItemTypes(itemTypes, _itemTypeFactory));

        if (predecessor != null)
            item.SetPredecessor(predecessor);

        return item;
    }
}