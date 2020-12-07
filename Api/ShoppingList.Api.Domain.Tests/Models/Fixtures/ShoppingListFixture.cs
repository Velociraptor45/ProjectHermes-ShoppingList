﻿using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Tests;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Models.Fixtures
{
    public class ShoppingListFixture
    {
        private readonly ShoppingListItemFixture shoppingListItemFixture;
        private readonly CommonFixture commonFixture;

        public ShoppingListFixture(ShoppingListItemFixture shoppingListItemFixture, CommonFixture commonFixture)
        {
            this.shoppingListItemFixture = shoppingListItemFixture;
            this.commonFixture = commonFixture;
        }

        public Domain.ShoppingLists.Models.ShoppingList GetShoppingList(int itemCount = 3,
            IEnumerable<ShoppingListItem> additionalItems = null)
        {
            var listId = new ShoppingListId(commonFixture.NextInt());
            var storeId = new StoreId(commonFixture.NextInt());
            return GetShoppingList(listId, storeId, itemCount, additionalItems);
        }

        public Domain.ShoppingLists.Models.ShoppingList GetShoppingList(StoreId storeId, int itemCount = 3,
            IEnumerable<ShoppingListItem> additionalItems = null)
        {
            var listId = new ShoppingListId(commonFixture.NextInt());
            return GetShoppingList(listId, storeId, itemCount, additionalItems);
        }

        public Domain.ShoppingLists.Models.ShoppingList GetShoppingList(ShoppingListId id, StoreId storeId, int itemCount = 3,
            IEnumerable<ShoppingListItem> additionalItems = null)
        {
            var allItems = additionalItems?.ToList() ?? new List<ShoppingListItem>();
            var additionalItemIds = allItems.Select(i => i.Id.Actual.Value);
            var uniqueItems = GetUniqueShoppingListItems(itemCount, additionalItemIds);
            allItems.AddRange(uniqueItems);
            allItems.Shuffle();

            var fixture = commonFixture.GetNewFixture();
            fixture.Inject(id);
            fixture.Inject(storeId);
            fixture.Inject(allItems.AsEnumerable());
            return fixture.Create<Domain.ShoppingLists.Models.ShoppingList>();
        }

        private IEnumerable<ShoppingListItem> GetUniqueShoppingListItems(int count, IEnumerable<int> exclude)
        {
            List<int> itemIds = commonFixture.NextUniqueInts(count, exclude).ToList();
            List<ShoppingListItem> items = new List<ShoppingListItem>();

            foreach (var itemId in itemIds)
            {
                var item = shoppingListItemFixture.GetShoppingListItemWithId(itemId);
                items.Add(item);
            }
            return items;
        }
    }
}