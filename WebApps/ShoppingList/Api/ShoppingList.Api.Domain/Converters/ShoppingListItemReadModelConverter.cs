﻿using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Queries.SharedModels;

namespace ShoppingList.Api.Domain.Converters
{
    public static class ShoppingListItemReadModelConverter
    {
        public static ShoppingListItemReadModel ToReadModel(this ShoppingListItem model)
        {
            return new ShoppingListItemReadModel(model.Id, model.Name, model.IsDeleted, model.Comment,
                model.IsTemporary, model.PricePerQuantity, model.QuantityType, model.QuantityInPacket, model.DefaultQuantity,
                model.QuantityLabel, model.PriceLabel, model.QuantityTypeInPacket,
                model.ItemCategory.ToReadModel(), model.Manufacturer.ToReadModel(),
                model.IsInBasket, model.Quantity);
        }
    }
}