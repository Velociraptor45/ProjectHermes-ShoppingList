﻿using ProjectHermes.ShoppingList.Api.Repositories.Common.Transactions;

namespace ProjectHermes.ShoppingList.Api.Repositories.Tests.Common.Transactions;

public class TransactionGeneratorMock : Mock<ITransactionGenerator>
{
    public TransactionGeneratorMock(MockBehavior behavior) : base(behavior)
    {
    }

    public void SetupGenerateAsync(ITransaction returnValue)
    {
        Setup(i => i.GenerateAsync(
                It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(returnValue));
    }

    public void VerifyGenerateAsyncOnce()
    {
        Verify(i => i.GenerateAsync(
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}