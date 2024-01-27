using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Commands.ModifyItemCategory;
using ProjectHermes.ShoppingList.Api.Contracts.ItemCategories.Commands;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.v1.Controllers.ItemCategoryControllerTests;

public class ModifyItemCategoryAsyncTests : ControllerCommandTestsBase<ItemCategoryController,
    ModifyItemCategoryCommand, bool, ModifyItemCategoryAsyncTests.ModifyItemCategoryAsyncFixture>
{
    public ModifyItemCategoryAsyncTests() : base(new ModifyItemCategoryAsyncFixture())
    {
    }

    [Theory]
    [InlineData(ErrorReasonCode.ItemCategoryNotFound)]
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

    public sealed class ModifyItemCategoryAsyncFixture : ControllerCommandFixtureBase
    {
        private ModifyItemCategoryContract? _contract;

        public ModifyItemCategoryAsyncFixture()
        {
            PossibleResultsList.Add(new NoContentStatusResult());
            PossibleResultsList.Add(new NotFoundStatusResult());
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(new List<ErrorReasonCode>()
            {
                ErrorReasonCode.ItemCategoryNotFound
            }));
        }

        public override MethodInfo Method =>
            typeof(ItemCategoryController).GetMethod(nameof(ItemCategoryController.ModifyItemCategoryAsync))!;

        public override ItemCategoryController CreateSut()
        {
            return new ItemCategoryController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override async Task<IActionResult> ExecuteTestMethod(ItemCategoryController sut)
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);
            return await sut.ModifyItemCategoryAsync(_contract);
        }

        public override void SetupCommand()
        {
            Command = new DomainTestBuilder<ModifyItemCategoryCommand>().Create();
        }

        public override void SetupParameters()
        {
            _contract = new DomainTestBuilder<ModifyItemCategoryContract>().Create();
        }

        public override void SetupCommandConverter()
        {
            TestPropertyNotSetException.ThrowIfNull(_contract);
            TestPropertyNotSetException.ThrowIfNull(Command);

            EndpointConvertersMock.SetupToDomain(_contract, Command);
        }
    }
}