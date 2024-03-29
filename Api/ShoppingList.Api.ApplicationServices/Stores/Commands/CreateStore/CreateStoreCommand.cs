﻿using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Creations;

namespace ProjectHermes.ShoppingList.Api.ApplicationServices.Stores.Commands.CreateStore;

public class CreateStoreCommand : ICommand<IStore>
{
    public CreateStoreCommand(StoreCreation storeCreation)
    {
        StoreCreation = storeCreation;
    }

    public StoreCreation StoreCreation { get; }
}