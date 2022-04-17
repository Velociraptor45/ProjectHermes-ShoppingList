﻿using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.ChangeItemQuantityOnShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.Shared;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.ShoppingLists;
using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ShoppingLists.ToContract
{
    public class ChangeItemQuantityOnShoppingListContractContract :
        IToContractConverter<ChangeItemQuantityOnShoppingListRequest, ChangeItemQuantityOnShoppingListContract>
    {
        private readonly IToContractConverter<ItemId, ItemIdContract> _itemIdConverter;

        public ChangeItemQuantityOnShoppingListContractContract(
            IToContractConverter<ItemId, ItemIdContract> itemIdConverter)
        {
            _itemIdConverter = itemIdConverter;
        }

        public ChangeItemQuantityOnShoppingListContract ToContract(ChangeItemQuantityOnShoppingListRequest source)
        {
            return new ChangeItemQuantityOnShoppingListContract(
                _itemIdConverter.ToContract(source.ItemId),
                source.ItemTypeId,
                source.Quantity);
        }
    }
}