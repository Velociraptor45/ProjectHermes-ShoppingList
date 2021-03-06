﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Frontend.Models.Index
{
    public class AvailableStores
    {
        public AvailableStores(IEnumerable<Store> stores, int selectedStoreId)
        {
            Stores = stores?.ToList() ?? throw new ArgumentNullException(nameof(stores));
            SelectedStoreId = selectedStoreId;
        }

        public List<Store> Stores { get; }
        public int SelectedStoreId { get; set; }
    }
}