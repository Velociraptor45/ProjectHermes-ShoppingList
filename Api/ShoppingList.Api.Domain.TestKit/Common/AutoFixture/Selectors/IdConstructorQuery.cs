﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture.Kernel;
using ProjectHermes.ShoppingList.Api.Core.Extensions;

namespace ShoppingList.Api.Domain.TestKit.Common.AutoFixture.Selectors
{
    public class IdConstructorQuery : IMethodQuery
    {
        private readonly List<Type> _types;

        public IdConstructorQuery()
        {
            _types = new List<Type>
            {
                typeof(int),
                typeof(Guid),
            };
        }

        public IEnumerable<IMethod> SelectMethods(Type type)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            if (!type.Name.EndsWith("Id"))
                return Enumerable.Empty<IMethod>();

            var ctors = type.GetConstructors();
            var ctor = ctors.Single(ctor =>
                ctor.GetParameters().Length == 1 &&
                ctor.GetParameters().Any(p => _types.Contains(p.ParameterType)));

            return new ConstructorMethod(ctor).ToMonoList();
        }
    }
}