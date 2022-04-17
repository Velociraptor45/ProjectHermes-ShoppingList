﻿using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.RemoveItemFromBasket;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.Shared;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.Common;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.ShoppingLists;
using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Converters.ShoppingLists.ToContract
{
    public class RemoveItemFromBasketContractConverter :
        IToContractConverter<RemoveItemFromBasketRequest, RemoveItemFromBasketContract>
    {
        private readonly IToContractConverter<ShoppingListItemId, ItemIdContract> _itemIdConverter;

        public RemoveItemFromBasketContractConverter(
            IToContractConverter<ShoppingListItemId, ItemIdContract> itemIdConverter)
        {
            _itemIdConverter = itemIdConverter;
        }

        public RemoveItemFromBasketContract ToContract(RemoveItemFromBasketRequest source)
        {
            return new RemoveItemFromBasketContract(
                _itemIdConverter.ToContract(source.ItemId),
                source.ItemTypeId);
        }
    }
}