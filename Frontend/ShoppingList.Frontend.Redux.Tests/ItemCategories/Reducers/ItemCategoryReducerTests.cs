﻿using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.Reducers;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.ItemCategories.Reducers;

public class ItemCategoryReducerTests
{
    public class OnSearchItemCategoriesStarted
    {
        private readonly OnSearchItemCategoriesStartedFixture _fixture;

        public OnSearchItemCategoriesStarted()
        {
            _fixture = new OnSearchItemCategoriesStartedFixture();
        }

        [Fact]
        public void OnSearchItemCategoriesStarted_WithSearchNotLoading_ShouldSetLoading()
        {
            // Arrange
            _fixture.SetupInitialSearchNotLoading();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemCategoryReducer.OnSearchItemCategoriesStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSearchItemCategoriesStarted_WithSearchLoading_ShouldSetLoading()
        {
            // Arrange
            _fixture.SetupInitialSearchLoading();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemCategoryReducer.OnSearchItemCategoriesStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSearchItemCategoriesStartedFixture : ItemCategoryReducerFixture
        {
            public void SetupInitialSearchLoading()
            {
                SetupInitialState(true);
            }

            public void SetupInitialSearchNotLoading()
            {
                SetupInitialState(false);
            }

            private void SetupInitialState(bool isLoading)
            {
                InitialState = ExpectedState with
                {
                    Search = ExpectedState.Search with
                    {
                        IsLoadingSearchResults = isLoading
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Search = ExpectedState.Search with
                    {
                        IsLoadingSearchResults = true
                    }
                };
            }
        }
    }

    public class OnSearchItemCategoriesFinished
    {
        private readonly OnSearchItemCategoriesFinishedFixture _fixture;

        public OnSearchItemCategoriesFinished()
        {
            _fixture = new OnSearchItemCategoriesFinishedFixture();
        }

        [Fact]
        public void OnSearchItemCategoriesFinished_WithSearchNotLoading_ShouldSetNotLoadingAndSortResults()
        {
            // Arrange
            _fixture.SetupInitialSearchNotLoading();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemCategoryReducer.OnSearchItemCategoriesFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState, opt => opt.WithStrictOrdering());
        }

        [Fact]
        public void OnSearchItemCategoriesFinished_WithSearchLoading_ShouldSetNotLoadingAndSortResults()
        {
            // Arrange
            _fixture.SetupInitialSearchLoading();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemCategoryReducer.OnSearchItemCategoriesFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState, opt => opt.WithStrictOrdering());
        }

        private sealed class OnSearchItemCategoriesFinishedFixture : ItemCategoryReducerFixture
        {
            public SearchItemCategoriesFinishedAction? Action { get; private set; }

            public void SetupInitialSearchLoading()
            {
                SetupInitialState(true);
            }

            public void SetupInitialSearchNotLoading()
            {
                SetupInitialState(false);
            }

            private void SetupInitialState(bool isLoading)
            {
                InitialState = ExpectedState with
                {
                    Search = ExpectedState.Search with
                    {
                        IsLoadingSearchResults = isLoading,
                        TriggeredAtLeastOnce = false,
                        SearchResults = new DomainTestBuilder<ItemCategorySearchResult>().CreateMany(2).ToList()
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Search = ExpectedState.Search with
                    {
                        IsLoadingSearchResults = false,
                        TriggeredAtLeastOnce = true,
                        SearchResults = new List<ItemCategorySearchResult>
                        {
                            new DomainTestBuilder<ItemCategorySearchResult>()
                                .FillPropertyWith(r => r.Name, $"A{new DomainTestBuilder<string>().Create()}")
                                .Create(),
                            new DomainTestBuilder<ItemCategorySearchResult>()
                                .FillPropertyWith(r => r.Name, $"B{new DomainTestBuilder<string>().Create()}")
                                .Create(),
                            new DomainTestBuilder<ItemCategorySearchResult>()
                                .FillPropertyWith(r => r.Name, $"Z{new DomainTestBuilder<string>().Create()}")
                                .Create()
                        }
                    }
                };
            }

            public void SetupAction()
            {
                Action = new SearchItemCategoriesFinishedAction(ExpectedState.Search.SearchResults.Reverse().ToList());
            }
        }
    }

    private abstract class ItemCategoryReducerFixture
    {
        public ItemCategoryState ExpectedState { get; protected set; } = new DomainTestBuilder<ItemCategoryState>().Create();
        public ItemCategoryState InitialState { get; protected set; } = new DomainTestBuilder<ItemCategoryState>().Create();
    }
}