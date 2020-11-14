﻿using ShoppingList.Api.Contracts.SharedContracts;
using ShoppingList.Api.Domain.Queries.SharedModels;
using ShoppingList.Api.Endpoint.Extensions.ItemCategory;
using ShoppingList.Api.Endpoint.Extensions.Manufacturer;
using System.Linq;

namespace ShoppingList.Api.Endpoint.Extensions.Item
{
    public static class StoreItemReadModelExtensions
    {
        public static StoreItemContract ToContract(this StoreItemReadModel readModel)
        {
            return new StoreItemContract(readModel.Id.Value, readModel.Name, readModel.IsDeleted, readModel.Comment,
                readModel.IsTemporary, (int)readModel.QuantityType, readModel.QuantityInPacket,
                (int)readModel.QuantityTypeInPacket, readModel.ItemCategory.ToContract(),
                readModel.Manufacturer.ToContract(), readModel.Availabilities.Select(av => av.ToContract()));
        }
    }
}