﻿namespace ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;
public record QuantityType(int Id, string Name, int DefaultQuantity, string PriceLabel, string QuantityLabel,
    int QuantityNormalizer);