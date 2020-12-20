﻿using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Tests;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ShoppingList.Api.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures
{
    public class StoreItemFixture
    {
        private readonly StoreItemAvailabilityFixture storeItemAvailabilityFixture;
        private readonly CommonFixture commonFixture;

        public StoreItemFixture(StoreItemAvailabilityFixture storeItemAvailabilityFixture, CommonFixture commonFixture)
        {
            this.storeItemAvailabilityFixture = storeItemAvailabilityFixture;
            this.commonFixture = commonFixture;
        }

        public IStoreItem GetStoreItem(int availabilityCount = 3,
            IEnumerable<IStoreItemAvailability> additionalAvailabilities = null)
        {
            var storeItemId = new StoreItemId(commonFixture.NextInt());
            return GetStoreItem(storeItemId, availabilityCount, additionalAvailabilities);
        }

        public IStoreItem GetStoreItem(StoreItemId id, int availabilityCount = 3,
            IEnumerable<IStoreItemAvailability> additionalAvailabilities = null,
            bool? isTemporary = null, bool? isDeleted = null)
        {
            var allAvailabilities = additionalAvailabilities?.ToList() ?? new List<IStoreItemAvailability>();
            var additionalStoreIds = allAvailabilities.Select(av => av.StoreId.Value);
            var uniqueStoreItemAvailabilities = GetUniqueStoreItemAvailabilities(availabilityCount, additionalStoreIds);
            allAvailabilities.AddRange(uniqueStoreItemAvailabilities);
            allAvailabilities.Shuffle();

            var fixture = commonFixture.GetNewFixture();
            fixture.Inject(id);
            fixture.Inject(allAvailabilities.AsEnumerable());
            if (isTemporary.HasValue)
                fixture.ConstructorArgumentFor<StoreItem, bool>("isTemporary", isTemporary.Value);
            if (isDeleted.HasValue)
                fixture.ConstructorArgumentFor<StoreItem, bool>("isDeleted", isDeleted.Value);
            return fixture.Create<StoreItem>();
        }

        private IEnumerable<IStoreItemAvailability> GetUniqueStoreItemAvailabilities(int count, IEnumerable<int> exclude)
        {
            List<int> storeIds = commonFixture.NextUniqueInts(count, exclude).ToList();
            List<IStoreItemAvailability> availabilities = new List<IStoreItemAvailability>();

            foreach (var storeId in storeIds)
            {
                var availability = storeItemAvailabilityFixture.GetAvailability(storeId);
                availabilities.Add(availability);
            }
            return availabilities;
        }
    }
}