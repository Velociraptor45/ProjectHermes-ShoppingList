﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.AddItemToShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.AddItemWithTypeToShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.ChangeItemQuantityOnShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.PutItemInBasket;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.RemoveItemFromBasket;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Commands.RemoveItemFromShoppingList;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Contracts.ShoppingList.Queries.GetActiveShoppingListByStoreId;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.CreateStore;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Commands.UpdateStore;
using ProjectHermes.ShoppingList.Api.Contracts.Store.Queries.AllActiveStores;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ChangeItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.ModifyItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItem;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Commands.UpdateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Get;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.SearchItemsForShoppingLists;
using ProjectHermes.ShoppingList.Api.Contracts.StoreItem.Queries.Shared;
using RestEase;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Client
{
    public class ShoppingListApiClient : IShoppingListApiClient
    {
        private readonly IShoppingListApiClient _apiClient;

        public ShoppingListApiClient(HttpClient httpClient)
        {
            _apiClient = new RestClient(httpClient)
            {
                JsonSerializerSettings =
                new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            }.For<IShoppingListApiClient>();
        }

        #region ShoppingListController

        public async Task<bool> IsAlive()
        {
            return await _apiClient.IsAlive();
        }

        public async Task<ShoppingListContract> GetActiveShoppingListByStoreIdAsync(Guid storeId)
        {
            return await _apiClient.GetActiveShoppingListByStoreIdAsync(storeId);
        }

        public async Task RemoveItemFromShoppingListAsync(Guid shoppingListId,
            RemoveItemFromShoppingListContract contract)
        {
            await _apiClient.RemoveItemFromShoppingListAsync(shoppingListId, contract);
        }

        public async Task AddItemToShoppingListAsync(Guid shoppingListId, AddItemToShoppingListContract contract)
        {
            await _apiClient.AddItemToShoppingListAsync(shoppingListId, contract);
        }

        public async Task AddItemWithTypeToShoppingListAsync(Guid shoppingListId, Guid itemId, Guid itemTypeId,
            AddItemWithTypeToShoppingListContract contract)
        {
            await _apiClient.AddItemWithTypeToShoppingListAsync(shoppingListId, itemId, itemTypeId, contract);
        }

        public async Task PutItemInBasketAsync(Guid shoppingListId, PutItemInBasketContract contract)
        {
            await _apiClient.PutItemInBasketAsync(shoppingListId, contract);
        }

        public async Task RemoveItemFromBasketAsync(Guid shoppingListId, RemoveItemFromBasketContract contract)
        {
            await _apiClient.RemoveItemFromBasketAsync(shoppingListId, contract);
        }

        public async Task ChangeItemQuantityOnShoppingList(ChangeItemQuantityOnShoppingListContract contract)
        {
            await _apiClient.ChangeItemQuantityOnShoppingList(contract);
        }

        public async Task FinishList(Guid shoppingListId)
        {
            await _apiClient.FinishList(shoppingListId);
        }

        public async Task<IEnumerable<QuantityTypeContract>> GetAllQuantityTypes()
        {
            return await _apiClient.GetAllQuantityTypes();
        }

        public async Task<IEnumerable<QuantityTypeInPacketContract>> GetAllQuantityTypesInPacket()
        {
            return await _apiClient.GetAllQuantityTypesInPacket();
        }

        #endregion ShoppingListController

        #region ItemController

        public async Task CreateItemAsync(CreateItemContract contract)
        {
            await _apiClient.CreateItemAsync(contract);
        }

        public async Task CreateItemWithTypesAsync(CreateItemWithTypesContract contract)
        {
            await _apiClient.CreateItemWithTypesAsync(contract);
        }

        public async Task ModifyItemAsync(Guid id, ModifyItemContract contract)
        {
            await _apiClient.ModifyItemAsync(id, contract);
        }

        public async Task ModifyItemWithTypesAsync(Guid id, ModifyItemWithTypesContract contract)
        {
            await _apiClient.ModifyItemWithTypesAsync(id, contract);
        }

        public async Task UpdateItemAsync(Guid id, UpdateItemContract contract)
        {
            await _apiClient.UpdateItemAsync(id, contract);
        }

        public async Task UpdateItemWithTypesAsync(Guid id, UpdateItemWithTypesContract contract)
        {
            await _apiClient.UpdateItemWithTypesAsync(id, contract);
        }

        public async Task DeleteItemAsync(Guid id)
        {
            await _apiClient.DeleteItemAsync(id);
        }

        public async Task<IEnumerable<SearchItemForShoppingListResultContract>> SearchItemsForShoppingListAsync(
            Guid storeId, string searchInput)
        {
            return await _apiClient.SearchItemsForShoppingListAsync(storeId, searchInput);
        }

        public async Task<IEnumerable<SearchItemResultContract>> SearchItemsAsync(string searchInput)
        {
            return await _apiClient.SearchItemsAsync(searchInput);
        }

        public async Task<IEnumerable<SearchItemResultContract>> SearchItemsByFilterAsync(IEnumerable<Guid> storeIds,
            IEnumerable<Guid> itemCategoryIds, IEnumerable<Guid> manufacturerIds)
        {
            return await _apiClient.SearchItemsByFilterAsync(storeIds, itemCategoryIds, manufacturerIds);
        }

        public async Task<StoreItemContract> GetAsync(Guid id)
        {
            return await _apiClient.GetAsync(id);
        }

        public async Task CreateTemporaryItemAsync(CreateTemporaryItemContract contract)
        {
            await _apiClient.CreateTemporaryItemAsync(contract);
        }

        public async Task MakeTemporaryItemPermanentAsync(Guid id, MakeTemporaryItemPermanentContract contract)
        {
            await _apiClient.MakeTemporaryItemPermanentAsync(id, contract);
        }

        #endregion ItemController

        #region StoreController

        public async Task<IEnumerable<ActiveStoreContract>> GetAllActiveStores()
        {
            return await _apiClient.GetAllActiveStores();
        }

        public async Task CreateStore(CreateStoreContract createStoreContract)
        {
            await _apiClient.CreateStore(createStoreContract);
        }

        public async Task UpdateStore(UpdateStoreContract updateStoreContract)
        {
            await _apiClient.UpdateStore(updateStoreContract);
        }

        #endregion StoreController

        #region ManufacturerController

        public async Task<IEnumerable<ManufacturerContract>> GetManufacturerSearchResults(string searchInput)
        {
            return await _apiClient.GetManufacturerSearchResults(searchInput);
        }

        public async Task<IEnumerable<ManufacturerContract>> GetAllActiveManufacturers()
        {
            return await _apiClient.GetAllActiveManufacturers();
        }

        public async Task CreateManufacturer(string name)
        {
            await _apiClient.CreateManufacturer(name);
        }

        #endregion ManufacturerController

        #region ItemCategoryController

        public async Task<IEnumerable<ItemCategoryContract>> SearchItemCategoriesByName(string searchInput)
        {
            return await _apiClient.SearchItemCategoriesByName(searchInput);
        }

        public async Task<IEnumerable<ItemCategoryContract>> GetAllActiveItemCategories()
        {
            return await _apiClient.GetAllActiveItemCategories();
        }

        public async Task<ItemCategoryContract> CreateItemCategory(string name)
        {
            return await _apiClient.CreateItemCategory(name);
        }

        public async Task DeleteItemCategory(Guid id)
        {
            await _apiClient.DeleteItemCategory(id);
        }

        #endregion ItemCategoryController
    }
}