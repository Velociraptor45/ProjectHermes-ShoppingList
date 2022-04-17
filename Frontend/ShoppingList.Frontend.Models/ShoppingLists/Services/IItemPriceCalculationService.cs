﻿using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Services
{
    public interface IItemPriceCalculationService
    {
        float CalculatePrice(int quantityTypeId, float pricePerQuantity, float quantity);
        Task InitializeAsync();
    }
}