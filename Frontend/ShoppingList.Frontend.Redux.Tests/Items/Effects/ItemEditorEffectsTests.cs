﻿using Microsoft.Extensions.Logging;
using Moq.Contrib.InOrder;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions.Editor;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions.Editor.Availabilities;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.Effects;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;
using RestEase;
using Xunit.Abstractions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.Items.Effects;

public class ItemEditorEffectsTests
{
    public class HandleLoadItemForEditingAction
    {
        private readonly HandleLoadItemForEditingActionFixture _fixture;
        private readonly ILogger<CallQueue> _logger;

        public HandleLoadItemForEditingAction(ITestOutputHelper output)
        {
            _fixture = new HandleLoadItemForEditingActionFixture();
            _logger = output.BuildLoggerFor<CallQueue>();
        }

        [Fact]
        public async Task HandleLoadItemForEditingAction_WithSuccessfulCall_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            _fixture.SetupAction();
            _fixture.SetupReturnedItem();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupGettingItem();
                _fixture.SetupDispatchingFinishAction();
            }, _logger);

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleLoadItemForEditingAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadItemForEditingAction_WithFailedCall_ShouldDispatchExceptionAction()
        {
            // Arrange
            _fixture.SetupAction();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupGettingItemFailed();
                _fixture.SetupDispatchingExceptionAction();
            }, _logger);

            var sut = _fixture.CreateSut();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleLoadItemForEditingAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleLoadItemForEditingActionFixture : ItemEditorEffectsFixture
        {
            private EditedItem? _item;
            public LoadItemForEditingAction? Action { get; private set; }

            public void SetupAction()
            {
                Action = new DomainTestBuilder<LoadItemForEditingAction>().Create();
            }

            public void SetupReturnedItem()
            {
                _item = new DomainTestBuilder<EditedItem>().Create();
            }

            public void SetupGettingItem()
            {
                TestPropertyNotSetException.ThrowIfNull(Action);
                TestPropertyNotSetException.ThrowIfNull(_item);

                ApiClientMock.SetupGetItemByIdAsync(Action.ItemId, _item);
            }

            public void SetupGettingItemFailed()
            {
                TestPropertyNotSetException.ThrowIfNull(Action);

                var exception = new DomainTestBuilder<ApiException>().Create();
                ApiClientMock.SetupGetItemByIdAsyncThrowing(Action.ItemId, exception);
            }

            public void SetupDispatchingStartAction()
            {
                SetupDispatchingAction<LoadItemForEditingStartedAction>();
            }

            public void SetupDispatchingFinishAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_item);

                SetupDispatchingAction(new LoadItemForEditingFinishedAction(_item));
            }

            public void SetupDispatchingExceptionAction()
            {
                SetupDispatchingAnyAction<DisplayApiExceptionNotificationAction>();
            }
        }
    }

    public class HandleAddStoreAction
    {
        private readonly HandleAddStoreActionFixture _fixture;

        public HandleAddStoreAction()
        {
            _fixture = new HandleAddStoreActionFixture();
        }

        [Fact]
        public async Task HandleAddStoreAction_WithItem_ShouldDispatchCorrectActions()
        {
            // Arrange
            _fixture.SetupActionForItem();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingAddedItemAction();
            });

            // Act
            await ItemEditorEffects.HandleAddStoreAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleAddStoreAction_WithItemType_ShouldDispatchCorrectActions()
        {
            // Arrange
            _fixture.SetupActionForItemType();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingAddedItemTypeAction();
            });

            // Act
            await ItemEditorEffects.HandleAddStoreAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleAddStoreActionFixture : ItemEditorEffectsFixture
        {
            public AddStoreAction? Action { get; private set; }

            public void SetupDispatchingAddedItemAction()
            {
                SetupDispatchingAction<StoreAddedToItemAction>();
            }

            public void SetupDispatchingAddedItemTypeAction()
            {
                var itemType = Action.Available as EditedItemType;
                SetupDispatchingAction(new StoreAddedToItemTypeAction(itemType.Id));
            }

            public void SetupActionForItem()
            {
                Action = new AddStoreAction(new DomainTestBuilder<EditedItem>().Create());
            }

            public void SetupActionForItemType()
            {
                Action = new AddStoreAction(new DomainTestBuilder<EditedItemType>().Create());
            }
        }
    }

    private abstract class ItemEditorEffectsFixture : ItemEffectsFixtureBase
    {
        public ItemEditorEffects CreateSut()
        {
            return new ItemEditorEffects(ApiClientMock.Object, ItemStateMock.Object, NavigationManagerMock.Object);
        }
    }
}