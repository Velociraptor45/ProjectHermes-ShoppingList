﻿using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.Shared;
using ProjectHermes.ShoppingList.Frontend.Models.Shared;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Extensions.Models
{
    public static class ItemIdExtensions
    {
        public static ItemIdContract ToContract(this ItemId model)
        {
            return model.ActualId.HasValue ?
                new ItemIdContract() { Actual = model.ActualId } :
                new ItemIdContract() { Offline = model.OfflineId };
        }
    }
}