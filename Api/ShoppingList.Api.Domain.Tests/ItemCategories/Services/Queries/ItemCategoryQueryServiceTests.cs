﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Queries;
using ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Models;
using ShoppingList.Api.Domain.TestKit.ItemCategories.Ports;
using ShoppingList.Api.TestTools.Exceptions;

using DomainModels = ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ItemCategories.Services.Queries;

public class ItemCategoryQueryServiceTests
{
    public class GetAsyncItemCategoryId
    {
        private readonly GetAsyncFixture _fixture;

        public GetAsyncItemCategoryId()
        {
            _fixture = new GetAsyncFixture();
        }

        [Fact]
        public async Task GetAsync_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupItemCategoryId();
            _fixture.SetupExpectedResult();
            _fixture.SetupFindingItemCategory();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemCategoryId);
            TestPropertyNotSetException.ThrowIfNull(_fixture.ExpectedResult);

            // Act
            var result = await sut.GetAsync(_fixture.ItemCategoryId.Value);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedResult);
        }

        [Fact]
        public async Task GetAsync_WithInvalidItemCategoryId_ShouldThrowDomainException()
        {
            // Arrange
            _fixture.SetupItemCategoryId();
            _fixture.SetupNotFindingItemCategory();
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ItemCategoryId);

            // Act
            var func = async () => await sut.GetAsync(_fixture.ItemCategoryId.Value);

            // Assert
            await func.Should().ThrowDomainExceptionAsync(ErrorReasonCode.ItemCategoryNotFound);
        }

        private class GetAsyncFixture : ItemCategoryQueryServiceFixture
        {
            public ItemCategoryId? ItemCategoryId { get; private set; }
            public ItemCategory? ExpectedResult { get; private set; }

            public void SetupItemCategoryId()
            {
                ItemCategoryId = DomainModels.ItemCategoryId.New;
            }

            public void SetupExpectedResult()
            {
                ExpectedResult = new ItemCategoryBuilder().Create();
            }

            public void SetupFindingItemCategory()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemCategoryId);
                TestPropertyNotSetException.ThrowIfNull(ExpectedResult);

                SetupFindingItemCategory(ItemCategoryId.Value, ExpectedResult);
            }

            public void SetupNotFindingItemCategory()
            {
                TestPropertyNotSetException.ThrowIfNull(ItemCategoryId);

                SetupNotFindingItemCategory(ItemCategoryId.Value);
            }
        }
    }

    private class ItemCategoryQueryServiceFixture
    {
        private readonly ItemCategoryRepositoryMock _itemCategoryRepositoryMock = new(MockBehavior.Strict);

        public ItemCategoryQueryService CreateSut()
        {
            return new ItemCategoryQueryService(_itemCategoryRepositoryMock.Object, default);
        }

        public void SetupFindingItemCategory(ItemCategoryId itemCategoryId, IItemCategory itemCategory)
        {
            _itemCategoryRepositoryMock.SetupFindByAsync(itemCategoryId, itemCategory);
        }

        public void SetupNotFindingItemCategory(ItemCategoryId itemCategoryId)
        {
            _itemCategoryRepositoryMock.SetupFindByAsync(itemCategoryId, null);
        }
    }
}