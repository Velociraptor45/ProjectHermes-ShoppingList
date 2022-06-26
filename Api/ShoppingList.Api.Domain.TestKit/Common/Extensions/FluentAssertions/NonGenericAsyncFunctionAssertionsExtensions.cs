﻿using FluentAssertions.Specialized;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;

namespace ShoppingList.Api.Domain.TestKit.Common.Extensions.FluentAssertions;

public static class NonGenericAsyncFunctionAssertionsExtensions
{
    public static async Task<ExceptionAssertions<DomainException>> ThrowDomainExceptionAsync(
        this NonGenericAsyncFunctionAssertions assertion, ErrorReasonCode code)
    {
        return (await assertion.ThrowAsync<DomainException>())
            .Where(e => e.Reason.ErrorCode == code);
    }

    public static async Task<ExceptionAssertions<DomainException>> ThrowDomainExceptionAsync<T>(
        this GenericAsyncFunctionAssertions<T> assertion, ErrorReasonCode code)
    {
        return (await assertion.ThrowAsync<DomainException>())
            .Where(e => e.Reason.ErrorCode == code);
    }
}