﻿using ProjectHermes.ShoppingList.Frontend.Redux.Manufacturers.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions.Editor.ManufacturerSelectors;
public record SearchManufacturerFinishedAction(IReadOnlyCollection<ManufacturerSearchResult> SearchResults);