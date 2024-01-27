using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Manufacturers.Commands.CreateManufacturer;
using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.v1.Controllers.ManufacturerControllerTests;

public class CreateManufacturerAsyncTests : ControllerCommandWithReturnTypeTestsBase<ManufacturerController,
    CreateManufacturerCommand, IManufacturer, ManufacturerContract,
    CreateManufacturerAsyncTests.CreateManufacturerAsyncFixture>
{
    public CreateManufacturerAsyncTests() : base(new CreateManufacturerAsyncFixture())
    {
    }

    public sealed class CreateManufacturerAsyncFixture : ControllerCommandWithReturnTypeFixtureBase
    {
        private string? _name;

        public CreateManufacturerAsyncFixture()
        {
            PossibleResultsList.Add(new CreatedStatusResult());
        }

        public override MethodInfo Method =>
            typeof(ManufacturerController).GetMethod(nameof(ManufacturerController.CreateManufacturerAsync))!;

        public override ManufacturerController CreateSut()
        {
            return new ManufacturerController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override async Task<IActionResult> ExecuteTestMethod(ManufacturerController sut)
        {
            TestPropertyNotSetException.ThrowIfNull(_name);
            return await sut.CreateManufacturerAsync(_name);
        }

        public override void SetupCommand()
        {
            TestPropertyNotSetException.ThrowIfNull(_name);
            Command = new CreateManufacturerCommand(new ManufacturerName(_name));
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