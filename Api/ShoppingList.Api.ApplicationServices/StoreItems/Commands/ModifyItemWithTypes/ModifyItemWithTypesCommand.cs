﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services.Modifications;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.StoreItems.Commands.ModifyItemWithTypes;

public class ModifyItemWithTypesCommand : ICommand<bool>
{
    public ModifyItemWithTypesCommand(ItemWithTypesModification itemWithTypesModification)
    {
        ItemWithTypesModification = itemWithTypesModification ??
                                    throw new ArgumentNullException(nameof(itemWithTypesModification));
    }

    public ItemWithTypesModification ItemWithTypesModification { get; }
}