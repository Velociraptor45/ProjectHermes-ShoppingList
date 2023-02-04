﻿using ProjectHermes.ShoppingList.Frontend.Redux.Manufacturers.States;

namespace ShoppingList.Frontend.Redux.Items.Actions.Editor.ManufacturerSelectors;
public record SearchManufacturerFinishedAction(IReadOnlyCollection<ManufacturerSearchResult> SearchResults);