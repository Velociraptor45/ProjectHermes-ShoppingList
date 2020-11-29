﻿using AutoFixture;
using AutoFixture.AutoMoq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectHermes.ShoppingList.Api.Domain.Tests
{
    public class CommonFixture
    {
        private readonly Random random = new Random();

        public Fixture GetNewFixture()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization { ConfigureMembers = true });
            return fixture;
        }

        public IEnumerable<int> NextUniqueInts(int amount, IEnumerable<int> exclude = null)
        {
            if (amount < 0)
                throw new ArgumentException($"{nameof(amount)} mustn't be negative.");

            List<int> numbers = new List<int>();
            List<int> excludedNumbers = exclude?.ToList() ?? new List<int>();

            for (int i = 0; i < amount; i++)
            {
                int number;
                do
                {
                    number = NextInt();
                } while (numbers.Contains(number) || excludedNumbers.Contains(number));
                numbers.Add(number);
            }
            return numbers;
        }

        public int NextInt(int minValue, int maxValue)
        {
            return random.Next(minValue, maxValue);
        }

        public int NextInt()
        {
            return NextInt(1, int.MaxValue);
        }

        public int NextInt(IEnumerable<int> exclude)
        {
            List<int> excludedInts = exclude.ToList();
            while (true)
            {
                var number = NextInt();
                if (!excludedInts.Contains(number))
                    return number;
            }
        }

        public int NextInt(int exclude)
        {
            return NextInt(new List<int> { exclude });
        }

        public bool NextBool()
        {
            return random.NextDouble() < .5f;
        }

        public float NextFloat()
        {
            return (float)random.NextDouble();
        }

        public DateTime NextDate()
        {
            var fixture = GetNewFixture();
            return fixture.Create<DateTime>();
        }
    }
}