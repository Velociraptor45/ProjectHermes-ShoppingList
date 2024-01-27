using AutoFixture;
using AutoMapper;
using FluentAssertions.Equivalency;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;

namespace ProjectHermes.ShoppingList.Api.Core.Tests.Converter;

public abstract class ToDomainConverterTestBase<TSource, TDest, TConverter> where TConverter : IToDomainConverter<TSource, TDest>
{
    [Fact]
    public void Convert_ShouldMapAllMembersCorrectly()
    {
        // Arrange
        var contract = CreateSource();
        Setup(contract);

        var mapper = new MapperConfiguration(AddMapping).CreateMapper();

        mapper.ConfigurationProvider.AssertConfigurationIsValid();

        var expectedResult = mapper.Map<TDest>(contract);
        OnAfterMapping(contract, expectedResult);
        var sut = CreateSut();

        // Act
        var result = sut.ToDomain(contract);

        // Assert
        result.Should().BeEquivalentTo(expectedResult, opt =>
        {
            CustomizeAssertionOptions(opt);
            return opt;
        });
    }

    public abstract TConverter CreateSut();

    protected virtual void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
    }

    protected abstract void AddMapping(IMappingExpression<TSource, TDest> mapping);

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

    public void AddMapping(IMapperConfigurationExpression cfg)
    {
        var mapping = cfg.CreateMap<TSource, TDest>(MemberList.Destination);
        AddMapping(mapping);
        AddAdditionalMapping(cfg);
    }

    protected virtual void OnAfterMapping(TSource src, TDest mapped)
    {
    }

    protected virtual void CustomizeAssertionOptions(EquivalencyAssertionOptions<TDest> opt)
    {
    }
}