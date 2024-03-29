﻿namespace ProjectHermes.ShoppingList.Frontend.Redux.Manufacturers.States;

public record ManufacturerSearch(
    bool IsLoadingSearchResults,
    bool TriggeredAtLeastOnce,
    IList<ManufacturerSearchResult> SearchResults);