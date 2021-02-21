﻿using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Queries.SharedModels;

namespace ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Extensions
{
    public static class StoreItemAvailabilityExtensions
    {
        public static StoreItemAvailabilityReadModel ToReadModel(this IStoreItemAvailability model)
        {
            return new StoreItemAvailabilityReadModel(model.Store.ToReadModel(), model.Price,
                model.DefaultSection.ToReadModel());
        }
    }
}