﻿using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ShoppingList.Api.Domain.Commands.CreateTemporaryItem;
using ShoppingList.Api.Domain.Models;
using ShoppingList.Api.Domain.Ports;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Commands.CreateTemporaryItem
{
    public class CreateTemporaryItemCommandHandlerTests
    {
        public Fixture GetNewFixture()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
            return fixture;
        }

        [Fact]
        public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var fixture = GetNewFixture();
            var handler = fixture.Create<CreateTemporaryItemCommandHandler>();

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(null, default);

            // Assert
            using (new AssertionScope())
            {
                await action.Should().ThrowAsync<ArgumentNullException>();
            }
        }

        [Fact]
        public async Task HandleAsync_WithValidCommand_ShouldStoreItemAndReturnTrue()
        {
            // Arrange
            var fixture = GetNewFixture();
            var repositoryMock = fixture.Freeze<Mock<IItemRepository>>();
            var handler = fixture.Create<CreateTemporaryItemCommandHandler>();
            var command = fixture.Create<CreateTemporaryItemCommand>();

            // Act
            var result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                repositoryMock.Verify(
                    i => i.StoreAsync(It.IsAny<StoreItem>(), It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }
    }
}