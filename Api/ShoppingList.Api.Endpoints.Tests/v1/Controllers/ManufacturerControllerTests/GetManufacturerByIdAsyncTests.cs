using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Manufacturers.Queries.ManufacturerById;
using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common.StatusResults;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.v1.Controllers.ManufacturerControllerTests;

public class GetManufacturerByIdAsyncTests : ControllerQueryTestsBase<ManufacturerController,
    ManufacturerByIdQuery, IManufacturer, ManufacturerContract,
    GetManufacturerByIdAsyncTests.GetManufacturerByIdAsyncFixture>
{
    public GetManufacturerByIdAsyncTests() : base(new GetManufacturerByIdAsyncFixture())
    {
    }

    [Theory]
    [InlineData(ErrorReasonCode.ManufacturerNotFound)]
    public async Task EndpointCall_WithDomainException_ShouldReturnNotFound(ErrorReasonCode errorCode)
    {
        // Arrange
        Fixture.SetupQuery();
        Fixture.SetupDomainException(errorCode);
        Fixture.SetupDomainExceptionInQueryDispatcher();
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

    public sealed class GetManufacturerByIdAsyncFixture : ControllerQueryFixtureBase
    {
        public GetManufacturerByIdAsyncFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NotFoundStatusResult());
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(new List<ErrorReasonCode>()
            {
                ErrorReasonCode.ManufacturerNotFound
            }));
        }

        private readonly Guid _itemCategoryId = Guid.NewGuid();

        public override MethodInfo Method =>
            typeof(ManufacturerController).GetMethod(nameof(ManufacturerController.GetManufacturerByIdAsync))!;

        public override ManufacturerController CreateSut()
        {
            return new ManufacturerController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override async Task<IActionResult> ExecuteTestMethod(ManufacturerController sut)
        {
            return await sut.GetManufacturerByIdAsync(_itemCategoryId);
        }

        public override void SetupParameters()
        {
        }

        public override void SetupQuery()
        {
            Query = new ManufacturerByIdQuery(new ManufacturerId(_itemCategoryId));
        }
    }
}