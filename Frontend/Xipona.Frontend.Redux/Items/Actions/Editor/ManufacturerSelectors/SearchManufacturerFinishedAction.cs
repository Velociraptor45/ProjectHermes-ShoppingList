﻿using ProjectHermes.Xipona.Frontend.Redux.Manufacturers.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Items.Actions.Editor.ManufacturerSelectors;
public record SearchManufacturerFinishedAction(IReadOnlyCollection<ManufacturerSearchResult> SearchResults);