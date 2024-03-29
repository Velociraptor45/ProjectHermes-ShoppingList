﻿using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Modifications;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

public interface IItemCategory
{
    ItemCategoryId Id { get; }
    ItemCategoryName Name { get; }
    bool IsDeleted { get; }

    void Delete();

    void Modify(ItemCategoryModification modification);
}