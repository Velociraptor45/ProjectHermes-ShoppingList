﻿using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Queries.SharedModels;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters.ToContract.ItemCategories
{
    public class ItemCategoryContractConverter : IToContractConverter<ItemCategoryReadModel, ItemCategoryContract>
    {
        public ItemCategoryContract ToContract(ItemCategoryReadModel source)
        {
            if (source is null)
                throw new System.ArgumentNullException(nameof(source));

            return new ItemCategoryContract(source.Id.Value, source.Name, source.IsDeleted);
        }
    }
}