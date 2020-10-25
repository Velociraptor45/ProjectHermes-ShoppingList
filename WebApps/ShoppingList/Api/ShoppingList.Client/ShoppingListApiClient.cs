﻿using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestEase;
using ShoppingList.Contracts.Commands.CreateItem;
using ShoppingList.Contracts.Commands.UpdateItem;
using ShoppingList.Contracts.Queries;
using ShoppingList.Contracts.SharedContracts;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShoppingList.Client
{
    public class ShoppingListApiClient : IShoppingListApiClient
    {
        private readonly IShoppingListApiClient apiClient;

        public ShoppingListApiClient(HttpClient httpClient)
        {
            apiClient = new RestClient(httpClient)
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

        public async Task<ShoppingListContract> GetActiveShoppingListByStoreId(int storeId)
        {
            return await apiClient.GetActiveShoppingListByStoreId(storeId);
        }

        public async Task RemoveItemFromShoppingList(int shoppingListId, int itemId)
        {
            await apiClient.RemoveItemFromShoppingList(shoppingListId, itemId);
        }

        public async Task AddItemToShoppingList(int shoppingListId, int itemId, float quantity)
        {
            await apiClient.AddItemToShoppingList(shoppingListId, itemId, quantity);
        }

        public async Task PutItemInBasket(int shoppingListId, int itemId)
        {
            await apiClient.PutItemInBasket(shoppingListId, itemId);
        }

        public async Task RemoveItemFromBasket(int shoppingListId, int itemId)
        {
            await apiClient.RemoveItemFromBasket(shoppingListId, itemId);
        }

        public async Task ChangeItemQuantityOnShoppingList(int shoppingListId, int itemId, float quantity)
        {
            await apiClient.ChangeItemQuantityOnShoppingList(shoppingListId, itemId, quantity);
        }

        public async Task CreatList(int storeId)
        {
            await apiClient.CreatList(storeId);
        }

        public async Task FinishList(int shoppingListId)
        {
            await apiClient.FinishList(shoppingListId);
        }

        #endregion ShoppingListController

        #region ItemController

        public async Task CreateItem(CreateItemContract createItemContract)
        {
            await apiClient.CreateItem(createItemContract);
        }

        public async Task UpdateItem(UpdateItemContract updateItemContract)
        {
            await apiClient.UpdateItem(updateItemContract);
        }

        public async Task<IEnumerable<ItemSearchContract>> GetItemSearchResults(string searchInput, int storeId)
        {
            return await apiClient.GetItemSearchResults(searchInput, storeId);
        }

        #endregion ItemController
    }
}