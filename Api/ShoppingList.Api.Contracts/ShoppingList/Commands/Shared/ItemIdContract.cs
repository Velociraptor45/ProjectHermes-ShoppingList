﻿using System;

namespace ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.Shared
{
    public class ItemIdContract
    {
        public int? Actual { get; set; }
        public Guid? Offline { get; set; }
    }
}