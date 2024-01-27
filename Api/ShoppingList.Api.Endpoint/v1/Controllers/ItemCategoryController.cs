using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Commands.CreateItemCategory;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Commands.DeleteItemCategory;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Commands.ModifyItemCategory;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Queries.AllActiveItemCategories;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Queries.ItemCategoryById;
using ProjectHermes.ShoppingList.Api.ApplicationServices.ItemCategories.Queries.ItemCategorySearch;
using ProjectHermes.ShoppingList.Api.Contracts.Common;
using ProjectHermes.ShoppingList.Api.Contracts.Common.Queries;
using ProjectHermes.ShoppingList.Api.Contracts.ItemCategories.Commands;
using ProjectHermes.ShoppingList.Api.Contracts.ItemCategories.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Services.Shared;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters;
using System.Threading;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;

[ApiController]
[Authorize(Policy = "User")]
[Route("v1/item-categories")]
public class ItemCategoryController : ControllerBase
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IEndpointConverters _converters;

    public ItemCategoryController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher,
        IEndpointConverters converters)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
        _converters = converters;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ItemCategoryContract), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetItemCategoryByIdAsync([FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new ItemCategoryByIdQuery(new ItemCategoryId(id));

            var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            var contract = _converters.ToContract<IItemCategory, ItemCategoryContract>(result);

            return Ok(contract);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);

            if (e.Reason.ErrorCode == ErrorReasonCode.ItemCategoryNotFound)
                return NotFound(errorContract);

            return UnprocessableEntity(errorContract);
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ItemCategorySearchResultContract>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [Route("")]
    public async Task<IActionResult> SearchItemCategoriesByNameAsync([FromQuery] string searchInput,
        [FromQuery] bool includeDeleted = false, CancellationToken cancellationToken = default)
    {
        searchInput = searchInput.Trim();
        if (string.IsNullOrEmpty(searchInput))
        {
            return BadRequest("Search input mustn't be null or empty");
        }

        var query = new ItemCategorySearchQuery(searchInput, includeDeleted);
        var itemCategoryReadModels = (await _queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

        if (!itemCategoryReadModels.Any())
            return NoContent();

        var itemCategoryContracts =
            _converters.ToContract<ItemCategorySearchResultReadModel, ItemCategorySearchResultContract>(
                itemCategoryReadModels);

        return Ok(itemCategoryContracts);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ItemCategoryContract>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Route("active")]
    public async Task<IActionResult> GetAllActiveItemCategoriesAsync(CancellationToken cancellationToken = default)
    {
        var query = new AllActiveItemCategoriesQuery();
        var readModels = (await _queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

        if (!readModels.Any())
            return NoContent();

        var contracts = _converters.ToContract<ItemCategoryReadModel, ItemCategoryContract>(readModels);

        return Ok(contracts);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("")]
    public async Task<IActionResult> ModifyItemCategoryAsync([FromBody] ModifyItemCategoryContract contract,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = _converters.ToDomain<ModifyItemCategoryContract, ModifyItemCategoryCommand>(contract);
            await _commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);

            if (e.Reason.ErrorCode == ErrorReasonCode.ItemCategoryNotFound)
                return NotFound(errorContract);

            return UnprocessableEntity(errorContract);
        }

        return NoContent();
    }

    [HttpPost]
    [ProducesResponseType(typeof(ItemCategoryContract), StatusCodes.Status201Created)]
    [Route("")]
    public async Task<IActionResult> CreateItemCategoryAsync([FromQuery] string name,
        CancellationToken cancellationToken = default)
    {
        var command = _converters.ToDomain<string, CreateItemCategoryCommand>(name);
        var model = await _commandDispatcher.DispatchAsync(command, cancellationToken);

        var contract = _converters.ToContract<IItemCategory, ItemCategoryContract>(model);

        return CreatedAtAction(nameof(GetItemCategoryByIdAsync), new { id = contract.Id }, contract);
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("{id}")]
    public async Task<IActionResult> DeleteItemCategoryAsync([FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = _converters.ToDomain<Guid, DeleteItemCategoryCommand>(id);
            await _commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);

            if (e.Reason.ErrorCode == ErrorReasonCode.ItemCategoryNotFound)
                return NotFound(errorContract);

            return UnprocessableEntity(errorContract);
        }

        return NoContent();
    }
}