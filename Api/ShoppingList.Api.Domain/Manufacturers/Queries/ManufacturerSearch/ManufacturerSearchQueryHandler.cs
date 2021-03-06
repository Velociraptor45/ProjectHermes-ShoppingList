﻿using ProjectHermes.ShoppingList.Api.Domain.Common.Models.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.Common.Queries;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Queries.SharedModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Queries.ManufacturerSearch
{
    public class ManufacturerSearchQueryHandler
        : IQueryHandler<ManufacturerSearchQuery, IEnumerable<ManufacturerReadModel>>
    {
        private readonly IManufacturerRepository manufacturerRepository;

        public ManufacturerSearchQueryHandler(IManufacturerRepository manufacturerRepository)
        {
            this.manufacturerRepository = manufacturerRepository;
        }

        public async Task<IEnumerable<ManufacturerReadModel>> HandleAsync(ManufacturerSearchQuery query, CancellationToken cancellationToken)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            var manufacturerModels = await manufacturerRepository.FindByAsync(query.SearchInput,
                cancellationToken);

            cancellationToken.ThrowIfCancellationRequested();

            return manufacturerModels.Select(model => model.ToReadModel());
        }
    }
}