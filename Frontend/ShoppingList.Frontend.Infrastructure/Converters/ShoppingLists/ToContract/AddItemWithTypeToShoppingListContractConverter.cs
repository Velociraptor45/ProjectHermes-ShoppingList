﻿using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.AddItemWithTypeToShoppingList;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.ShoppingLists;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ShoppingLists.ToContract
{
    public class AddItemWithTypeToShoppingListContractConverter :
        IToContractConverter<AddItemWithTypeToShoppingListRequest, AddItemWithTypeToShoppingListContract>
    {
        public AddItemWithTypeToShoppingListContract ToContract(AddItemWithTypeToShoppingListRequest source)
        {
            return new AddItemWithTypeToShoppingListContract(
                source.SectionId,
                source.Quantity);
        }
    }
}