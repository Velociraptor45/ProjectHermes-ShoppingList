using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Manufacturers.Commands.ModifyManufacturer;
using ProjectHermes.ShoppingList.Api.Contracts.Manufacturers.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.v1.Controllers.ManufacturerControllerTests;

public class ModifyManufacturerAsyncTests : ControllerCommandTestsBase<ManufacturerController,
    ModifyManufacturerCommand, bool, ModifyManufacturerAsyncTests.ModifyManufacturerAsyncFixture>
{
    public ModifyManufacturerAsyncTests() : base(new ModifyManufacturerAsyncFixture())
    {
    }

    [Theory]
    [InlineData(ErrorReasonCode.ManufacturerNotFound)]
    public async Task EndpointCall_WithDomainException_ShouldReturnNotFound(ErrorReasonCode errorCode)
    {
        // Arrange
        Fixture.SetupCommand();
        Fixture.SetupParameters();
        Fixture.SetupCommandConverter();
        Fixture.SetupDomainException(errorCode);
        Fixture.SetupDomainExceptionInCommandDispatcher();
        Fixture.SetupExpectedErrorContract();
        Fixture.SetupErrorConversion();
        var sut = Fixture.CreateSut();

        // Act
        var result = await Fixture.ExecuteTestMethod(sut);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();
        var unprocessableEntity = result as NotFoundObjectResult;
        unprocessableEntity!.Value.Should().BeEquivalentTo(Fixture.ExpectedErrorContract);
    }

    public sealed class ModifyManufacturerAsyncFixture : ControllerCommandFixtureBase
    {
        private ModifyManufacturerContract? _contract;

        public ModifyManufacturerAsyncFixture()
        {
            PossibleResultsList.Add(new NoContentStatusResult());
            PossibleResultsList.Add(new NotFoundStatusResult());
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(new List<ErrorReasonCode>()
            {
                ErrorReasonCode.ManufacturerNotFound
            }));
        }

        public override MethodInfo Method =>
            typeof(ManufacturerController).GetMethod(nameof(ManufacturerController.ModifyManufacturerAsync))!;

        public override ManufacturerController CreateSut()
        {
            return new ManufacturerController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override async Task<IActionResult> ExecuteTestMethod(ManufacturerController sut)
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);
            return await sut.ModifyManufacturerAsync(_contract);
        }

        public override void SetupCommand()
        {
            Command = new DomainTestBuilder<ModifyManufacturerCommand>().Create();
        }

        public override void SetupParameters()
        {
            _contract = new DomainTestBuilder<ModifyManufacturerContract>().Create();
        }

        public override void SetupCommandConverter()
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);
            TestPropertyNotSetException.ThrowIfNull(Command);

            EndpointConvertersMock.SetupToDomain(_contract, Command);
        }
    }
}