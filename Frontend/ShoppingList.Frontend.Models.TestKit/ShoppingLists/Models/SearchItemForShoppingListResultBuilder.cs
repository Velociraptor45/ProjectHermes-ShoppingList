﻿using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;
using ShoppingList.Frontend.Models.TestKit.Common;

namespace ShoppingList.Frontend.Models.TestKit.ShoppingLists.Models;

public class SearchItemForShoppingListResultBuilder : DomainTestBuilderBase<SearchItemForShoppingListResult>
{
    public SearchItemForShoppingListResultBuilder WithPriceLabel(string priceLabel)
    {
        FillConstructorWith(nameof(priceLabel), priceLabel);
        return this;
    }

    public SearchItemForShoppingListResultBuilder WithPrice(float price)
    {
        FillConstructorWith(nameof(price), price);
        return this;
    }

    public SearchItemForShoppingListResultBuilder WithManufacturerName(string manufacturerName)
    {
        FillConstructorWith(nameof(manufacturerName), manufacturerName);
        return this;
    }

    public SearchItemForShoppingListResultBuilder WithName(string name)
    {
        FillConstructorWith(nameof(name), name);
        return this;
    }
}