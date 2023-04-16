﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Recipes.Commands.CreateRecipe;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Recipes.Commands.ModifyRecipe;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Recipes.Queries.AllIngredientQuantityTypes;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Recipes.Queries.RecipeById;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Recipes.Queries.SearchByTagIds;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Recipes.Queries.SearchRecipesByName;
using ProjectHermes.ShoppingList.Api.Contracts.Common;
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Commands.CreateRecipe;
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Commands.ModifyRecipe;
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.AllIngredientQuantityTypes;
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.Get;
using ProjectHermes.ShoppingList.Api.Contracts.Recipes.Queries.SearchRecipesByName;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries.Quantities;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters;
using System.Threading;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;

[ApiController]
[Route("v1/recipes")]
public class RecipeController : ControllerBase
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IEndpointConverters _converters;

    public RecipeController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher,
        IEndpointConverters converters)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
        _converters = converters;
    }

    [HttpGet]
    [ProducesResponseType(typeof(RecipeContract), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = new RecipeByIdQuery(new RecipeId(id));
            var result = await _queryDispatcher.DispatchAsync(query, cancellationToken);

            var contract = _converters.ToContract<IRecipe, RecipeContract>(result);

            return Ok(contract);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            if (e.Reason.ErrorCode is ErrorReasonCode.RecipeNotFound)
                return NotFound(errorContract);

            return UnprocessableEntity(errorContract);
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RecipeSearchResultContract>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("search-by-name")]
    public async Task<IActionResult> SearchRecipesByNameAsync([FromQuery] string searchInput,
        CancellationToken cancellationToken = default)
    {
        searchInput = searchInput.Trim();
        if (string.IsNullOrEmpty(searchInput))
        {
            return BadRequest("Search input mustn't be null or empty");
        }

        try
        {
            var query = new SearchRecipesByNameQuery(searchInput);
            var results = (await _queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

            if (!results.Any())
                return NoContent();

            var contracts = _converters.ToContract<RecipeSearchResult, RecipeSearchResultContract>(results);

            return Ok(contracts);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            return UnprocessableEntity(errorContract);
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RecipeSearchResultContract>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Route("search-by-tags")]
    public async Task<IActionResult> SearchRecipesByTagsAsync([FromQuery] IEnumerable<Guid> tagIds,
        CancellationToken cancellationToken = default)
    {
        var query = new SearchRecipesByTagsQuery(tagIds.Select(t => new RecipeTagId(t)));

        var results = (await _queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();
        if (!results.Any())
            return NoContent();

        var contracts = _converters.ToContract<RecipeSearchResult, RecipeSearchResultContract>(results);
        return Ok(contracts);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<IngredientQuantityTypeContract>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Route("ingredient-quantity-types")]
    public async Task<IActionResult> GetAllIngredientQuantityTypes(CancellationToken cancellationToken = default)
    {
        var query = new AllIngredientQuantityTypesQuery();
        var results = (await _queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

        if (!results.Any())
            return NoContent();

        var contracts = _converters.ToContract<IngredientQuantityTypeReadModel, IngredientQuantityTypeContract>(results);

        return Ok(contracts);
    }

    [HttpPost]
    [ProducesResponseType(typeof(RecipeContract), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("")]
    public async Task<IActionResult> CreateRecipeAsync([FromBody] CreateRecipeContract createRecipeContract,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = _converters.ToDomain<CreateRecipeContract, CreateRecipeCommand>(createRecipeContract);

            var model = await _commandDispatcher.DispatchAsync(command, cancellationToken);

            var contract = _converters.ToContract<IRecipe, RecipeContract>(model);

            // ReSharper disable once Mvc.ActionNotResolved
            return CreatedAtAction(nameof(GetAsync), new { id = contract.Id }, contract);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            return UnprocessableEntity(errorContract);
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("{id:guid}/modify")]
    public async Task<IActionResult> ModifyRecipeAsync([FromRoute] Guid id, [FromBody] ModifyRecipeContract contract,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var command = _converters.ToDomain<(Guid, ModifyRecipeContract), ModifyRecipeCommand>((id, contract));

            await _commandDispatcher.DispatchAsync(command, cancellationToken);

            return Ok();
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            if (e.Reason.ErrorCode
                is ErrorReasonCode.RecipeNotFound
                or ErrorReasonCode.IngredientNotFound
                or ErrorReasonCode.PreparationStepNotFound)
                return NotFound(errorContract);

            return UnprocessableEntity(errorContract);
        }
    }
}