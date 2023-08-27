﻿namespace ProjectHermes.ShoppingList.Api.Domain.Items.Models;

public sealed record Comment(string Value)
{
    public static Comment Empty => new(string.Empty);
}