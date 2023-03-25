﻿using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Actions.SearchBar;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.Reducers;
using ProjectHermes.ShoppingList.Frontend.Redux.ShoppingList.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.ShoppingLists.Reducers;

public class ShoppingListSearchBarReducerTests
{
    public class OnItemForShoppingListSearchInputChanged
    {
        private readonly OnItemForShoppingListSearchInputChangedFixture _fixture;

        public OnItemForShoppingListSearchInputChanged()
        {
            _fixture = new OnItemForShoppingListSearchInputChangedFixture();
        }

        [Fact]
        public void OnItemForShoppingListSearchInputChanged_WithValidInput_WithButtonDisabled_ShouldSetInputAndEnableButton()
        {
            // Arrange
            _fixture.SetupInput();
            _fixture.SetupInitialButtonDisabled();
            _fixture.SetupExpectedButtonEnabled();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListSearchBarReducer.OnItemForShoppingListSearchInputChanged(_fixture.InitialState,
                _fixture.Action!);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnItemForShoppingListSearchInputChanged_WithValidInput_WithButtonEnabled_ShouldSetInputAndEnableButton()
        {
            // Arrange
            _fixture.SetupInput();
            _fixture.SetupInitialButtonEnabled();
            _fixture.SetupExpectedButtonEnabled();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListSearchBarReducer.OnItemForShoppingListSearchInputChanged(_fixture.InitialState,
                _fixture.Action!);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void OnItemForShoppingListSearchInputChanged_WithInvalidInput_WithButtonDisabled_ShouldSetInputAndDisableButton(
            string input)
        {
            // Arrange
            _fixture.SetupInput(input);
            _fixture.SetupInitialButtonDisabled();
            _fixture.SetupExpectedButtonDisabled();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListSearchBarReducer.OnItemForShoppingListSearchInputChanged(_fixture.InitialState,
                _fixture.Action!);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void OnItemForShoppingListSearchInputChanged_WithInvalidInput_WithButtonEnabled_ShouldSetInputAndDisableButton(
                string input)
        {
            // Arrange
            _fixture.SetupInput(input);
            _fixture.SetupInitialButtonEnabled();
            _fixture.SetupExpectedButtonDisabled();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListSearchBarReducer.OnItemForShoppingListSearchInputChanged(_fixture.InitialState,
                _fixture.Action!);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnItemForShoppingListSearchInputChangedFixture : ShoppingListSearchBarReducerFixture
        {
            private string? _input;

            public ItemForShoppingListSearchInputChangedAction? Action { get; private set; }

            public void SetupInput(string? input = null)
            {
                _input = input ?? new DomainTestBuilder<string>().Create();
            }

            public void SetupInitialButtonEnabled()
            {
                SetupInitialState(true);
            }

            public void SetupInitialButtonDisabled()
            {
                SetupInitialState(false);
            }

            private void SetupInitialState(bool isButtonEnabled)
            {
                InitialState = ExpectedState with
                {
                    SearchBar = ExpectedState.SearchBar with
                    {
                        Input = new DomainTestBuilder<string>().Create()
                    },
                    TemporaryItemCreator = ExpectedState.TemporaryItemCreator with
                    {
                        IsButtonEnabled = isButtonEnabled
                    }
                };
            }

            public void SetupExpectedButtonEnabled()
            {
                SetupExpectedState(true);
            }

            public void SetupExpectedButtonDisabled()
            {
                SetupExpectedState(false);
            }

            private void SetupExpectedState(bool isButtonEnabled)
            {
                TestPropertyNotSetException.ThrowIfNull(_input);

                ExpectedState = ExpectedState with
                {
                    SearchBar = ExpectedState.SearchBar with
                    {
                        Input = _input.Trim(),
                        Results = isButtonEnabled
                            ? ExpectedState.SearchBar.Results
                            : new List<SearchItemForShoppingListResult>()
                    },
                    TemporaryItemCreator = ExpectedState.TemporaryItemCreator with
                    {
                        IsButtonEnabled = isButtonEnabled
                    }
                };
            }

            public void SetupAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_input);

                Action = new ItemForShoppingListSearchInputChangedAction(_input);
            }
        }
    }

    public class OnSearchItemForShoppingListFinished
    {
        private readonly OnSearchItemForShoppingListFinishedFixture _fixture;

        public OnSearchItemForShoppingListFinished()
        {
            _fixture = new OnSearchItemForShoppingListFinishedFixture();
        }

        [Fact]
        public void OnSearchItemForShoppingListFinished_WithValidData_ShouldSetAndOrderSearchResults()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListSearchBarReducer.OnSearchItemForShoppingListFinished(_fixture.InitialState,
                _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState, opt => opt.WithStrictOrdering());
        }

        private sealed class OnSearchItemForShoppingListFinishedFixture : ShoppingListSearchBarReducerFixture
        {
            public SearchItemForShoppingListFinishedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    SearchBar = ExpectedState.SearchBar with
                    {
                        Results = new DomainTestBuilder<SearchItemForShoppingListResult>().CreateMany(2).ToList()
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    SearchBar = ExpectedState.SearchBar with
                    {
                        Results = new List<SearchItemForShoppingListResult>
                        {
                            new DomainTestBuilder<SearchItemForShoppingListResult>()
                                .FillPropertyWith(r => r.Name, $"A{new DomainTestBuilder<string>().Create()}")
                                .Create(),
                            new DomainTestBuilder<SearchItemForShoppingListResult>()
                                .FillPropertyWith(r => r.Name, $"B{new DomainTestBuilder<string>().Create()}")
                                .Create(),
                            new DomainTestBuilder<SearchItemForShoppingListResult>()
                                .FillPropertyWith(r => r.Name, $"Z{new DomainTestBuilder<string>().Create()}")
                                .Create()
                        }
                    }
                };
            }

            public void SetupAction()
            {
                Action = new SearchItemForShoppingListFinishedAction(ExpectedState.SearchBar.Results);
            }
        }
    }

