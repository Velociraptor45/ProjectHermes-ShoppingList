using AutoFixture.Kernel;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests.Common.Reasons;

public class ReasonTests
{
    [Fact]
    public void ErrorCode_ShouldBeUniqueForEveryReason()
    {
        // Arrange
        var allReasonTypes = typeof(IReason).Assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i == typeof(IReason)))
        .ToList();

        var fixture = new Fixture();
        fixture.Customize(new DomainCustomization());
        var allReasons = allReasonTypes.Select(t => new SpecimenContext(fixture).Resolve(t))
            .Cast<IReason>()
            .ToList();

        var allReasonErrorCodeCombinations = allReasons.Select(r => new { TypeName = r.GetType().Name, r.ErrorCode })
            .ToList();

        // Act
        var duplicateReasons = allReasonErrorCodeCombinations.GroupBy(r => r.ErrorCode)
            .Where(g => g.Count() > 1)
            .ToList();

        // Assert
        duplicateReasons.Should().BeEmpty();
    }

    [Fact]
    public void ErrorCodes_ShouldAllBeUsed()
    {
        // Arrange

        var allReasonTypes = typeof(IReason).Assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i == typeof(IReason)))
            .ToList();

        var fixture = new Fixture();
        fixture.Customize(new DomainCustomization());
        var allUsedErrorCodes = allReasonTypes.Select(t => new SpecimenContext(fixture).Resolve(t))
            .Cast<IReason>()
            .Select(r => r.ErrorCode)
            .ToHashSet();

        var allErrorCodes = Enum.GetValues<ErrorReasonCode>().ToList();

        // Act
        var unusedErrorCodes = allErrorCodes.Where(c => !allUsedErrorCodes.Contains(c)).ToList();

        // Assert
        unusedErrorCodes.Should().BeEmpty();
    }

    [Fact]
    public void Messages_ShouldNotBeEmpty()
    {
        // Arrange
        var allReasonTypes = typeof(IReason).Assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i == typeof(IReason)))
        .ToList();

        var fixture = new Fixture();
        fixture.Customize(new DomainCustomization());
        var allReasons = allReasonTypes.Select(t => new SpecimenContext(fixture).Resolve(t))
            .Cast<IReason>()
            .ToList();

        // Act
        var reasonsWithEmptyMessages = allReasons.Where(r => string.IsNullOrWhiteSpace(r.Message)).ToList();

        // Assert
        reasonsWithEmptyMessages.Should().BeEmpty();
    }
}