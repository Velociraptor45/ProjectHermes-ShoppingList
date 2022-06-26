﻿using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.Shared;
using System;
using System.Collections.Generic;

namespace ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItemWithTypes
{
    public class UpdateItemTypeContract
    {
        public UpdateItemTypeContract(Guid oldId, string name, IEnumerable<ItemAvailabilityContract> availabilities)
        {
            OldId = oldId;
            Name = name;
            Availabilities = availabilities;
        }

        public Guid OldId { get; set; }
        public string Name { get; set; }
        public IEnumerable<ItemAvailabilityContract> Availabilities { get; set; }
    }
}