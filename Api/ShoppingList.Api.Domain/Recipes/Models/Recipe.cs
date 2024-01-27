using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Modifications;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Domain.Shared.Validations;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;

public class Recipe : AggregateRoot, IRecipe
{
    private readonly Ingredients _ingredients;
    private readonly PreparationSteps _preparationSteps;
    private readonly RecipeTags _tags;

    public Recipe(RecipeId id, RecipeName name, NumberOfServings numberOfServings, Ingredients ingredients,
        PreparationSteps preparationSteps, RecipeTags tags)
    {
        _ingredients = ingredients;
        _preparationSteps = preparationSteps;
        _tags = tags;
        Id = id;
        Name = name;
        NumberOfServings = numberOfServings;
    }

    public RecipeId Id { get; }
    public RecipeName Name { get; private set; }
    public NumberOfServings NumberOfServings { get; private set; }
    public IReadOnlyCollection<IIngredient> Ingredients => _ingredients.AsReadOnly();
    public IReadOnlyCollection<IPreparationStep> PreparationSteps => _preparationSteps.AsReadOnly();
    public IReadOnlyCollection<RecipeTagId> Tags => _tags.AsReadOnly();

    public async Task ModifyAsync(RecipeModification modification, IValidator validator)
    {
        Name = modification.Name;
        NumberOfServings = modification.NumberOfServings;
        await _ingredients.ModifyManyAsync(modification.IngredientModifications, validator);
        _preparationSteps.ModifyMany(modification.PreparationStepModifications);
        await _tags.ModifyAsync(validator, modification.RecipeTagIds);
    }

    public void RemoveDefaultItem(ItemId defaultItemId, ItemTypeId? itemTypeId)
    {
        _ingredients.RemoveDefaultItem(defaultItemId, itemTypeId);
    }

    public void ModifyIngredientsAfterItemUpdate(ItemId oldItemId, IItem newItem)
    {
        _ingredients.ModifyAfterItemUpdate(oldItemId, newItem);
    }

    public void ModifyIngredientsAfterAvailabilityWasDeleted(ItemId itemId, ItemTypeId? itemTypeId, IItem item,
        StoreId deletedAvailabilityStoreId)
    {
        _ingredients.ModifyAfterAvailabilityWasDeleted(itemId, itemTypeId, item, deletedAvailabilityStoreId);
    }

    public void ModifyIngredientsAfterAvailabilitiesChanged(ItemId itemId, ItemTypeId? itemTypeId,
        IEnumerable<ItemAvailability> newAvailabilities)
    {
        _ingredients.ModifyAfterAvailabilitiesChanged(itemId, itemTypeId, newAvailabilities);
    }

    public void RemoveIngredientsOfItemCategory(ItemCategoryId itemCategoryId)
    {
        _ingredients.RemoveIngredientsOfItemCategory(itemCategoryId);
    }
}