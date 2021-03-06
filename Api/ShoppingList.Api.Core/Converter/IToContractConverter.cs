﻿using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Core.Converter
{
    public interface IToContractConverter<in TSource, out TDestination>
    {
        TDestination ToContract(TSource source);

        IEnumerable<TDestination> ToContract(IEnumerable<TSource> sources)
        {
            var sourcesList = sources.ToList();

            foreach (var source in sourcesList)
            {
                yield return ToContract(source);
            }
        }
    }
}