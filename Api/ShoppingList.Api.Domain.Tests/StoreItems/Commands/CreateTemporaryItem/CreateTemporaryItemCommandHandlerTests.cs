﻿using AutoFixture;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions.Reason;
using ProjectHermes.ShoppingList.Api.Domain.Common.Models;
using ProjectHermes.ShoppingList.Api.Domain.Common.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Commands.CreateTemporaryItem;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models.Factories;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Fixtures;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.StoreItems.Commands.CreateTemporaryItem
{
    public class CreateTemporaryItemCommandHandlerTests
    {
        private readonly CommonFixture commonFixture;
        private readonly StoreItemAvailabilityFixture storeItemAvailabilityFixture;
        private readonly StoreItemSectionFixture storeItemSectionFixture;
        private readonly StoreFixture storeFixture;
        private readonly StoreItemFixture storeItemFixture;

        public CreateTemporaryItemCommandHandlerTests()
        {
            commonFixture = new CommonFixture();
            storeItemAvailabilityFixture = new StoreItemAvailabilityFixture(commonFixture);
            storeItemSectionFixture = new StoreItemSectionFixture(commonFixture);
            storeFixture = new StoreFixture(commonFixture);
            storeItemFixture = new StoreItemFixture(new StoreItemAvailabilityFixture(commonFixture), commonFixture);
        }

        [Fact]
        public async Task HandleAsync_WithCommandIsNull_ShouldThrowArgumentNullException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

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
        public async Task HandleAsync_WithDeletedStore_ShouldThrowDomainException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            Mock<IStoreRepository> storeRepositoryMock = fixture.Freeze<Mock<IStoreRepository>>();

            var handler = fixture.Create<CreateTemporaryItemCommandHandler>();
            var command = fixture.Create<CreateTemporaryItemCommand>();

            IStore store = storeFixture.GetStore(command.TemporaryItemCreation.Availability.StoreId, isDeleted: true);

            storeRepositoryMock.SetupFindByAsync(command.TemporaryItemCreation.Availability.StoreId, store);

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                (await action.Should().ThrowAsync<DomainException>())
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.StoreNotFound);
            }
        }

        [Fact]
        public async Task HandleAsync_WithStoreIsNull_ShouldThrowDomainException()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            Mock<IStoreRepository> storeRepositoryMock = fixture.Freeze<Mock<IStoreRepository>>();

            var handler = fixture.Create<CreateTemporaryItemCommandHandler>();
            var command = fixture.Create<CreateTemporaryItemCommand>();

            storeRepositoryMock.SetupFindByAsync(command.TemporaryItemCreation.Availability.StoreId, null);

            // Act
            Func<Task<bool>> action = async () => await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                (await action.Should().ThrowAsync<DomainException>())
                    .Where(e => e.Reason.ErrorCode == ErrorReasonCode.StoreNotFound);
            }
        }

        [Fact]
        public async Task HandleAsync_WithValidCommand_ShouldStoreItem()
        {
            // Arrange
            var fixture = commonFixture.GetNewFixture();

            Mock<IStoreItemFactory> storeItemFactoryMock = fixture.Freeze<Mock<IStoreItemFactory>>();
            Mock<IItemRepository> itemRepositoryMock = fixture.Freeze<Mock<IItemRepository>>();
            Mock<IStoreRepository> storeRepositoryMock = fixture.Freeze<Mock<IStoreRepository>>();
            Mock<IStoreItemAvailabilityFactory> availabilityFactoryMock = fixture.Freeze<Mock<IStoreItemAvailabilityFactory>>();
            Mock<IStoreItemSectionReadRepository> sectionReadRepositoryMock = fixture.Freeze<Mock<IStoreItemSectionReadRepository>>();

            var handler = fixture.Create<CreateTemporaryItemCommandHandler>();
            var command = fixture.Create<CreateTemporaryItemCommand>();

            // setup sections
            StoreItemSectionId sectionId = command.TemporaryItemCreation.Availability.StoreItemSectionId;
            IStoreItemSection section = storeItemSectionFixture.Create(sectionId);
            sectionReadRepositoryMock.SetupFindByAsync(sectionId.ToMonoList(), section.ToMonoList());

            // setup availabilities
            IStoreItemAvailability availability = storeItemAvailabilityFixture.GetAvailability(section);
            availabilityFactoryMock.SetupCreate(availability);

            IStoreItem storeItem = storeItemFixture.GetStoreItem();
            IStore store = storeFixture.GetStore(command.TemporaryItemCreation.Availability.StoreId, isDeleted: false);

            storeRepositoryMock.SetupFindByAsync(command.TemporaryItemCreation.Availability.StoreId, store);
            storeItemFactoryMock.SetupCreate(command.TemporaryItemCreation, availability, storeItem);

            // Act
            var result = await handler.HandleAsync(command, default);

            // Assert
            using (new AssertionScope())
            {
                result.Should().BeTrue();
                itemRepositoryMock.Verify(
                    i => i.StoreAsync(It.Is<IStoreItem>(item => item == storeItem),
                    It.IsAny<CancellationToken>()),
                    Times.Once);
            }
        }

        //todo further tests
    }
}