using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Manufacturers.Queries.AllActiveManufacturers;
using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Services.Shared;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common.StatusResults;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.v1.Controllers.ManufacturerControllerTests;

public class GetAllActiveManufacturersAsyncTests : ControllerEnumerableQueryTestsBase<ManufacturerController,
    AllActiveManufacturersQuery, ManufacturerReadModel, ManufacturerContract,
    GetAllActiveManufacturersAsyncTests.GetAllActiveManufacturersAsyncFixture>
{
    public GetAllActiveManufacturersAsyncTests() : base(new GetAllActiveManufacturersAsyncFixture())
    {
    }

    public sealed class GetAllActiveManufacturersAsyncFixture : ControllerEnumerableQueryFixtureBase
    {
        public GetAllActiveManufacturersAsyncFixture()
        {
            PossibleResultsList.Add(new OkStatusResult());
            PossibleResultsList.Add(new NoContentStatusResult());
        }

        public override MethodInfo Method =>
            typeof(ManufacturerController).GetMethod(nameof(ManufacturerController.GetAllActiveManufacturersAsync))!;

        public override ManufacturerController CreateSut()
        {
            return new ManufacturerController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override async Task<IActionResult> ExecuteTestMethod(ManufacturerController sut)
        {
            return await sut.GetAllActiveManufacturersAsync();
        }

        public override void SetupParameters()
        {
        }

        public override void SetupQuery()
        {
            Query = new AllActiveManufacturersQuery();
        }
    }
}