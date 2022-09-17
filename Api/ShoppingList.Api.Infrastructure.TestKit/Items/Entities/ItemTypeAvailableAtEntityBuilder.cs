﻿using ProjectHermes.ShoppingList.Api.Core.TestKit;
using ProjectHermes.ShoppingList.Api.Infrastructure.Items.Entities;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.TestKit.Items.Entities;

public class ItemTypeAvailableAtEntityBuilder : TestBuilderBase<ItemTypeAvailableAt>
{
    public ItemTypeAvailableAtEntityBuilder()
    {
    }

    public ItemTypeAvailableAtEntityBuilder(ItemTypeAvailableAt availability)
    {
        WithItemTypeId(availability.ItemTypeId);
        WithStoreId(availability.StoreId);
        WithPrice(availability.Price);
        WithDefaultSectionId(availability.DefaultSectionId);
        WithItemType(availability.ItemType);
    }

    public ItemTypeAvailableAtEntityBuilder WithItemTypeId(Guid itemTypeId)
    {
        FillPropertyWith(p => p.ItemTypeId, itemTypeId);
        return this;
    }

    public ItemTypeAvailableAtEntityBuilder WithStoreId(Guid storeId)
    {
        FillPropertyWith(p => p.StoreId, storeId);
        return this;
    }

    public ItemTypeAvailableAtEntityBuilder WithPrice(float price)
    {
        FillPropertyWith(p => p.Price, price);
        return this;
    }

    public ItemTypeAvailableAtEntityBuilder WithDefaultSectionId(Guid defaultSectionId)
    {
        FillPropertyWith(p => p.DefaultSectionId, defaultSectionId);
        return this;
    }

    public ItemTypeAvailableAtEntityBuilder WithItemType(ItemType? itemType)
    {
        FillPropertyWith(p => p.ItemType, itemType);
        return this;
    }

    public ItemTypeAvailableAtEntityBuilder WithoutItemType()
    {
        return WithItemType(null);
    }
}