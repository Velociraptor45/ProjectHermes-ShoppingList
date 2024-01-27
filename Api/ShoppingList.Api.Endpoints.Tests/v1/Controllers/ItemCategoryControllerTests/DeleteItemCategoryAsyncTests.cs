using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Commands.DeleteItemCategory;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common;
using ProjectHermes.ShoppingList.Api.Endpoints.Tests.Common.StatusResults;
using ProjectHermes.ShoppingList.Api.TestTools.Exceptions;
using System.Reflection;

namespace ProjectHermes.ShoppingList.Api.Endpoints.Tests.v1.Controllers.ItemCategoryControllerTests;

public class DeleteItemCategoryAsyncTests : ControllerCommandTestsBase<ItemCategoryController, DeleteItemCategoryCommand,
    bool, DeleteItemCategoryAsyncTests.DeleteItemCategoryAsyncFixture>
{
    public DeleteItemCategoryAsyncTests() : base(new DeleteItemCategoryAsyncFixture())
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

    public sealed class DeleteItemCategoryAsyncFixture : ControllerCommandFixtureBase
    {
        private Guid? _id;

        public DeleteItemCategoryAsyncFixture()
        {
            PossibleResultsList.Add(new UnprocessableEntityStatusResult(new List<ErrorReasonCode>
            {
                ErrorReasonCode.ItemNotFound
            }));
            PossibleResultsList.Add(new NotFoundStatusResult());
            PossibleResultsList.Add(new NoContentStatusResult());
        }

        public override MethodInfo Method =>
            typeof(ItemCategoryController).GetMethod(nameof(ItemCategoryController.DeleteItemCategoryAsync))!;

        public override ItemCategoryController CreateSut()
        {
            return new ItemCategoryController(
                QueryDispatcherMock.Object,
                CommandDispatcherMock.Object,
                EndpointConvertersMock.Object);
        }

        public override async Task<IActionResult> ExecuteTestMethod(ItemCategoryController sut)
        {
            TestPropertyNotSetException.ThrowIfNull(_id);
            return await sut.DeleteItemCategoryAsync(_id.Value);
        }

        public override void SetupParameters()
        {
            _id = new DomainTestBuilder<Guid>().Create();
        }

        public override void SetupCommand()
        {
            Command = new DomainTestBuilder<DeleteItemCategoryCommand>().Create();
        }

        public override void SetupCommandConverter()
        {
            TestPropertyNotSetException.ThrowIfNull(_id);
            TestPropertyNotSetException.ThrowIfNull(Command);
            EndpointConvertersMock.SetupToDomain(_id, Command);
        }
    }
}