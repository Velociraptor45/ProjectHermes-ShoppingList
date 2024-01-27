using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Manufacturers.Commands.DeleteManufacturer;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.v1.Controllers.ManufacturerControllerTests;

public class DeleteManufacturerAsyncTests : ControllerCommandTestsBase<ManufacturerController, DeleteManufacturerCommand,
    bool, DeleteManufacturerAsyncTests.DeleteManufacturerAsyncFixture>
{
    public DeleteManufacturerAsyncTests() : base(new DeleteManufacturerAsyncFixture())
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

    public sealed class DeleteManufacturerAsyncFixture : ControllerCommandFixtureBase
    {
        private Guid? _id;

        public DeleteManufacturerAsyncFixture()
        {
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(new List<ErrorReasonCode>
            {
                ErrorReasonCode.ItemNotFound
            }));
            PossibleResultsList.Add(new NotFoundStatusResult());
            PossibleResultsList.Add(new NoContentStatusResult());
        }

        public override MethodInfo Method =>
            typeof(ManufacturerController).GetMethod(nameof(ManufacturerController.DeleteManufacturerAsync))!;

        public override ManufacturerController CreateSut()
        {
            return new ManufacturerController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override async Task<IActionResult> ExecuteTestMethod(ManufacturerController sut)
        {
            TestPropertyNotSetException.ThrowIfNull(_id);
            return await sut.DeleteManufacturerAsync(_id.Value);
        }

        public override void SetupParameters()
        {
            _id = new DomainTestBuilder<Guid>().Create();
        }

        public override void SetupCommand()
        {
            Command = new DomainTestBuilder<DeleteManufacturerCommand>().Create();
        }

        public override void SetupCommandConverter()
        {
            TestPropertyNotSetException.ThrowIfNull(_id);
            TestPropertyNotSetException.ThrowIfNull(Command);
            EndpointConvertersMock.SetupToDomain(_id, Command);
        }
    }
}