    public class OnItemForShoppingListSearchResultSelected
    {
        private readonly OnItemForShoppingListSearchResultSelectedFixture _fixture;

        public OnItemForShoppingListSearchResultSelected()
        {
            _fixture = new OnItemForShoppingListSearchResultSelectedFixture();
        }

        [Fact]
        public void OnItemForShoppingListSearchResultSelected_WithValidData_ShouldClearInputAndSearchResults()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = ShoppingListSearchBarReducer.OnItemForShoppingListSearchResultSelected(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnItemForShoppingListSearchResultSelectedFixture : ShoppingListSearchBarReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    SearchBar = ExpectedState.SearchBar with
                    {
                        Input = new DomainTestBuilder<string>().Create(),
                        Results = new DomainTestBuilder<SearchItemForShoppingListResult>().CreateMany(2).ToList()
                    },
                    TemporaryItemCreator = ExpectedState.TemporaryItemCreator with
                    {
                        ItemName = new DomainTestBuilder<string>().Create()
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    SearchBar = ExpectedState.SearchBar with
                    {
                        Input = string.Empty,
                        Results = new List<SearchItemForShoppingListResult>()
                    },
                    TemporaryItemCreator = ExpectedState.TemporaryItemCreator with
                    {
                        ItemName = string.Empty
                    }
                };
            }
        }
    }

    public class OnSetSearchBarActive
    {
        private readonly OnSetSearchBarActiveFixture _fixture;

        public OnSetSearchBarActive()
        {
            _fixture = new OnSetSearchBarActiveFixture();
        }

        [Fact]
        public void OnSetSearchBarActive_WithSearchBarInactive_ShouldSetSearchBarActive()
        {
            // Arrange
            _fixture.SetupSearchBarInactive();
            _fixture.SetupExpectedState();

            // Act
            var result = ShoppingListSearchBarReducer.OnSetSearchBarActive(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSetSearchBarActive_WithSearchBarActive_ShouldSetSearchBarActive()
        {
            // Arrange
            _fixture.SetupSearchBarActive();
            _fixture.SetupExpectedState();

            // Act
            var result = ShoppingListSearchBarReducer.OnSetSearchBarActive(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSetSearchBarActiveFixture : ShoppingListSearchBarReducerFixture
        {
            public void SetupSearchBarActive()
            {
                SetupInitialState(true);
            }

            public void SetupSearchBarInactive()
            {
                SetupInitialState(false);
            }

            private void SetupInitialState(bool isActive)
            {
                InitialState = ExpectedState with
                {
                    SearchBar = ExpectedState.SearchBar with
                    {
                        IsActive = isActive
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    SearchBar = ExpectedState.SearchBar with
                    {
                        IsActive = true
                    }
                };
            }
        }
    }

    public class OnSetSearchBarInactive
    {
        private readonly OnSetSearchBarInactiveFixture _fixture;

        public OnSetSearchBarInactive()
        {
            _fixture = new OnSetSearchBarInactiveFixture();
        }

        [Fact]
        public void OnSetSearchBarInactive_WithSearchBarInactive_ShouldSetSearchBarInactive()
        {
            // Arrange
            _fixture.SetupSearchBarInactive();
            _fixture.SetupExpectedState();

            // Act
            var result = ShoppingListSearchBarReducer.OnSetSearchBarInactive(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSetSearchBarInactive_WithSearchBarActive_ShouldSetSearchBarInactive()
        {
            // Arrange
            _fixture.SetupSearchBarActive();
            _fixture.SetupExpectedState();

            // Act
            var result = ShoppingListSearchBarReducer.OnSetSearchBarInactive(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSetSearchBarInactiveFixture : ShoppingListSearchBarReducerFixture
        {
            public void SetupSearchBarActive()
            {
                SetupInitialState(true);
            }

            public void SetupSearchBarInactive()
            {
                SetupInitialState(false);
            }

            private void SetupInitialState(bool isActive)
            {
                InitialState = ExpectedState with
                {
                    SearchBar = ExpectedState.SearchBar with
                    {
                        IsActive = isActive
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    SearchBar = ExpectedState.SearchBar with
                    {
                        IsActive = false
                    }
                };
            }
        }
    }

    private abstract class ShoppingListSearchBarReducerFixture
    {
        public ShoppingListState ExpectedState { get; protected set; } = new DomainTestBuilder<ShoppingListState>().Create();
        public ShoppingListState InitialState { get; protected set; } = new DomainTestBuilder<ShoppingListState>().Create();
    }
}