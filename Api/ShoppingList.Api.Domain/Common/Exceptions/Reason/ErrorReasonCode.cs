﻿namespace ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason
{
    public enum ErrorReasonCode
    {
        ItemNotFound = 0,
        InvalidItemQuantity = 2,
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
        StoreItemSectionNotFound = 18,
        NoDefaultSectionSpecified = 19,
        SectionAlreadyInShoppingList = 20,
        MultipleAvailabilitiesForStore = 21,
        CannotModifyItemAsItemWithTypes = 22,
        CannotAddTypedItemToShoppingListWithoutTypeIdReason = 23,
        CannotRemoveAllTypesFromItemWithTypes = 24,
        CannotCreateItemWithTypesWithoutTypes = 25,
        ItemTypeNotFound = 26,
        ShoppingListItemMissingType = 27,
        ItemTypeNotPartOfItem = 28,
        ItemTypeAtStoreNotAvailable = 29,
        TemporaryItemCannotHaveTypeIdReason = 30
    }
}