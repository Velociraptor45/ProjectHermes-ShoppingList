﻿using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Core.Tests.AutoFixture;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

using DomainModels = ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.ShoppingLists.Commands.ChangeItemQuantityOnShoppingList
{
    public class ChangeItemQuantityOnShoppingListCommandHandlerTests
    {
        private readonly CommonFixture commonFixture;
        private readonly ShoppingListItemFixture shoppingListItemFixture;
        private readonly ShoppingListFixture shoppingListFixture;
        private readonly StoreItemAvailabilityFixture storeItemAvailabilityFixture;
        private readonly StoreItemFixture storeItemFixture;

        public ChangeItemQuantityOnShoppingListCommandHandlerTests()
        {
            commonFixture = new CommonFixture();
            shoppingListItemFixture = new ShoppingListItemFixture(commonFixture);
            shoppingListFixture = new ShoppingListFixture(shoppingListItemFixture, commonFixture);
            storeItemAvailabilityFixture = new StoreItemAvailabilityFixture(commonFixture);
            storeItemFixture = new StoreItemFixture(storeItemAvailabilityFixture, commonFixture);
        }

        /*
         * all valid
         *
         */

        [Fact]
        public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();
            var handler = fixture.Create<ChangeItemQuantityOnShoppingListCommandHandler>();

            // Act
            Func<Task> function = async () => await handler.HandleAsync(null, default);

            // Assert
            using (new AssertionScope())
            {
                await function.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task HandleAsync_WithInvalidShoppingListId_ShouldThrowDomainException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();
            var shoppingListRepoMock = fixture.Freeze<Mock<IShoppingListRepository>>();
            var handler = fixture.Create<ChangeItemQuantityOnShoppingListCommandHandler>();
            var command = fixture.Create<ChangeItemQuantityOnShoppingListCommand>();

            shoppingListRepoMock
                .Setup(instance => instance.FindByAsync(
                    It.Is<ShoppingListId>(id => id == command.ShoppingListId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult<IShoppingList>(null));

            // Act
            Func<Task> function = async () => await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                (await function.Should().ThrowAsync<DomainException>())
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.ShoppingListNotFound);
            }
        }

        [Fact]
        public async Task HandleAsync_WithValidData_ShouldChangeItemQuantityAndStoreIt()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();
            var shoppingListRepoMock = fixture.Freeze<Mock<IShoppingListRepository>>();
            var handler = fixture.Create<ChangeItemQuantityOnShoppingListCommandHandler>();
            fixture.ConstructorArgumentFor<ChangeItemQuantityOnShoppingListCommand, float>("quantity", commonFixture.NextFloat() + .01f);
            var command = fixture.Create<ChangeItemQuantityOnShoppingListCommand>();

            var item = shoppingListItemFixture.GetShoppingListItemWithId(command.ItemId);
            IShoppingList shoppingList = shoppingListFixture.GetShoppingList(command.ShoppingListId, additionalItems: item.ToMonoList());

            shoppingListRepoMock
                .Setup(instance => instance.FindByAsync(
                    It.Is<ShoppingListId>(id => id == command.ShoppingListId),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(shoppingList));

            // Act
            bool result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                shoppingListRepoMock.Verify(
                    i => i.StoreAsync(
                        It.Is<DomainModels.ShoppingList>(list => list.Id == shoppingList.Id),
                        It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }
    }
}