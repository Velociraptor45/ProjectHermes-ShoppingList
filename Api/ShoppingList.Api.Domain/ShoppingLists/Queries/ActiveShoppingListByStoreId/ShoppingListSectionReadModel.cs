﻿using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Queries.ActiveShoppingListByStoreId
{
    public class ShoppingListSectionReadModel
    {
        private readonly IEnumerable<ShoppingListItemReadModel> itemReadModels;

        public ShoppingListSectionReadModel(SectionId id, string name, int sortingIndex,
            bool isDefaultSection, IEnumerable<ShoppingListItemReadModel> itemReadModels)
        {
            Id = id;
            Name = name;
            SortingIndex = sortingIndex;
            IsDefaultSection = isDefaultSection;
            this.itemReadModels = itemReadModels;
        }

        public IReadOnlyCollection<ShoppingListItemReadModel> ItemReadModels => itemReadModels.ToList().AsReadOnly();

        public SectionId Id { get; }
        public string Name { get; }
        public int SortingIndex { get; }
        public bool IsDefaultSection { get; }
    }
}