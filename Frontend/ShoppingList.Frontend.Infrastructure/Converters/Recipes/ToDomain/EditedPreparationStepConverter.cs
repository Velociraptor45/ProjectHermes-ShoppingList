﻿using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.Get;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using System;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Recipes.ToDomain;

public class EditedPreparationStepConverter : IToDomainConverter<PreparationStepContract, EditedPreparationStep>
{
    public EditedPreparationStep ToDomain(PreparationStepContract source)
    {
        return new EditedPreparationStep(
            Guid.NewGuid(),
            source.Id,
            source.Instruction,
            source.SortingIndex);
    }
}