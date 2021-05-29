﻿using ProjectHermes.ShoppingList.Api.Core;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Models
{
    public class SectionId : GenericPrimitive<int>
    {
        public SectionId(int value) : base(value)
        {
        }
    }
}