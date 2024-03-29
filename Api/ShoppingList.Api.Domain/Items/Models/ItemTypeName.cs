﻿using ProjectHermes.ShoppingList.Api.Domain.Shared.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Models;

public record ItemTypeName : Name
{
    public ItemTypeName(string value) : base(value)
    {
    }
}