using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Commands.CreateItemCategory;
using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.v1.Controllers.ItemCategoryControllerTests;

public class CreateItemCategoryAsyncTests : ControllerCommandWithReturnTypeTestsBase<ItemCategoryController,
    CreateItemCategoryCommand, IItemCategory, ItemCategoryContract,
    CreateItemCategoryAsyncTests.CreateItemCategoryAsyncFixture>
{
    public CreateItemCategoryAsyncTests() : base(new CreateItemCategoryAsyncFixture())
    {
    }

    public sealed class CreateItemCategoryAsyncFixture : ControllerCommandWithReturnTypeFixtureBase
    {
        private string? _name;

        public CreateItemCategoryAsyncFixture()
        {
            PossibleResultsList.Add(new CreatedStatusResult());
        }

        public override MethodInfo Method =>
            typeof(ItemCategoryController).GetMethod(nameof(ItemCategoryController.CreateItemCategoryAsync))!;

        public override ItemCategoryController CreateSut()
        {
            return new ItemCategoryController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override async Task<IActionResult> ExecuteTestMethod(ItemCategoryController sut)
        {
            TestPropertyNotSetException.ThrowIfNull(_name);
            return await sut.CreateItemCategoryAsync(_name);
        }

        public override void SetupCommand()
        {
            TestPropertyNotSetException.ThrowIfNull(_name);
            Command = new CreateItemCategoryCommand(new ItemCategoryName(_name));
        }

        public override void SetupParameters()
        {
            _name = new DomainTestBuilder<string>().Create();
        }

        public override void SetupCommandConverter()
        {
            TestPropertyNotSetException.ThrowIfNull(_name);
            TestPropertyNotSetException.ThrowIfNull(Command);

            EndpointConvertersMock.SetupToDomain(_name, Command);
        }
    }
}