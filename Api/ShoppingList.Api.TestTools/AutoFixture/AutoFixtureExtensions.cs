﻿using AutoFixture;

namespace ShoppingList.Api.TestTools.AutoFixture;

public static class AutoFixtureExtensions
{
    public static IFixture ConstructorArgumentFor<TTargetType, TValueType>(this IFixture fixture,
        string paramName, TValueType value)
    {
        var item = fixture.Customizations.FirstOrDefault(c => c is ConstructorArgumentRelay<TTargetType, TValueType> arg
                                                              && arg.ParamName == paramName);
        if (item != null)
            fixture.Customizations.Remove(item);

        fixture.Customizations.Add(new ConstructorArgumentRelay<TTargetType, TValueType>(paramName, value));
        return fixture;
    }
}