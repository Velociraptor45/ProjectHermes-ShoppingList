﻿using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Infrastructure.ShoppingLists.Converters.ToEntity
{
    public class ShoppingListConverter : IToEntityConverter<IShoppingList, Entities.ShoppingList>
    {
        public Entities.ShoppingList ToEntity(IShoppingList source)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            return new Entities.ShoppingList()
            {
                Id = source.Id.Value,
                CompletionDate = source.CompletionDate,
                StoreId = source.StoreId.Value,
                ItemsOnList = CreateItemsOnListMap(source).ToList()
            };
        }

        private IEnumerable<ItemsOnList> CreateItemsOnListMap(IShoppingList source)
        {
            foreach (var section in source.Sections)
            {
                foreach (var item in section.Items)
                {
                    yield return new ItemsOnList()
                    {
                        ShoppingListId = source.Id.Value,
                        ItemId = item.Id.Value,
                        InBasket = item.IsInBasket,
                        Quantity = item.Quantity,
                        SectionId = section.Id.Value
                    };
                }
            }
        }
    }
}