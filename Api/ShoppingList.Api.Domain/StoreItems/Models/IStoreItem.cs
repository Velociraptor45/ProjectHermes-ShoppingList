﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.ChangeItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.MakeTemporaryItemPermanent;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models
{
    public interface IStoreItem
    {
        StoreItemId Id { get; }
        string Name { get; }
        bool IsDeleted { get; }
        string Comment { get; }
        bool IsTemporary { get; }
        QuantityType QuantityType { get; }
        float QuantityInPacket { get; }
        QuantityTypeInPacket QuantityTypeInPacket { get; }

        IItemCategory ItemCategory { get; }
        IManufacturer Manufacturer { get; }
        IStoreItem Predecessor { get; }

        IReadOnlyCollection<IStoreItemAvailability> Availabilities { get; }

        void Delete();

        IStoreItemSection GetDefaultSectionForStore(StoreItemStoreId storeId);

        bool IsAvailableInStore(StoreItemStoreId storeId);

        void MakePermanent(PermanentItem permanentItem, IItemCategory itemCategory, IManufacturer manufacturer,
            IEnumerable<IStoreItemAvailability> availabilities);

        void Modify(ItemModify itemChange, IItemCategory itemCategory, IManufacturer manufacturer,
            IEnumerable<IStoreItemAvailability> availabilities);

        void SetPredecessor(IStoreItem predecessor);
    }
}