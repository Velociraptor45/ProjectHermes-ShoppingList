﻿using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories
{
    public class ShoppingListFactory : IShoppingListFactory
    {
        private readonly IShoppingListSectionFactory shoppingListSectionFactory;

        public ShoppingListFactory(IShoppingListSectionFactory shoppingListSectionFactory)
        {
            this.shoppingListSectionFactory = shoppingListSectionFactory;
        }

        public IShoppingList Create(ShoppingListId id, StoreId storeId, DateTime? completionDate,
            IEnumerable<IShoppingListSection> sections)
        {
            return new ShoppingList(id, storeId, completionDate, sections);
        }

        public IShoppingList CreateNew(IStore store)
        {
            var sections = store.Sections.Select(s => shoppingListSectionFactory.CreateEmpty(s));

            return new ShoppingList(new ShoppingListId(0), store.Id, null, sections);
        }
    }
}