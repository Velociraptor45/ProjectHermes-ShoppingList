﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Commands;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Common.Queries;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Commands;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Commands.CreateItemWithTypes;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Commands.DeleteItem;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Commands.ItemUpdateWithTypes;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Commands.ModifyItem;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Commands.ModifyItemWithTypes;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Commands.UpdateItem;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Queries.AllQuantityTypesInPacket;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Queries.ItemById;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Queries.SearchItems;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Queries.SearchItemsByFilters;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Queries.SearchItemsByItemCategory;
using ProjectHermes.ShoppingList.Api.ApplicationServices.Items.Queries.SearchItemsForShoppingLists;
using ProjectHermes.ShoppingList.Api.Contracts.Common;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.CreateItem;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.CreateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.MakeTemporaryItemPermanent;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.ModifyItem;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.ModifyItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.UpdateItem;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.UpdateItemPrice;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Commands.UpdateItemWithTypes;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.AllQuantityTypes;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.Get;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.SearchItemsByItemCategory;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.SearchItemsForShoppingLists;
using ProjectHermes.ShoppingList.Api.Contracts.Items.Queries.Shared;
using ProjectHermes.ShoppingList.Api.Domain.Common.Exceptions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Reasons;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Creations;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Queries.Quantities;
using ProjectHermes.ShoppingList.Api.Domain.Items.Services.Searches;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Endpoint.v1.Converters;
using System.Threading;

namespace ProjectHermes.ShoppingList.Api.Endpoint.v1.Controllers;

