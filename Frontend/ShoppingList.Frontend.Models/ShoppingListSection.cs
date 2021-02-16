﻿using ProjectHermes.ShoppingList.Frontend.Models.Shared;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models
{
    public class ShoppingListSection
    {
        private readonly Dictionary<ItemId, ShoppingListItem> items;

        public ShoppingListSection(int id, string name, int sortingIndex, bool isDefaultSection,
            IEnumerable<ShoppingListItem> items)
        {
            this.items = items.ToDictionary(i => i.Id);
            Id = id;
            Name = name;
            SortingIndex = sortingIndex;
            IsDefaultSection = isDefaultSection;
        }

        public int Id { get; }
        public string Name { get; }
        public int SortingIndex { get; }
        public bool IsDefaultSection { get; }
        public IReadOnlyCollection<ShoppingListItem> Items => items.Values.ToList().AsReadOnly();

        public void RemoveItem(ItemId itemId)
        {
            if (items.ContainsKey(itemId))
            {
                items.Remove(itemId);
            }
        }

        public void AddItem(ShoppingListItem item)
        {
            if (items.ContainsKey(item.Id))
                return;

            items.Add(item.Id, item);
        }
    }
}