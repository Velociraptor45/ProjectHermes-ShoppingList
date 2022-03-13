﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.Shared;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Services.ShoppingListModifications
{
    public class ShoppingListModificationService : IShoppingListModificationService
    {
        private readonly IShoppingListRepository _shoppingListRepository;
        private readonly IItemRepository _itemRepository;
        private readonly CancellationToken _cancellationToken;

        public ShoppingListModificationService(
            IShoppingListRepository shoppingListRepository,
            IItemRepository itemRepository,
            CancellationToken cancellationToken)
        {
            _shoppingListRepository = shoppingListRepository;
            _itemRepository = itemRepository;
            _cancellationToken = cancellationToken;
        }

        public async Task ChangeItemQuantityAsync(ShoppingListId shoppingListId,
            OfflineTolerantItemId offlineTolerantItemId, ItemTypeId? itemTypeId, float quantity)
        {
            ArgumentNullException.ThrowIfNull(offlineTolerantItemId);

            var list = await _shoppingListRepository.FindByAsync(shoppingListId, _cancellationToken);
            if (list == null)
                throw new DomainException(new ShoppingListNotFoundReason(shoppingListId));

            ItemId itemId;
            if (offlineTolerantItemId.IsActualId)
            {
                itemId = new ItemId(offlineTolerantItemId.ActualId!.Value);
            }
            else
            {
                if (itemTypeId != null)
                    throw new DomainException(new TemporaryItemCannotHaveTypeIdReason());

                var temporaryId = new TemporaryItemId(offlineTolerantItemId.OfflineId!.Value);
                var item = await _itemRepository.FindByAsync(temporaryId, _cancellationToken);

                if (item == null)
                    throw new DomainException(new ItemNotFoundReason(temporaryId));

                itemId = item.Id;
            }

            list.ChangeItemQuantity(itemId, itemTypeId, quantity);

            _cancellationToken.ThrowIfCancellationRequested();

            await _shoppingListRepository.StoreAsync(list, _cancellationToken);
        }
    }
}