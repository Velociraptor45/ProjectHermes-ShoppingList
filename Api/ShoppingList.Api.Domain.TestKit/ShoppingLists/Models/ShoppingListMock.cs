﻿using Moq;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ShoppingList.Api.Domain.TestKit.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingList.Api.Domain.TestKit.ShoppingLists.Models
{
    public class ShoppingListMock : Mock<IShoppingList>
    {
        public ShoppingListMock(IShoppingList shoppingList)
        {
            SetupId(shoppingList.Id);
            SetupStoreId(shoppingList.StoreId);
            SetupItems(shoppingList.Items);
            SetupSections(shoppingList.Sections);
        }

        public IShoppingListItem GetRandomItem(CommonFixture commonFixture)
        {
            return commonFixture.ChooseRandom(Object.Items);
        }

        public IShoppingListItem GetRandomItem(CommonFixture commonFixture, Func<IShoppingListItem, bool> condition)
        {
            var filteredItems = Object.Items.Where(condition);

            return commonFixture.ChooseRandom(filteredItems);
        }

        #region Setup properties

        public void SetupId(ShoppingListId returnValue)
        {
            Setup(i => i.Id)
                .Returns(returnValue);
        }

        public void SetupStoreId(StoreId returnValue)
        {
            Setup(i => i.StoreId)
                .Returns(returnValue);
        }

        public void SetupItems(IReadOnlyCollection<IShoppingListItem> items)
        {
            Setup(i => i.Items)
                .Returns(items);
        }

        public void SetupSections(IEnumerable<IShoppingListSection> sections)
        {
            var readonlySections = sections.ToList().AsReadOnly();

            Setup(i => i.Sections)
                .Returns(readonlySections);
        }

        #endregion Setup properties

        #region Setup methods

        public void SetupFinish(DateTime completionDate, IShoppingList returnValue)
        {
            Setup(i => i.Finish(completionDate))
                .Returns(returnValue);
        }

        #endregion Setup methods

        #region Verify methods

        public void VerifyRemoveItemOnce(ItemId itemId)
        {
            Verify(i => i.RemoveItem(itemId),
                Times.Once);
        }

        public void VerifyPutItemInBasketOnce(ItemId itemId)
        {
            Verify(i => i.PutItemInBasket(itemId), Times.Once);
        }

        public void VerifyPutItemInBasketNever()
        {
            Verify(i => i.PutItemInBasket(It.IsAny<ItemId>()), Times.Never);
        }

        public void VerifyRemoveItemFromBasketOnce(ItemId itemId)
        {
            Verify(i => i.RemoveFromBasket(itemId), Times.Once);
        }

        public void VerifyAddItemOnce(IShoppingListItem listItem, SectionId sectionId)
        {
            Verify(i => i.AddItem(listItem, sectionId),
                Times.Once);
        }

        public void VerifyAddItemNever()
        {
            Verify(i => i.AddItem(
                    It.IsAny<IShoppingListItem>(),
                    It.IsAny<SectionId>()),
                Times.Never);
        }

        public void VerifyAddSectionOnce(IShoppingListSection section)
        {
            Verify(i => i.AddSection(section),
                Times.Once);
        }

        public void VerifyAddSectionNever()
        {
            Verify(i => i.AddSection(
                    It.IsAny<IShoppingListSection>()),
                Times.Never);
        }

        public void VerifyFinishOnce(DateTime completionDate)
        {
            Verify(i => i.Finish(completionDate),
                Times.Once);
        }

        public void VerifyChangeItemQuantityOnce(ItemId itemId, float quantity)
        {
            Verify(i => i.ChangeItemQuantity(itemId, quantity),
                Times.Once);
        }

        #endregion Verify methods
    }
}