﻿using ProjectHermes.ShoppingList.Frontend.Models.Items;
using System;

namespace ProjectHermes.ShoppingList.Frontend.Models.Shared.Requests
{
    public class ModifyItemRequest : IApiRequest
    {
        public ModifyItemRequest(Guid requestId, StoreItem storeItem)
        {
            RequestId = requestId;
            StoreItem = storeItem;
        }

        public Guid RequestId { get; }
        public StoreItem StoreItem { get; }
    }
}