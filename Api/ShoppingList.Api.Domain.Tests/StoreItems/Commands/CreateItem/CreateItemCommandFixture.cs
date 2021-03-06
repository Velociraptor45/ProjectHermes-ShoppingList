﻿using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Commands.CreateItem
{
    public class CreateItemCommandFixture
    {
        private readonly CommonFixture commonFixture;

        public CreateItemCommandFixture(CommonFixture commonFixture)
        {
            this.commonFixture = commonFixture;
        }

        public CreateItemCommand GetCreateItemCommand(ItemCategoryId itemCategoryId, ManufacturerId manufacturerId,
            IEnumerable<IStoreItemAvailability> availabilities = null)
        {
            var fixture = commonFixture.GetNewFixture();

            fixture.ConstructorArgumentFor<ItemCreation, ManufacturerId>("manufacturerId", manufacturerId);
            fixture.ConstructorArgumentFor<ItemCreation, ItemCategoryId>("itemCategoryId", itemCategoryId);
            if (availabilities != null)
            {
                var avs = new List<ShortAvailability>();
                foreach (var availability in availabilities)
                {
                    avs.Add(GetShortAvailability(availability));
                }
                fixture.ConstructorArgumentFor<ItemCreation, IEnumerable<ShortAvailability>>("availabilities", avs);
            }

            return fixture.Create<CreateItemCommand>();
        }

        private ShortAvailability GetShortAvailability(IStoreItemAvailability availability)
        {
            var fixture = commonFixture.GetNewFixture();

            fixture.ConstructorArgumentFor<ShortAvailability, StoreItemStoreId>("storeId", availability.Store.Id);
            fixture.ConstructorArgumentFor<ShortAvailability, float>("price", availability.Price);
            fixture.ConstructorArgumentFor<ShortAvailability, StoreItemSectionId>("storeItemSectionId", availability.DefaultSection.Id);

            return fixture.Create<ShortAvailability>();
        }
    }
}