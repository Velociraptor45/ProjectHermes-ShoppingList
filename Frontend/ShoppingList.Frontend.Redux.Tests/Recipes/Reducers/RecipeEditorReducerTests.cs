﻿using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Actions.Editor;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.Reducers;
using ProjectHermes.ShoppingList.Frontend.Redux.Recipes.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.Recipes.Reducers;

public class RecipeEditorReducerTests
{
    public class OnSetNewRecipe
    {
        private readonly OnSetNewRecipeFixture _fixture;

        public OnSetNewRecipe()
        {
            _fixture = new OnSetNewRecipeFixture();
        }

        [Fact]
        public void OnSetNewRecipe_WithValidData_ShouldSetRecipeAndLeaveEditMode()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = RecipeEditorReducer.OnSetNewRecipe(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState,
                opt => opt.Excluding(info =>
                    info.Path == "Editor.Recipe.Ingredients[0].Key"
                    || info.Path == "Editor.Recipe.PreparationSteps[0].Key"));
        }

        private sealed class OnSetNewRecipeFixture : RecipeEditorReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = new DomainTestBuilder<EditedRecipe>().Create(),
                        IsInEditMode = false
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = new EditedRecipe(
                            Guid.Empty,
                            string.Empty,
                            1,
                            new List<EditedIngredient>
                            {
                                new(
                                    Guid.NewGuid(),
                                    Guid.Empty,
                                    string.Empty,
                                    Guid.Empty,
                                    ExpectedState.IngredientQuantityTypes.First().Id,
                                    1,
                                    null,
                                    null,
                                    null,
                                    null,
                                    new ItemCategorySelector(
                                        new List<ItemCategorySearchResult>(0),
                                        string.Empty),
                                    new ItemSelector(new List<SearchItemByItemCategoryResult>(0)))
                            },
                            new SortedSet<EditedPreparationStep>
                            {
                                new(Guid.NewGuid(), Guid.Empty, string.Empty, 0)
                            },
                            new List<Guid>(0)),
                        IsInEditMode = true
                    }
                };
            }
        }
    }

    public class OnLoadRecipeForEditingFinished
    {
        private readonly OnLoadRecipeForEditingFinishedFixture _fixture;

        public OnLoadRecipeForEditingFinished()
        {
            _fixture = new OnLoadRecipeForEditingFinishedFixture();
        }

        [Fact]
        public void OnLoadRecipeForEditingFinished_WithValidData_ShouldSetRecipeAndLeaveEditMode()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = RecipeEditorReducer.OnLoadRecipeForEditingFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnLoadRecipeForEditingFinishedFixture : RecipeEditorReducerFixture
        {
            public LoadRecipeForEditingFinishedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = new DomainTestBuilder<EditedRecipe>().Create(),
                        IsInEditMode = true
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsInEditMode = false
                    }
                };
            }

            public void SetupAction()
            {
                Action = new LoadRecipeForEditingFinishedAction(ExpectedState.Editor.Recipe!);
            }
        }
    }

    public class OnRecipeNameChanged
    {
        private readonly OnRecipeNameChangedFixture _fixture;

        public OnRecipeNameChanged()
        {
            _fixture = new OnRecipeNameChangedFixture();
        }

        [Fact]
        public void OnRecipeNameChanged_WithValidData_ShouldSetName()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = RecipeEditorReducer.OnRecipeNameChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnRecipeNameChanged_WithRecipeNull_ShouldNotChangeState()
        {
            // Arrange
            _fixture.SetupInitialStateWithRecipeNull();
            _fixture.SetupExpectedStateWithRecipeNull();
            _fixture.SetupActionForRecipeNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = RecipeEditorReducer.OnRecipeNameChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnRecipeNameChangedFixture : RecipeEditorReducerFixture
        {
            public RecipeNameChangedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = ExpectedState.Editor.Recipe! with
                        {
                            Name = new DomainTestBuilder<string>().Create()
                        }
                    }
                };
            }

            public void SetupInitialStateWithRecipeNull()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = null
                    }
                };
            }

            public void SetupExpectedStateWithRecipeNull()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = null
                    }
                };
            }

            public void SetupAction()
            {
                Action = new RecipeNameChangedAction(ExpectedState.Editor.Recipe!.Name);
            }

            public void SetupActionForRecipeNull()
            {
                Action = new DomainTestBuilder<RecipeNameChangedAction>().Create();
            }
        }
    }

    public class OnToggleEditMode
    {
        private readonly OnToggleEditModeFixture _fixture;

        public OnToggleEditMode()
        {
            _fixture = new OnToggleEditModeFixture();
        }

        [Fact]
        public void OnToggleEditMode_WithEditorInEditMode_ShouldSetNotInEditMode()
        {
            // Arrange
            _fixture.SetupInitialStateInEditMode();
            _fixture.SetupExpectedStateNotInEditMode();

            // Act
            var result = RecipeEditorReducer.OnToggleEditMode(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnToggleEditMode_WithEditorNotInEditMode_ShouldSetInEditMode()
        {
            // Arrange
            _fixture.SetupInitialStateNotInEditMode();
            _fixture.SetupExpectedStateInEditMode();

            // Act
            var result = RecipeEditorReducer.OnToggleEditMode(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnToggleEditModeFixture : RecipeEditorReducerFixture
        {
            public void SetupInitialStateInEditMode()
            {
                SetupInitialState(true);
            }

            public void SetupInitialStateNotInEditMode()
            {
                SetupInitialState(false);
            }

            public void SetupInitialState(bool isInEditMode)
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsInEditMode = isInEditMode
                    }
                };
            }

            public void SetupExpectedStateInEditMode()
            {
                SetupExpectedState(true);
            }

            public void SetupExpectedStateNotInEditMode()
            {
                SetupExpectedState(false);
            }

            public void SetupExpectedState(bool isInEditMode)
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsInEditMode = isInEditMode
                    }
                };
            }
        }
    }

    public class OnRecipeTagInputChanged
    {
        private readonly OnRecipeTagInputChangedFixture _fixture;

        public OnRecipeTagInputChanged()
        {
            _fixture = new OnRecipeTagInputChangedFixture();
        }

        [Fact]
        public void OnRecipeTagInputChanged_WithValidData_ShouldSetInput()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = RecipeEditorReducer.OnRecipeTagInputChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnRecipeTagInputChangedFixture : RecipeEditorReducerFixture
        {
            public RecipeTagInputChangedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        RecipeTagCreateInput = new DomainTestBuilder<string>().Create()
                    }
                };
            }

            public void SetupAction()
            {
                Action = new RecipeTagInputChangedAction(ExpectedState.Editor.RecipeTagCreateInput);
            }
        }
    }

    public class OnRecipeTagsDropdownClosed
    {
        private readonly OnRecipeTagsDropdownClosedFixture _fixture;

        public OnRecipeTagsDropdownClosed()
        {
            _fixture = new OnRecipeTagsDropdownClosedFixture();
        }

        [Fact]
        public void OnRecipeTagsDropdownClosed_ShouldClearRecipeTagInput()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = RecipeEditorReducer.OnRecipeTagsDropdownClosed(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnRecipeTagsDropdownClosedFixture : RecipeEditorReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        RecipeTagCreateInput = new DomainTestBuilder<string>().Create()
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        RecipeTagCreateInput = string.Empty
                    }
                };
            }
        }
    }

    public class OnRecipeTagsChanged
    {
        private readonly OnRecipeTagsChangedFixture _fixture;

        public OnRecipeTagsChanged()
        {
            _fixture = new OnRecipeTagsChangedFixture();
        }

        [Fact]
        public void OnRecipeTagsChanged_WithValidData_ShouldSetRecipeTagIds()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = RecipeEditorReducer.OnRecipeTagsChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnRecipeTagsChanged_WithRecipeNull_ShouldNotChangeState()
        {
            // Arrange
            _fixture.SetupInitialStateWithRecipeNull();
            _fixture.SetupExpectedStateWithRecipeNull();
            _fixture.SetupActionForRecipeNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = RecipeEditorReducer.OnRecipeTagsChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnRecipeTagsChangedFixture : RecipeEditorReducerFixture
        {
            public RecipeTagsChangedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = ExpectedState.Editor.Recipe! with
                        {
                            RecipeTagIds = new DomainTestBuilder<Guid>().CreateMany(2).ToList()
                        }
                    }
                };
            }

            public void SetupInitialStateWithRecipeNull()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = null
                    }
                };
            }

            public void SetupExpectedStateWithRecipeNull()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = null
                    }
                };
            }

            public void SetupAction()
            {
                Action = new RecipeTagsChangedAction(ExpectedState.Editor.Recipe!.RecipeTagIds);
            }

            public void SetupActionForRecipeNull()
            {
                Action = new DomainTestBuilder<RecipeTagsChangedAction>().Create();
            }
        }
    }

    public class OnCreateNewRecipeTagFinished
    {
        private readonly OnCreateNewRecipeTagFinishedFixture _fixture;

        public OnCreateNewRecipeTagFinished()
        {
            _fixture = new OnCreateNewRecipeTagFinishedFixture();
        }

        [Fact]
        public void OnCreateNewRecipeTagFinished_WithValidData_ShouldSetRecipeTagIds()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = RecipeEditorReducer.OnCreateNewRecipeTagFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnCreateNewRecipeTagFinished_WithRecipeNull_ShouldNotChangeState()
        {
            // Arrange
            _fixture.SetupInitialStateWithRecipeNull();
            _fixture.SetupExpectedStateWithRecipeNull();
            _fixture.SetupActionForRecipeNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = RecipeEditorReducer.OnCreateNewRecipeTagFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnCreateNewRecipeTagFinishedFixture : RecipeEditorReducerFixture
        {
            public CreateNewRecipeTagFinishedAction? Action { get; private set; }

            public void SetupExpectedState()
            {
                var tag = ExpectedState.RecipeTags.ElementAt(1);
                var recipeTagIds = ExpectedState.Editor.Recipe!.RecipeTagIds.ToList();
                recipeTagIds.RemoveAt(recipeTagIds.Count - 1);
                recipeTagIds.Add(tag.Id);

                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = ExpectedState.Editor.Recipe! with
                        {
                            RecipeTagIds = recipeTagIds
                        }
                    }
                };
            }

            public void SetupInitialState()
            {
                var allTags = ExpectedState.RecipeTags.ToList();
                allTags.RemoveAt(1);

                var recipeTagIds = ExpectedState.Editor.Recipe!.RecipeTagIds.ToList();
                recipeTagIds.RemoveAt(recipeTagIds.Count - 1);

                InitialState = ExpectedState with
                {
                    RecipeTags = allTags,
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = ExpectedState.Editor.Recipe! with
                        {
                            RecipeTagIds = recipeTagIds
                        }
                    }
                };
            }

            public void SetupInitialStateWithRecipeNull()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = null
                    }
                };
            }

            public void SetupExpectedStateWithRecipeNull()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = null
                    }
                };
            }

            public void SetupAction()
            {
                Action = new CreateNewRecipeTagFinishedAction(ExpectedState.RecipeTags.ElementAt(1));
            }

            public void SetupActionForRecipeNull()
            {
                Action = new DomainTestBuilder<CreateNewRecipeTagFinishedAction>().Create();
            }
        }
    }

    public class OnRecipeNumberOfServingsChanged
    {
        private readonly OnRecipeNumberOfServingsChangedFixture _fixture;

        public OnRecipeNumberOfServingsChanged()
        {
            _fixture = new OnRecipeNumberOfServingsChangedFixture();
        }

        [Fact]
        public void OnRecipeNumberOfServingsChanged_WithValidData_ShouldSetNumberOfServings()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = RecipeEditorReducer.OnRecipeNumberOfServingsChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnRecipeNumberOfServingsChanged_WithRecipeNull_ShouldNotChangeState()
        {
            // Arrange
            _fixture.SetupInitialStateWithRecipeNull();
            _fixture.SetupExpectedStateWithRecipeNull();
            _fixture.SetupActionForRecipeNull();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = RecipeEditorReducer.OnRecipeNumberOfServingsChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnRecipeNumberOfServingsChangedFixture : RecipeEditorReducerFixture
        {
            public RecipeNumberOfServingsChangedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = ExpectedState.Editor.Recipe! with
                        {
                            NumberOfServings = new DomainTestBuilder<int>().Create()
                        }
                    }
                };
            }

            public void SetupInitialStateWithRecipeNull()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = null
                    }
                };
            }

            public void SetupExpectedStateWithRecipeNull()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Recipe = null
                    }
                };
            }

            public void SetupAction()
            {
                Action = new RecipeNumberOfServingsChangedAction(ExpectedState.Editor.Recipe!.NumberOfServings);
            }

            public void SetupActionForRecipeNull()
            {
                Action = new DomainTestBuilder<RecipeNumberOfServingsChangedAction>().Create();
            }
        }
    }

    public class OnModifyRecipeStarted
    {
        private readonly OnModifyRecipeStartedFixture _fixture;

        public OnModifyRecipeStarted()
        {
            _fixture = new OnModifyRecipeStartedFixture();
        }

        [Fact]
        public void OnModifyRecipeStarted_WithNotSaving_ShouldSetSaving()
        {
            // Arrange
            _fixture.SetupInitialStateNotSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = RecipeEditorReducer.OnModifyRecipeStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnModifyRecipeStarted_WithSaving_ShouldNotChangeState()
        {
            // Arrange
            _fixture.SetupInitialStateSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = RecipeEditorReducer.OnModifyRecipeStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.InitialState);
        }

        private sealed class OnModifyRecipeStartedFixture : RecipeEditorReducerFixture
        {
            public void SetupInitialStateNotSaving()
            {
                SetupInitialState(false);
            }

            public void SetupInitialStateSaving()
            {
                SetupInitialState(true);
            }

            private void SetupInitialState(bool isSaving)
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsSaving = isSaving,
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsSaving = true
                    }
                };
            }
        }
    }

    public class OnModifyRecipeFinished
    {
        private readonly OnModifyRecipeFinishedFixture _fixture;

        public OnModifyRecipeFinished()
        {
            _fixture = new OnModifyRecipeFinishedFixture();
        }

        [Fact]
        public void OnModifyRecipeFinished_WithSaving_ShouldSetNotSaving()
        {
            // Arrange
            _fixture.SetupInitialStateSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = RecipeEditorReducer.OnModifyRecipeFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnModifyRecipeFinished_WithNotSaving_ShouldNotChangeState()
        {
            // Arrange
            _fixture.SetupInitialStateNotSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = RecipeEditorReducer.OnModifyRecipeFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.InitialState);
        }

        private sealed class OnModifyRecipeFinishedFixture : RecipeEditorReducerFixture
        {
            public void SetupInitialStateNotSaving()
            {
                SetupInitialState(false);
            }

            public void SetupInitialStateSaving()
            {
                SetupInitialState(true);
            }

            private void SetupInitialState(bool isSaving)
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsSaving = isSaving,
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsSaving = false
                    }
                };
            }
        }
    }

    public class OnCreateRecipeStarted
    {
        private readonly OnCreateRecipeStartedFixture _fixture;

        public OnCreateRecipeStarted()
        {
            _fixture = new OnCreateRecipeStartedFixture();
        }

        [Fact]
        public void OnCreateRecipeStarted_WithNotSaving_ShouldSetSaving()
        {
            // Arrange
            _fixture.SetupInitialStateNotSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = RecipeEditorReducer.OnCreateRecipeStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnCreateRecipeStarted_WithSaving_ShouldNotChangeState()
        {
            // Arrange
            _fixture.SetupInitialStateSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = RecipeEditorReducer.OnCreateRecipeStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.InitialState);
        }

        private sealed class OnCreateRecipeStartedFixture : RecipeEditorReducerFixture
        {
            public void SetupInitialStateNotSaving()
            {
                SetupInitialState(false);
            }

            public void SetupInitialStateSaving()
            {
                SetupInitialState(true);
            }

            private void SetupInitialState(bool isSaving)
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsSaving = isSaving,
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsSaving = true
                    }
                };
            }
        }
    }

    public class OnCreateRecipeFinished
    {
        private readonly OnCreateRecipeFinishedFixture _fixture;

        public OnCreateRecipeFinished()
        {
            _fixture = new OnCreateRecipeFinishedFixture();
        }

        [Fact]
        public void OnCreateRecipeFinished_WithSaving_ShouldSetNotSaving()
        {
            // Arrange
            _fixture.SetupInitialStateSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = RecipeEditorReducer.OnCreateRecipeFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnCreateRecipeFinished_WithNotSaving_ShouldNotChangeState()
        {
            // Arrange
            _fixture.SetupInitialStateNotSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = RecipeEditorReducer.OnCreateRecipeFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.InitialState);
        }

        private sealed class OnCreateRecipeFinishedFixture : RecipeEditorReducerFixture
        {
            public void SetupInitialStateNotSaving()
            {
                SetupInitialState(false);
            }

            public void SetupInitialStateSaving()
            {
                SetupInitialState(true);
            }

            private void SetupInitialState(bool isSaving)
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsSaving = isSaving,
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsSaving = false
                    }
                };
            }
        }
    }

    private abstract class RecipeEditorReducerFixture
    {
        public RecipeState ExpectedState { get; protected set; } = new DomainTestBuilder<RecipeState>().Create();
        public RecipeState InitialState { get; protected set; } = new DomainTestBuilder<RecipeState>().Create();
    }
}