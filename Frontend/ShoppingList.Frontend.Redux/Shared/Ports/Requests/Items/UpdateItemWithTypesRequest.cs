﻿using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Shared.Ports.Requests.Items
{
    public class UpdateItemWithTypesRequest : IApiRequest
    {
        public UpdateItemWithTypesRequest(Guid requestId, EditedItem item)
        {
            RequestId = requestId;
            Item = item;
        }

        public Guid RequestId { get; }
        public EditedItem Item { get; }
    }
}