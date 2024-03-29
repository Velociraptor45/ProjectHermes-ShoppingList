﻿using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Commands.DeleteItem;

public class DeleteItemCommand : ICommand<bool>
{
    public DeleteItemCommand(ItemId itemId)
    {
        ItemId = itemId;
    }

    public ItemId ItemId { get; }
}