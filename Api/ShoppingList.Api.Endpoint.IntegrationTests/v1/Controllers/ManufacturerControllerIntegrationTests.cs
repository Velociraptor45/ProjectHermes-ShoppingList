﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Contracts.Common;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Ports;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Infrastructure.Manufacturers.Contexts;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Contexts;
using ProjectHermes.ShoppingList.Api.Infrastructure.StoreItems.Entities;
using ShoppingList.Api.Domain.TestKit.Manufacturers.Models;
using ShoppingList.Api.Domain.TestKit.StoreItems.Models;
using ShoppingList.Api.TestTools.Exceptions;
using System;
using Xunit;
using Manufacturer = ProjectHermes.ShoppingList.Api.Infrastructure.Manufacturers.Entities.Manufacturer;
using Models = ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;

namespace ShoppingList.Api.Endpoint.IntegrationTests.v1.Controllers;

public class ManufacturerControllerIntegrationTests
{
    [Collection("IntegrationTests")]
    public class DeleteManufacturerAsync
    {
        private readonly DeleteManufacturerAsyncFixture _fixture;

        public DeleteManufacturerAsync(DockerFixture dockerFixture)
        {
            _fixture = new DeleteManufacturerAsyncFixture(dockerFixture);
        }

        [Fact]
        public async Task DeleteManufacturerAsync_WithValidData_ShouldReturnOkResult()
        {
            // Arrange
            _fixture.SetupManufacturerId();
            await _fixture.PrepareDatabaseAsync();
            var sut = _fixture.CreateSut();

            // Act
            var response = await sut.DeleteManufacturerAsync(_fixture.ManufacturerId!.Value.Value);

            // Assert
            response.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task DeleteManufacturerAsync_WithValidData_ShouldDeleteManufacturer()
        {
            // Arrange
            _fixture.SetupManufacturerId();
            await _fixture.PrepareDatabaseAsync();
            var sut = _fixture.CreateSut();

            // Act
            await sut.DeleteManufacturerAsync(_fixture.ManufacturerId!.Value.Value);

            // Assert
            var manufacturers = await _fixture.LoadPersistedManufacturersAsync();

            manufacturers.Should().HaveCount(1);
            var manufacturer = manufacturers.First();
            manufacturer.Deleted.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteManufacturerAsync_WithValidData_ShouldRemoveManufacturerIdFromItems()
        {
            // Arrange
            _fixture.SetupManufacturerId();
            await _fixture.PrepareDatabaseAsync();
            var sut = _fixture.CreateSut();

            // Act
            await sut.DeleteManufacturerAsync(_fixture.ManufacturerId!.Value.Value);

            // Assert
            var items = await _fixture.LoadPersistedItemsAsync();

            items.Should().HaveCount(2);
            foreach (var item in items)
            {
                item.ManufacturerId.Should().BeNull();
            }
        }

        [Fact]
        public async Task DeleteManufacturerAsync_WithManufacturerNotExisting_ShouldReturnNotFound()
        {
            // Arrange
            _fixture.SetupManufacturerId();
            _fixture.SetupExpectedFotFoundContract();
            await _fixture.PrepareDatabaseForManufacturerNotExistingAsync();
            var sut = _fixture.CreateSut();

            // Act
            var response = await sut.DeleteManufacturerAsync(_fixture.ManufacturerId!.Value.Value);

            // Assert
            response.Should().BeOfType<NotFoundObjectResult>();
            var notFoundResult = response as NotFoundObjectResult;
            notFoundResult!.Value.Should().BeEquivalentTo(_fixture.ExpectedNotFoundContract);
        }

        private class DeleteManufacturerAsyncFixture : ManufacturerControllerFixture
        {
            public DeleteManufacturerAsyncFixture(DockerFixture dockerFixture) : base(dockerFixture)
            {
            }

            public Models.ManufacturerId? ManufacturerId { get; private set; }
            public ErrorContract? ExpectedNotFoundContract { get; private set; }

            public void SetupManufacturerId()
            {
                ManufacturerId = Models.ManufacturerId.New;
            }

            public async Task PrepareDatabaseForManufacturerNotExistingAsync()
            {
                await ApplyMigrationsAsync(SetupScope);
            }

            public override async Task PrepareDatabaseAsync()
            {
                TestPropertyNotSetException.ThrowIfNull(ManufacturerId);

                using var transaction = await CreateTransactionAsync(SetupScope);
                await ApplyMigrationsAsync(SetupScope);

                // manufacturer
                var manufacturerRepository = CreateManufacturerRepository(SetupScope);
                var manufacturer = new ManufacturerBuilder()
                    .WithIsDeleted(false)
                    .WithId(ManufacturerId.Value)
                    .Create();

                await manufacturerRepository.StoreAsync(manufacturer, default);

                // items
                var itemRepository = CreateItemRepository(SetupScope);
                var items = new List<IStoreItem>()
                {
                    new StoreItemBuilder()
                        .WithManufacturerId(ManufacturerId)
                        .WithIsDeleted(false)
                        .AsItem()
                        .Create(),
                    new StoreItemBuilder()
                        .WithManufacturerId(ManufacturerId)
                        .WithIsDeleted(false)
                        .Create()
                };

                foreach (var item in items)
                {
                    await itemRepository.StoreAsync(item, default);
                }

                await transaction.CommitAsync(default);
            }

            public void SetupExpectedFotFoundContract()
            {
                TestPropertyNotSetException.ThrowIfNull(ManufacturerId);

                ExpectedNotFoundContract = new ErrorContract(
                    $"Manufacturer {ManufacturerId.Value.Value} not found.",
                    ErrorReasonCode.ManufacturerNotFound.ToInt());
            }
        }
    }

    private abstract class ManufacturerControllerFixture : DatabaseFixture, IDisposable
    {
        protected readonly IServiceScope SetupScope;

        protected ManufacturerControllerFixture(DockerFixture dockerFixture) : base(dockerFixture)
        {
            SetupScope = CreateServiceScope();
        }

        public ManufacturerController CreateSut()
        {
            var scope = CreateServiceScope();
            return scope.ServiceProvider.GetRequiredService<ManufacturerController>();
        }

        public override IEnumerable<DbContext> GetDbContexts(IServiceScope scope)
        {
            yield return scope.ServiceProvider.GetRequiredService<ItemContext>();
            yield return scope.ServiceProvider.GetRequiredService<ManufacturerContext>();
        }

        protected ManufacturerContext CreateManufacturerContext(IServiceScope scope)
        {
            return scope.ServiceProvider.GetRequiredService<ManufacturerContext>();
        }

        protected ItemContext CreateItemContext(IServiceScope scope)
        {
            return scope.ServiceProvider.GetRequiredService<ItemContext>();
        }

        protected IManufacturerRepository CreateManufacturerRepository(IServiceScope scope)
        {
            return scope.ServiceProvider.GetRequiredService<IManufacturerRepository>();
        }

        protected IItemRepository CreateItemRepository(IServiceScope scope)
        {
            return scope.ServiceProvider.GetRequiredService<IItemRepository>();
        }

        public async Task<IList<Manufacturer>> LoadPersistedManufacturersAsync()
        {
            using var scope = CreateServiceScope();
            var ctx = CreateManufacturerContext(scope);

            using (await CreateTransactionAsync(scope))
            {
                var entities = await ctx.Manufacturers.AsNoTracking()
                    .ToListAsync();

                return entities;
            }
        }

        public async Task<IList<Item>> LoadPersistedItemsAsync()
        {
            using var scope = CreateServiceScope();
            var ctx = CreateItemContext(scope);

            using (await CreateTransactionAsync(scope))
            {
                var entities = await ctx.Items.AsNoTracking()
                    .ToListAsync();

                return entities;
            }
        }

        public abstract Task PrepareDatabaseAsync();

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                SetupScope.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}