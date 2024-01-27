using AutoFixture;
using AutoMapper;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Common;

namespace ProjectHermes.ShoppingList.Api.Core.Tests.Converter;

public abstract class ToContractConverterTestBase<TSource, TDest, TConverter> where TConverter : IToContractConverter<TSource, TDest>
{
    [Fact]
    public void Convert_ShouldMapAllMembersCorrectly()
    {
        // Arrange
        var contract = CreateSource();

        var mapper = new MapperConfiguration(AddMapping).CreateMapper();

        mapper.ConfigurationProvider.AssertConfigurationIsValid();

        var expectedResult = mapper.Map<TDest>(contract);
        var sut = CreateSut();

        // Act
        var result = sut.ToContract(contract);

        // Assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    protected abstract TConverter CreateSut();

    protected virtual void AddAdditionalMapping(IMapperConfigurationExpression cfg)
    {
    }

    protected abstract void AddMapping(IMappingExpression<TSource, TDest> mapping);

    protected virtual void Customize(IFixture fixture)
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
}