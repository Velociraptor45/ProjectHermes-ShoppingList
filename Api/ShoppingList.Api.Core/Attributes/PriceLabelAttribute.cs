﻿using System;

namespace ProjectHermes.ShoppingList.Api.Core.Attributes
{
    public class PriceLabelAttribute : Attribute
    {
        public PriceLabelAttribute(string priceLabel)
        {
            PriceLabel = priceLabel;
        }

        public string PriceLabel { get; }
    }
}