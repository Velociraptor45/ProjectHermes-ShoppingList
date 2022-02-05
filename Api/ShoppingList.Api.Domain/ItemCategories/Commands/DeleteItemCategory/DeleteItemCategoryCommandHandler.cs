﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports.Infrastructure;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Commands.DeleteItemCategory;

public class DeleteItemCategoryCommandHandler : ICommandHandler<DeleteItemCategoryCommand, bool>
{
    private readonly IItemCategoryRepository _itemCategoryRepository;
    private readonly IItemRepository _itemRepository;
    private readonly IShoppingListRepository _shoppingListRepository;
    private readonly ITransactionGenerator _transactionGenerator;

    public DeleteItemCategoryCommandHandler(IItemCategoryRepository itemCategoryRepository,
        IItemRepository itemRepository, IShoppingListRepository shoppingListRepository,
        ITransactionGenerator transactionGenerator)
    {
        _itemCategoryRepository = itemCategoryRepository;
        _itemRepository = itemRepository;
        _shoppingListRepository = shoppingListRepository;
        _transactionGenerator = transactionGenerator;
    }

    public async Task<bool> HandleAsync(DeleteItemCategoryCommand command, CancellationToken cancellationToken)
    {
        if (command is null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        var category = await _itemCategoryRepository.FindByAsync(command.ItemCategoryId, cancellationToken);
        if (category == null)
            throw new DomainException(new ItemCategoryNotFoundReason(command.ItemCategoryId));

        category.Delete();

        var storeItems = (await _itemRepository.FindActiveByAsync(command.ItemCategoryId, cancellationToken))
            .ToList();

        using var transaction = await _transactionGenerator.GenerateAsync(cancellationToken);
        foreach (var item in storeItems)
        {
            var lists = await _shoppingListRepository.FindActiveByAsync(item.Id, cancellationToken);
            foreach (var list in lists)
            {
                if (item.HasItemTypes)
                {
                    foreach (var type in item.ItemTypes)
                    {
                        list.RemoveItem(item.Id, type.Id);
                    }
                }
                else
                {
                    list.RemoveItem(item.Id);
                }

                await _shoppingListRepository.StoreAsync(list, cancellationToken);
            }
            item.Delete();
            await _itemRepository.StoreAsync(item, cancellationToken);
        }
        await _itemCategoryRepository.StoreAsync(category, cancellationToken);

        await transaction.CommitAsync(cancellationToken);
        return true;
    }
}