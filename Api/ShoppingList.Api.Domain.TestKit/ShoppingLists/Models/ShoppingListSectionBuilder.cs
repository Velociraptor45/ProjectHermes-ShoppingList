﻿using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Common;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Models
{
    public class ShoppingListSectionBuilder : DomainTestBuilderBase<ShoppingListSection>
    {
        public ShoppingListSectionBuilder WithId(SectionId id)
        {
            FillContructorWith("id", id);
            return this;
        }

        public ShoppingListSectionBuilder WithItems(IEnumerable<IShoppingListItem> items)
        {
            FillContructorWith("shoppingListItems", items);
            return this;
        }

        public ShoppingListSectionBuilder WithItem(IShoppingListItem item)
        {
            return WithItems(item.ToMonoList());
        }

        public ShoppingListSectionBuilder WithoutItems()
        {
            return WithItems(Enumerable.Empty<IShoppingListItem>());
        }
    }
}