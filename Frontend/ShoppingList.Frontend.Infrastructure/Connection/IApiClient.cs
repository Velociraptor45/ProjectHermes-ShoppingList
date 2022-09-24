﻿using ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.ItemCategories;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.Items;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.Manufacturers;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.ShoppingLists;
using ProjectHermes.ShoppingList.Frontend.Infrastructure.Requests.Stores;
using ProjectHermes.ShoppingList.Frontend.Models.ItemCategories.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Items.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Manufacturers.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Recipes.Models;
using ProjectHermes.ShoppingList.Frontend.Models.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Frontend.Models.Stores.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Frontend.Infrastructure.Connection
{
    public interface IApiClient
    {
        Task AddItemToShoppingListAsync(AddItemToShoppingListRequest request);

        Task ModifyItemAsync(ModifyItemRequest request);

        Task ChangeItemQuantityOnShoppingListAsync(ChangeItemQuantityOnShoppingListRequest request);

        Task CreateItemAsync(CreateItemRequest request);

        Task<ItemCategory> CreateItemCategoryAsync(string name);

        Task<Manufacturer> CreateManufacturerAsync(string name);

        Task CreateTemporaryItem(CreateTemporaryItemRequest request);

        Task DeleteItemAsync(DeleteItemRequest request);

        Task FinishListAsync(FinishListRequest request);

        Task<ShoppingListRoot> GetActiveShoppingListByStoreIdAsync(Guid storeId);

        Task<IEnumerable<ItemCategory>> GetAllActiveItemCategoriesAsync();

        Task<IEnumerable<Manufacturer>> GetAllActiveManufacturersAsync();

        Task<IEnumerable<Store>> GetAllActiveStoresAsync();

        Task<IEnumerable<QuantityTypeInPacket>> GetAllQuantityTypesInPacketAsync();

        Task<IEnumerable<QuantityType>> GetAllQuantityTypesAsync();

        Task<Item> GetItemByIdAsync(Guid itemId);

        Task<IEnumerable<SearchItemResult>> SearchItemsAsync(string searchInput, Guid? itemCategoryId);

        Task<IEnumerable<SearchItemResult>> SearchItemsByFilterAsync(IEnumerable<Guid> storeIds,
            IEnumerable<Guid> itemCategoryIds, IEnumerable<Guid> manufacturerIds);

        Task<IEnumerable<SearchItemForShoppingListResult>> SearchItemsForShoppingListAsync(string searchInput, Guid storeId);

        Task IsAliveAsync();

        Task MakeTemporaryItemPermanent(MakeTemporaryItemPermanentRequest request);

        Task PutItemInBasketAsync(PutItemInBasketRequest request);

        Task RemoveItemFromBasketAsync(RemoveItemFromBasketRequest request);

        Task RemoveItemFromShoppingListAsync(RemoveItemFromShoppingListRequest request);

        Task UpdateItemAsync(UpdateItemRequest request);

        Task CreateStoreAsync(CreateStoreRequest request);

        Task ModifyStoreAsync(ModifyStoreRequest request);

        Task ModifyItemWithTypesAsync(ModifyItemWithTypesRequest request);

        Task AddItemWithTypeToShoppingListAsync(AddItemWithTypeToShoppingListRequest request);

        Task UpdateItemWithTypesAsync(UpdateItemWithTypesRequest request);

        Task CreateItemWithTypesAsync(CreateItemWithTypesRequest request);

        Task<IEnumerable<ManufacturerSearchResult>> GetManufacturerSearchResultsAsync(string searchInput);

        Task<Manufacturer> GetManufacturerByIdAsync(Guid id);

        Task DeleteManufacturerAsync(Guid id);

        Task ModifyManufacturerAsync(ModifyManufacturerRequest request);

        Task<ItemCategory> GetItemCategoryByIdAsync(Guid id);

        Task<IEnumerable<ItemCategorySearchResult>> GetItemCategoriesSearchResultsAsync(string searchInput);

        Task DeleteItemCategoryAsync(Guid id);

        Task ModifyItemCategoryAsync(ModifyItemCategoryRequest request);

        Task UpdateItemPriceAsync(UpdateItemPriceRequest request);

        Task<Recipe> GetRecipeByIdAsync(Guid recipeId);

        Task<IEnumerable<RecipeSearchResult>> SearchRecipesByNameAsync(string searchInput);

        Task<Recipe> CreateRecipeAsync(Recipe recipe);

        Task ModifyRecipeAsync(Recipe recipe);
        Task<IEnumerable<SearchItemByItemCategoryResult>> SearchItemByItemCategoryAsync(Guid itemCategoryId);
        Task<IEnumerable<IngredientQuantityType>> GetAllIngredientQuantityTypes();
    }
}