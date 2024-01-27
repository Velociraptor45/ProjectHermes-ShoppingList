using AutoFixture.Kernel;
using ProjectHermes.ShoppingList.Api.Domain.TestKit.Shared;

namespace ProjectHermes.ShoppingList.Api.Domain.TestKit.ShoppingLists.SpecimenBuilders;

public class EnumSpecimenBuilder<TEnum> : ISpecimenBuilder
    where TEnum : Enum
{
    private readonly CommonFixture _commonFixture = new();

    public object Create(object request, ISpecimenContext context)
    {
        if (request is TEnum || (request is SeededRequest sr && (Type)sr.Request == typeof(TEnum)))
        {
            var values = ((TEnum[])Enum.GetValues(typeof(TEnum)))
                .ToList();

            return _commonFixture.ChooseRandom(values);
        }

        return new NoSpecimen();
    }
}