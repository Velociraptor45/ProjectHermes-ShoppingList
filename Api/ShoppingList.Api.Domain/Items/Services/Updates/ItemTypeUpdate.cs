﻿using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Services.Updates;

public class ItemTypeUpdate
{
    public ItemTypeUpdate(ItemTypeId oldId, ItemTypeName name, IEnumerable<ItemAvailability> availabilities)
    {
        OldId = oldId;
        Name = name;
        Availabilities = availabilities.ToArray();
    }

    public ItemTypeId OldId { get; }
    public ItemTypeName Name { get; }
    public IReadOnlyCollection<ItemAvailability> Availabilities { get; }
}