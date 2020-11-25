﻿using ShoppingList.Api.Domain.Exceptions;
using ShoppingList.Api.Domain.Extensions;
using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Ports;
using ShoppingList.Api.Domain.Ports.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.Commands.UpdateItem
{
    public class UpdateItemCommandHandler : ICommandHandler<UpdateItemCommand, bool>
    {
        private readonly IItemRepository itemRepository;
        private readonly IItemCategoryRepository itemCategoryRepository;
        private readonly IManufacturerRepository manufacturerRepository;
        private readonly IShoppingListRepository shoppingListRepository;
        private readonly ITransactionGenerator transactionGenerator;

        public UpdateItemCommandHandler(IItemRepository itemRepository, IItemCategoryRepository itemCategoryRepository,
            IManufacturerRepository manufacturerRepository, IShoppingListRepository shoppingListRepository,
            ITransactionGenerator transactionGenerator)
        {
            this.itemRepository = itemRepository;
            this.itemCategoryRepository = itemCategoryRepository;
            this.manufacturerRepository = manufacturerRepository;
            this.shoppingListRepository = shoppingListRepository;
            this.transactionGenerator = transactionGenerator;
        }

        public async Task<bool> HandleAsync(UpdateItemCommand command, CancellationToken cancellationToken)
        {
            if (command is null)
            {
                throw new System.ArgumentNullException(nameof(command));
            }

            StoreItem oldItem = await itemRepository.FindByAsync(command.ItemUpdate.OldId, cancellationToken);
            if (oldItem.IsTemporary)
                throw new TemporaryItemNotUpdateableException(command.ItemUpdate.OldId);

            oldItem.Delete();

            ItemCategory itemCategory = await itemCategoryRepository
                .FindByAsync(command.ItemUpdate.ItemCategoryId, cancellationToken);
            Manufacturer manufacturer = await manufacturerRepository
                .FindByAsync(command.ItemUpdate.ManufacturerId, cancellationToken);

            using ITransaction transaction = await transactionGenerator.GenerateAsync(cancellationToken);
            await itemRepository.StoreAsync(oldItem, cancellationToken);

            // create new Item
            StoreItem updatedItem = command.ItemUpdate.ToStoreItem(itemCategory, manufacturer);
            updatedItem = await itemRepository.StoreAsync(updatedItem, cancellationToken);

            // change existing item references on shopping lists
            var shoppingListsWithOldItem = (await shoppingListRepository
                .FindActiveByAsync(command.ItemUpdate.OldId, cancellationToken))
                .ToList();

            foreach (var list in shoppingListsWithOldItem)
            {
                ShoppingListItem shoppingListItem = list.Items
                    .First(i => i.Id == command.ItemUpdate.OldId.ToShoppingListItemId());
                list.RemoveItem(command.ItemUpdate.OldId.ToShoppingListItemId());
                if (updatedItem.IsAvailableInStore(list.Store.Id))
                {
                    list.AddItem(updatedItem, shoppingListItem.IsInBasket, shoppingListItem.Quantity);
                }

                await shoppingListRepository.StoreAsync(list, cancellationToken);
            }

            await transaction.CommitAsync(cancellationToken);

            return true;
        }
    }
}