﻿using Moq;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Models;
using ProjectHermes.ShoppingList.Api.Domain.StoreItems.Services;
using ShoppingList.Api.Core.TestKit.Extensions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingList.Api.Domain.TestKit.StoreItems.Services
{
    public class AvailabilityValidationServiceMock : Mock<IAvailabilityValidationService>
    {
        public AvailabilityValidationServiceMock(MockBehavior behavior) : base(behavior)
        {
        }

        public void VerifyValidateOnce(IEnumerable<IStoreItemAvailability> availabilities)
        {
            Verify(i => i.ValidateAsync(
                    availabilities,
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }

        public void SetupValidateAsync(IEnumerable<IStoreItemAvailability> availabilities)
        {
            Setup(m => m.ValidateAsync(
                    It.Is<IEnumerable<IStoreItemAvailability>>(avs => avs.IsEquivalentTo(availabilities)),
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        }
    }
}