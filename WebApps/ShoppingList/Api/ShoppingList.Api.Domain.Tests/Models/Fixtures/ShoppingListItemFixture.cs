﻿using AutoFixture;
using ShoppingList.Api.Core.Tests.AutoFixture;
using ShoppingList.Api.Domain.Models;
using System;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Models.Fixtures
{
    public class ShoppingListItemFixture
    {
        private readonly CommonFixture commonFixture;

        public ShoppingListItemFixture(CommonFixture commonFixture)
        {
            this.commonFixture = commonFixture;
        }

        public ShoppingListItem GetShoppingListItemWithId(ShoppingListItemId id)
        {
            var fixture = commonFixture.GetNewFixture();
            fixture.Inject(id);
            return fixture.Create<ShoppingListItem>();
        }

        public ShoppingListItem GetShoppingListItemWithId(int id)
        {
            return GetShoppingListItemWithId(new ShoppingListItemId(id));
        }

        public ShoppingListItem GetShoppingListItemWithId(Guid id)
        {
            return GetShoppingListItemWithId(new ShoppingListItemId(id));
        }

        public ShoppingListItem GetShoppingListItemWithId()
        {
            return GetShoppingListItemWithId(commonFixture.NextInt());
        }

        public ShoppingListItem GetShoppingListItem(bool? isInBasket = null, bool? isTemporary = null,
            bool? isDeleted = null)
        {
            var fixture = commonFixture.GetNewFixture();

            if (isInBasket.HasValue)
                fixture.ConstructorArgumentFor<ShoppingListItem, bool>("isInBasket", isInBasket.Value);
            if (isTemporary.HasValue)
                fixture.ConstructorArgumentFor<ShoppingListItem, bool>("isTemporary", isTemporary.Value);
            if (isDeleted.HasValue)
                fixture.ConstructorArgumentFor<ShoppingListItem, bool>("isDeleted", isDeleted.Value);

            return fixture.Create<ShoppingListItem>();
        }
    }
}