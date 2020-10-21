﻿using Microsoft.AspNetCore.Mvc;
using ShoppingList.ApplicationServices;
using ShoppingList.Domain.Models;
using ShoppingList.Domain.Queries.ActiveShoppingListByStoreId;
using ShoppingList.Domain.Queries.SharedModels;
using ShoppingList.Endpoint.Converters;
using System;
using System.Threading.Tasks;

namespace ShoppingList.Endpoint.V1.Controllers
{
    [ApiController]
    [Route("v1/shopping-list")]
    public class ShoppingListController : ControllerBase
    {
        private readonly IQueryDispatcher queryDispatcher;

        public ShoppingListController(IQueryDispatcher queryDispatcher)
        {
            this.queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [Route("active-shopping-list/{storeId}")]
        public async Task<IActionResult> GetActiveShoppingListByStoreId([FromRoute(Name = "storeId")] int storeId)
        {
            var query = new ActiveShoppingListByStoreIdQuery(new StoreId(storeId));
            ShoppingListReadModel readModel;
            try
            {
                readModel = await queryDispatcher.DispatchAsync(query, default);
            }
            catch (ArgumentException e)
            {
                return BadRequest(e.Message);
            }

            var contract = readModel.ToContract();

            return Ok(contract);
        }
    }
}