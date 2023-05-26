﻿using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;

namespace ProjectHermes.ShoppingList.Api.Domain.Stores.Services.Creations;

public class StoreCreationService : IStoreCreationService
{
    private readonly IStoreRepository _storeRepository;
    private readonly IStoreFactory _storeFactory;
    private readonly IShoppingListFactory _shoppingListFactory;
    private readonly IShoppingListRepository _shoppingListRepository;
    private readonly CancellationToken _cancellationToken;

    public StoreCreationService(
        Func<CancellationToken, IStoreRepository> storeRepositoryDelegate,
        IStoreFactory storeFactory,
        IShoppingListFactory shoppingListFactory,
        Func<CancellationToken, IShoppingListRepository> shoppingListRepositoryDelegate,
        CancellationToken cancellationToken)
    {
        _storeRepository = storeRepositoryDelegate(cancellationToken);
        _storeFactory = storeFactory;
        _shoppingListFactory = shoppingListFactory;
        _shoppingListRepository = shoppingListRepositoryDelegate(cancellationToken);
        _cancellationToken = cancellationToken;
    }

    public async Task<IStore> CreateAsync(StoreCreation creation)
    {
        _cancellationToken.ThrowIfCancellationRequested();

        IStore store = _storeFactory.CreateNew(creation);

        store = await _storeRepository.StoreAsync(store);

        _cancellationToken.ThrowIfCancellationRequested();

        var shoppingList = _shoppingListFactory.CreateNew(store);
        await _shoppingListRepository.StoreAsync(shoppingList);

        return store;
    }
}