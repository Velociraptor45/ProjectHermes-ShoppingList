﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.UpdateItem;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories
{
    public class StoreItemFactory : IStoreItemFactory
    {
        public IStoreItem Create(ItemCreation itemCreation, IItemCategory itemCategory,
            IManufacturer manufacturer, IEnumerable<IStoreItemAvailability> storeItemAvailabilities)
        {
            return new StoreItem(new StoreItemId(0),
                itemCreation.Name,
                false,
                itemCreation.Comment,
                false,
                itemCreation.QuantityType,
                itemCreation.QuantityInPacket,
                itemCreation.QuantityTypeInPacket,
                itemCategory,
                manufacturer,
                storeItemAvailabilities);
        }

        public IStoreItem Create(TemporaryItemCreation model, IStoreItemAvailability storeItemAvailability)
        {
            return new StoreItem(
                new StoreItemId(model.ClientSideId),
                model.Name,
                false,
                string.Empty,
                true,
                QuantityType.Unit,
                1,
                QuantityTypeInPacket.Unit,
                null,
                null,
                new List<IStoreItemAvailability>() { storeItemAvailability });
        }

        public IStoreItem Create(ItemUpdate itemUpdate, IItemCategory itemCategory, IManufacturer manufacturer,
            IStoreItem predecessor, IEnumerable<IStoreItemAvailability> storeItemAvailabilities)
        {
            var model = new StoreItem(new StoreItemId(0),
                itemUpdate.Name,
                false,
                itemUpdate.Comment,
                false,
                itemUpdate.QuantityType,
                itemUpdate.QuantityInPacket,
                itemUpdate.QuantityTypeInPacket,
                itemCategory,
                manufacturer,
                storeItemAvailabilities);
            model.SetPredecessor(predecessor);
            return model;
        }
    }
}