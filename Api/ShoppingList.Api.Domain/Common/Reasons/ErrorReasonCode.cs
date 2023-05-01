﻿namespace ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;

public enum ErrorReasonCode
{
    ItemNotFound = 0,
    InvalidQuantityInBasket = 2,
    ItemAlreadyOnShoppingList = 3,
    ItemAtStoreNotAvailable = 4,
    ItemCategoryNotFound = 5,
    ItemNotTemporary = 6,
    ItemNotOnShoppingList = 7,
    ManufacturerNotFound = 8,
    ShoppingListAlreadyExists = 9,
    ShoppingListAlreadyFinished = 10,
    ShoppingListNotFound = 11,
    StoreNotFound = 12,
    TemporaryItemNotModifyable = 13,
    TemporaryItemNotUpdateable = 14,
    ItemNotInSection = 15,
    SectionInStoreNotFound = 16,
    ItemAlreadyInSection = 17,
    SectionAlreadyInShoppingList = 20,
    MultipleAvailabilitiesForStore = 21,
    CannotModifyItemAsItemWithTypes = 22,
    CannotAddTypedItemToShoppingListWithoutTypeId = 23,
    CannotRemoveAllTypesFromItemWithTypes = 24,
    CannotCreateItemWithTypesWithoutTypes = 25,
    ItemTypeNotFound = 26,
    ShoppingListItemMissingType = 27,
    ItemTypeNotPartOfItem = 28,
    ItemTypeAtStoreNotAvailable = 29,
    TemporaryItemCannotHaveTypeId = 30,
    CannotUpdateItemAsItemWithTypes = 31,
    ShoppingListItemHasNoType = 32,
    ShoppingListItemHasType = 33,
    SectionNotFound = 34,
    QuantityTypeHasNoInPacketValues = 35,
    QuantityTypeHasInPacketValues = 36,
    PriceNotValid = 37,
    InvalidQuantity = 38,
    IngredientQuantityNotValid = 39,
    DuplicatedSortingIndex = 40,
    StoresNotFound = 41,
    RecipeNotFound = 42,
    IngredientNotFound = 43,
    PreparationStepNotFound = 44,
    InvalidItemIdCombination = 45,
    MultipleDefaultSections = 46,
    CannotUpdateItemWithTypesAsItem = 47,
    OldAndNewSectionNotInSameStore = 48,
    ItemWithTypesHasNoAvailabilities = 49,
    ItemHasNoItemTypes = 50,
    CannotModifyDeletedItemType = 51,
    ModelOutOfDate = 52,
    InvalidRecipeTagIds = 53,
    DefaultIngredientItemHasToHaveDefaultStore = 54,
    CannotCreateItemWithoutAvailabilities = 55,
    CannotUpdateItemWithoutAvailabilities = 56,
    CannotModifyItemWithoutAvailabilities = 57,
    CannotCreateItemTypeWithoutAvailabilities = 58,
    CannotUpdateItemTypeWithoutAvailabilities = 59,
    CannotModifyItemTypeWithoutAvailabilities = 60,
    NumberOfServingsMustBeAtLeastOne = 61,
}