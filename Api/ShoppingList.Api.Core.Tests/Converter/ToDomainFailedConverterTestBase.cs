using AutoFixture;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;
using System;

namespace ProjectHermes.ShoppingList.Api.Core.Tests.Converter;

public abstract class ToDomainFailedConverterTestBase<TSource, TDest, TConverter, TException>
    where TConverter : IToDomainConverter<TSource, TDest>
    where TException : Exception
{
    protected abstract string ExpectedMessage { get; }

    [Fact]
    public void Convert_ShouldMapAllMembersCorrectly()
    {
        // Arrange
        var contract = CreateSource();
        Setup(contract);

        var sut = CreateSut();

        // Act
        var result = () => sut.ToDomain(contract);

        // Assert
        result.Should().Throw<TException>().WithMessage(ExpectedMessage);
    }

    public abstract TConverter CreateSut();

    protected virtual void Customize(IFixture fixture)
    {
    }

    protected virtual void Setup(TSource source)
    {
    }

    protected virtual TSource CreateSource()
    {
        var builder = new DomainTestBuilder<TSource>();
        Customize(builder);
        return builder.Create();
    }
}