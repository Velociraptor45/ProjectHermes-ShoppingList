﻿using Moq.Contrib.InOrder;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.Effects;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;
using RestEase;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.ItemCategories.Effects;

public class ItemCategoryEffectsTests
{
    public class HandleSearchItemCategoriesAction
    {
        private readonly HandleSearchItemCategoriesActionFixture _fixture = new();

        [Fact]
        public async Task HandleSearchItemCategoriesAction_WithSearchInputEmpty_ShouldDispatchFinishedActionWithEmptyResult()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchInputEmpty();
                _fixture.SetupSearchResultEmpty();
                _fixture.SetupAction();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleSearchItemCategoriesAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchItemCategoriesAction_WithSearchInput_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupSearchResult();
                _fixture.SetupAction();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupSearchSucceeded();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleSearchItemCategoriesAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchItemCategoriesAction_WithWithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupAction();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupSearchFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
            });
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleSearchItemCategoriesAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchItemCategoriesAction_WithWithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupAction();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupSearchFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
            });
            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleSearchItemCategoriesAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleSearchItemCategoriesActionFixture : ItemCategoryEffectsFixture
        {
            private string? _searchInput;
            private IReadOnlyCollection<ItemCategorySearchResult>? _searchResult;
            public SearchItemCategoriesAction? Action { get; private set; }

            public void SetupSearchInput()
            {
                _searchInput = new DomainTestBuilder<string>().Create();
            }

            public void SetupSearchInputEmpty()
            {
                _searchInput = string.Empty;
            }

            public void SetupSearchResult()
            {
                _searchResult = new DomainTestBuilder<ItemCategorySearchResult>().CreateMany(2).ToList();
            }

            public void SetupSearchResultEmpty()
            {
                _searchResult = new List<ItemCategorySearchResult>();
            }

            public void SetupSearchSucceeded()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);
                TestPropertyNotSetException.ThrowIfNull(_searchResult);

                ApiClientMock.SetupGetItemCategorySearchResultsAsync(_searchInput, _searchResult);
            }

            public void SetupSearchFailedWithErrorInApi()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);

                ApiClientMock.SetupGetItemCategorySearchResultsAsyncThrowing(_searchInput,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupSearchFailedWithErrorWhileTransmittingRequest()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);

                ApiClientMock.SetupGetItemCategorySearchResultsAsyncThrowing(_searchInput,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);

                Action = new SearchItemCategoriesAction(_searchInput);
            }

            public void SetupDispatchingFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchResult);

                SetupDispatchingAction(new SearchItemCategoriesFinishedAction(_searchResult));
            }

            public void SetupDispatchingStartedAction()
            {
                SetupDispatchingAction<SearchItemCategoriesStartedAction>();
            }
        }
    }

    private abstract class ItemCategoryEffectsFixture : ItemCategoryEffectsFixtureBase
    {
        public ItemCategoryEffects CreateSut()
        {
            SetupStateReturningState();
            return new ItemCategoryEffects(ApiClientMock.Object, NavigationManagerMock.Object);
        }
    }
}