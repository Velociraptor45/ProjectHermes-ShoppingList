﻿using ProjectHermes.ShoppingList.Frontend.Models.ItemCategories.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Manufacturers.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Stores.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.WebApp.Pages.Items.Models
{
    public class ItemsState
    {
        private readonly List<Store> _stores;
        private List<ItemCategory> _itemCategories;
        private List<Manufacturer> _manufacturers;
        private List<SearchItemResult> _items = new();
        private readonly List<QuantityType> _quantityTypes;
        private readonly List<QuantityTypeInPacket> _quantityTypesInPacket;

        public ItemsState(IEnumerable<Store> stores, IEnumerable<ItemCategory> itemCategories, IEnumerable<Manufacturer> manufacturers,
            IEnumerable<QuantityType> quantityTypes, IEnumerable<QuantityTypeInPacket> quantityTypesInPacket)
        {
            _stores = stores.ToList();
            _itemCategories = itemCategories.ToList();
            _manufacturers = manufacturers.ToList();
            _quantityTypes = quantityTypes.ToList();
            _quantityTypesInPacket = quantityTypesInPacket.ToList();
        }

        public IReadOnlyCollection<Store> Stores => _stores.AsReadOnly();
        public IReadOnlyCollection<ItemCategory> ItemCategories => _itemCategories.AsReadOnly();
        public IReadOnlyCollection<Manufacturer> Manufacturers => _manufacturers.AsReadOnly();
        public IReadOnlyCollection<SearchItemResult> Items => _items.AsReadOnly();
        public IReadOnlyCollection<QuantityType> QuantityTypes => _quantityTypes.AsReadOnly();
        public IReadOnlyCollection<QuantityTypeInPacket> QuantityTypesInPacket => _quantityTypesInPacket.AsReadOnly();

        public Func<Task> ManufacturerCreated { get; set; }
        public Func<Task> ItemCategoryCreated { get; set; }

        public Action StateChanged { get; set; }
        public Item EditedItem { get; private set; }
        public bool IsInEditMode => EditedItem != null;

        public void UpdateManufacturers(IEnumerable<Manufacturer> manufacturers)
        {
            _manufacturers = manufacturers.ToList();
            StateChanged?.Invoke();
        }

        public void UpdateItemCategories(IEnumerable<ItemCategory> itemCategories)
        {
            _itemCategories = itemCategories.ToList();
            StateChanged?.Invoke();
        }

        public void UpdateItems(IEnumerable<SearchItemResult> items)
        {
            _items = items.ToList();
            StateChanged?.Invoke();
        }

        public Store GetStore(Guid id)
        {
            return Stores.FirstOrDefault(s => s.Id == id);
        }

        public void EnterEditorForNewItem()
        {
            // todo: ugly
            var item =
                new Item(Guid.Empty, "", false, "", false,
                    new QuantityType(0, "", 0, "", "", 0), 0,
                    new QuantityTypeInPacket(0, "", ""), null, null,
                    new List<ItemAvailability>(),
                    new List<ItemType>());

            EnterEditor(item);
        }

        public void EnterEditor(Item item)
        {
            EditedItem = item;
            StateChanged?.Invoke();
        }

        public void LeaveEditor()
        {
            EditedItem = null;
            StateChanged?.Invoke();
        }
    }
}