[ApiController]
[Route("v1/items")]
public class ItemController : ControllerBase
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IEndpointConverters _converters;

    public ItemController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher,
        IEndpointConverters converters)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
        _converters = converters;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ItemContract), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("{id:guid}")]
    public async Task<IActionResult> GetAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var query = new ItemByIdQuery(new ItemId(id));
        ItemReadModel result;
        try
        {
            result = await _queryDispatcher.DispatchAsync(query, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.ItemNotFound)
                return NotFound(errorContract);

            return UnprocessableEntity(errorContract);
        }

        var contract = _converters.ToContract<ItemReadModel, ItemContract>(result);

        return Ok(contract);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SearchItemResultContract>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Route("search")]
    public async Task<IActionResult> SearchItemsAsync([FromQuery] string searchInput,
        CancellationToken cancellationToken = default)
    {
        var query = new SearchItemQuery(searchInput);

        var readModels = (await _queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

        if (!readModels.Any())
            return NoContent();

        var contracts =
            _converters.ToContract<SearchItemResultReadModel, SearchItemResultContract>(readModels);

        return Ok(contracts);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SearchItemResultContract>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Route("filter")]
    public async Task<IActionResult> SearchItemsByFilterAsync([FromQuery] IEnumerable<Guid> storeIds,
        [FromQuery] IEnumerable<Guid> itemCategoryIds, [FromQuery] IEnumerable<Guid> manufacturerIds,
        CancellationToken cancellationToken = default)
    {
        var query = new SearchItemsByFilterQuery(
            storeIds.Select(id => new StoreId(id)),
            itemCategoryIds.Select(id => new ItemCategoryId(id)),
            manufacturerIds.Select(id => new ManufacturerId(id)));

        var readModels = (await _queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

        if (!readModels.Any())
            return NoContent();

        var contracts = _converters.ToContract<SearchItemResultReadModel, SearchItemResultContract>(readModels);

        return Ok(contracts);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SearchItemForShoppingListResultContract>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("search/{storeId:guid}")]
    public async Task<IActionResult> SearchItemsForShoppingListAsync([FromRoute] Guid storeId,
        [FromQuery] string searchInput, CancellationToken cancellationToken = default)
    {
        var query = new SearchItemsForShoppingListQuery(searchInput, new StoreId(storeId));

        List<SearchItemForShoppingResultReadModel> readModels;
        try
        {
            readModels = (await _queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.StoreNotFound)
                return NotFound(errorContract);

            return UnprocessableEntity(errorContract);
        }

        if (!readModels.Any())
            return NoContent();

        var contracts = _converters
            .ToContract<SearchItemForShoppingResultReadModel, SearchItemForShoppingListResultContract>(readModels)
            .ToList();

        return Ok(contracts);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<SearchItemByItemCategoryResultContract>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("search/by-item-category/{itemCategoryId:guid}")]
    public async Task<IActionResult> SearchItemsByItemCategoryAsync([FromRoute] Guid itemCategoryId,
        CancellationToken cancellationToken = default)
    {
        var query = new SearchItemsByItemCategoryQuery(new ItemCategoryId(itemCategoryId));

        List<SearchItemByItemCategoryResult> readModels;
        try
        {
            readModels = (await _queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            if (e.Reason.ErrorCode is ErrorReasonCode.ItemCategoryNotFound or ErrorReasonCode.StoresNotFound)
                return NotFound(errorContract);

            return UnprocessableEntity(errorContract);
        }

        if (!readModels.Any())
            return NoContent();

        var contracts =
            _converters.ToContract<SearchItemByItemCategoryResult, SearchItemByItemCategoryResultContract>(readModels);

        return Ok(contracts);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<QuantityTypeContract>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Route("quantity-types")]
    public async Task<IActionResult> GetAllQuantityTypesAsync(CancellationToken cancellationToken = default)
    {
        var query = new AllQuantityTypesQuery();
        var readModels = (await _queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

        if (!readModels.Any())
            return NoContent();

        var contracts = _converters.ToContract<QuantityTypeReadModel, QuantityTypeContract>(readModels);

        return Ok(contracts);
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<QuantityTypeInPacketContract>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [Route("quantity-types-in-packet")]
    public async Task<IActionResult> GetAllQuantityTypesInPacketAsync(CancellationToken cancellationToken = default)
    {
        var query = new AllQuantityTypesInPacketQuery();
        var readModels = (await _queryDispatcher.DispatchAsync(query, cancellationToken)).ToList();

        if (!readModels.Any())
            return NoContent();

        var contracts = _converters.ToContract<QuantityTypeInPacketReadModel, QuantityTypeInPacketContract>(readModels);

        return Ok(contracts);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ItemContract), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("without-types")]
    public async Task<IActionResult> CreateItemAsync([FromBody] CreateItemContract contract,
        CancellationToken cancellationToken = default)
    {
        if ((contract.QuantityInPacket is not null && contract.QuantityTypeInPacket is null)
            || (contract.QuantityInPacket is null && contract.QuantityTypeInPacket is not null))
        {
            return BadRequest(
                $"{nameof(contract.QuantityInPacket)} and {contract.QuantityTypeInPacket} must both be filled or both empty");
        }

        try
        {
            var model = _converters.ToDomain<CreateItemContract, ItemCreation>(contract);
            var command = new CreateItemCommand(model);

            var readModel = await _commandDispatcher.DispatchAsync(command, cancellationToken);

            var createdContract = _converters.ToContract<ItemReadModel, ItemContract>(readModel);

            // ReSharper disable once Mvc.ActionNotResolved
            return CreatedAtAction(nameof(GetAsync), new { id = createdContract.Id }, createdContract);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            return UnprocessableEntity(errorContract);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(ItemContract), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("with-types")]
    public async Task<IActionResult> CreateItemWithTypesAsync([FromBody] CreateItemWithTypesContract contract,
        CancellationToken cancellationToken = default)
    {
        if ((contract.QuantityInPacket is not null && contract.QuantityTypeInPacket is null)
           || (contract.QuantityInPacket is null && contract.QuantityTypeInPacket is not null))
        {
            return BadRequest(
                $"{nameof(contract.QuantityInPacket)} and {contract.QuantityTypeInPacket} must both be filled or both empty");
        }

        try
        {
            var model = _converters.ToDomain<CreateItemWithTypesContract, IItem>(contract);
            var command = new CreateItemWithTypesCommand(model);
            var readModel = await _commandDispatcher.DispatchAsync(command, cancellationToken);

            var returnContract = _converters.ToContract<ItemReadModel, ItemContract>(readModel);

            // ReSharper disable once Mvc.ActionNotResolved
            return CreatedAtAction(nameof(GetAsync), new { id = returnContract.Id }, returnContract);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            return UnprocessableEntity(errorContract);
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("with-types/{id:guid}/modify")]
    public async Task<IActionResult> ModifyItemWithTypesAsync([FromRoute] Guid id,
        [FromBody] ModifyItemWithTypesContract contract, CancellationToken cancellationToken = default)
    {
        if ((contract.QuantityInPacket is not null && contract.QuantityTypeInPacket is null)
            || (contract.QuantityInPacket is null && contract.QuantityTypeInPacket is not null))
        {
            return BadRequest(
                $"{nameof(contract.QuantityInPacket)} and {contract.QuantityTypeInPacket} must both be filled or both empty");
        }

        var command =
            _converters.ToDomain<(Guid, ModifyItemWithTypesContract), ModifyItemWithTypesCommand>((id, contract));

        try
        {
            await _commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.ItemNotFound)
                return NotFound(errorContract);

            return UnprocessableEntity(errorContract);
        }

        return NoContent();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("without-types/{id:guid}/modify")]
    public async Task<IActionResult> ModifyItemAsync([FromRoute] Guid id,
        [FromBody] ModifyItemContract contract, CancellationToken cancellationToken = default)
    {
        if ((contract.QuantityInPacket is not null && contract.QuantityTypeInPacket is null)
            || (contract.QuantityInPacket is null && contract.QuantityTypeInPacket is not null))
        {
            return BadRequest(
                $"{nameof(contract.QuantityInPacket)} and {contract.QuantityTypeInPacket} must both be filled or both empty");
        }

        var command = _converters.ToDomain<(Guid, ModifyItemContract), ModifyItemCommand>((id, contract));

        try
        {
            await _commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.ItemNotFound)
                return NotFound(errorContract);

            return UnprocessableEntity(errorContract);
        }

        return NoContent();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("without-types/{id:guid}/update")]
    public async Task<IActionResult> UpdateItemAsync([FromRoute] Guid id, [FromBody] UpdateItemContract contract,
        CancellationToken cancellationToken = default)
    {
        if ((contract.QuantityInPacket is not null && contract.QuantityTypeInPacket is null)
            || (contract.QuantityInPacket is null && contract.QuantityTypeInPacket is not null))
        {
            return BadRequest(
                $"{nameof(contract.QuantityInPacket)} and {contract.QuantityTypeInPacket} must both be filled or both empty");
        }

        var command = _converters.ToDomain<(Guid, UpdateItemContract), UpdateItemCommand>((id, contract));

        try
        {
            await _commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.ItemNotFound)
                return NotFound(errorContract);

            return UnprocessableEntity(errorContract);
        }

        return NoContent();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("{id:guid}/update-price")]
    public async Task<IActionResult> UpdateItemPriceAsync([FromRoute] Guid id, [FromBody] UpdateItemPriceContract contract,
        CancellationToken cancellationToken = default)
    {
        var command = _converters.ToDomain<(Guid, UpdateItemPriceContract), UpdateItemPriceCommand>((id, contract));

        try
        {
            await _commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            if (e.Reason.ErrorCode is ErrorReasonCode.ItemNotFound or ErrorReasonCode.ItemTypeNotFound)
                return NotFound(errorContract);

            return UnprocessableEntity(errorContract);
        }

        return NoContent();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("with-types/{id:guid}/update")]
    public async Task<IActionResult> UpdateItemWithTypesAsync([FromRoute] Guid id,
        [FromBody] UpdateItemWithTypesContract contract, CancellationToken cancellationToken = default)
    {
        if ((contract.QuantityInPacket is not null && contract.QuantityTypeInPacket is null)
            || (contract.QuantityInPacket is null && contract.QuantityTypeInPacket is not null))
        {
            return BadRequest(
                $"{nameof(contract.QuantityInPacket)} and {contract.QuantityTypeInPacket} must both be filled or both empty");
        }

        var command =
            _converters.ToDomain<(Guid, UpdateItemWithTypesContract), UpdateItemWithTypesCommand>((id, contract));

        try
        {
            await _commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            if (e.Reason.ErrorCode is ErrorReasonCode.ItemNotFound or ErrorReasonCode.StoreNotFound)
                return NotFound(errorContract);

            return UnprocessableEntity(errorContract);
        }

        return NoContent();
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("temporary/{id:guid}")]
    public async Task<IActionResult> MakeTemporaryItemPermanentAsync([FromRoute] Guid id,
        [FromBody] MakeTemporaryItemPermanentContract contract, CancellationToken cancellationToken = default)
    {
        if (contract.QuantityInPacket is not null && contract.QuantityTypeInPacket is null
            || contract.QuantityInPacket is null && contract.QuantityTypeInPacket is not null)
        {
            return BadRequest(
                $"{nameof(contract.QuantityInPacket)} and {contract.QuantityTypeInPacket} must both be filled or both empty");
        }

        var command =
            _converters.ToDomain<(Guid, MakeTemporaryItemPermanentContract), MakeTemporaryItemPermanentCommand>((id,
                contract));
        try
        {
            await _commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.ItemNotFound)
                return NotFound(errorContract);

            return UnprocessableEntity(errorContract);
        }

        return NoContent();
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorContract), StatusCodes.Status422UnprocessableEntity)]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteItemAsync([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var command = new DeleteItemCommand(new ItemId(id));
        try
        {
            await _commandDispatcher.DispatchAsync(command, cancellationToken);
        }
        catch (DomainException e)
        {
            var errorContract = _converters.ToContract<IReason, ErrorContract>(e.Reason);
            if (e.Reason.ErrorCode == ErrorReasonCode.ItemNotFound)
                return NotFound(errorContract);

            return UnprocessableEntity(errorContract);
        }

        return NoContent();
    }
}