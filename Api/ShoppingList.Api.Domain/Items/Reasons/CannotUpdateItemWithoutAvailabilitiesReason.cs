﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;

namespace ProjectHermes.ShoppingList.Api.Domain.Items.Reasons;

public class CannotUpdateItemWithoutAvailabilitiesReason : IReason
{
    public CannotUpdateItemWithoutAvailabilitiesReason()
    {
        Message = "Cannot update item without availabilities";
    }

    public string Message { get; }
    public ErrorReasonCode ErrorCode => ErrorReasonCode.CannotUpdateItemWithoutAvailabilities;
}