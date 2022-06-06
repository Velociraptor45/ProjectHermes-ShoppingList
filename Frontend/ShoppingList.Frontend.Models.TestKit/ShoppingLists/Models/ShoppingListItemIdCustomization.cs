﻿using AutoFixture;
using AutoFixture.Kernel;
using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;

namespace ShoppingList.Frontend.Models.TestKit.ShoppingLists.Models;

public class ShoppingListItemIdCustomization : ICustomization
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new ShoppingListItemIdSpecimenBuilder());
    }

    private class ShoppingListItemIdSpecimenBuilder : ISpecimenBuilder
    {
        public object Create(object request, ISpecimenContext context)
        {
            if (!MatchesType(request))
                return new NoSpecimen();

            return CreateInstance();
        }

        private bool MatchesType(object request)
        {
            var t = request as Type;
            return typeof(ShoppingListItemId) == t;
        }

        private ShoppingListItemId CreateInstance()
        {
            return ShoppingListItemId.FromActualId(Guid.NewGuid());
        }
    }
}