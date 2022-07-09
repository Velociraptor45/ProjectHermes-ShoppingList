﻿using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Services;
using System;
using System.IO;

namespace ProjectHermes.ShoppingList.Api.Endpoint.IntegrationTests;

public sealed class DockerFixture : IDisposable
{
    private readonly ICompositeService _container;

    public DockerFixture()
    {
        var fileDir = Path.Combine(Directory.GetCurrentDirectory(), "docker-compose.yml");
        _container = new Builder()
            .UseContainer()
            .UseCompose()
            .FromFile(fileDir)
            .RemoveOrphans()
            //.WaitForProcess("Database", "mariadb", millisTimeout: 30000)
            //.WaitForPort("Database", "15906/tcp", 30000)
            .Build()
            .Start();

        // wait for DB to initialize
        Task.Delay(10000).GetAwaiter().GetResult();
    }

    public const string ConnectionString =
        $"server=127.0.0.1;port=15906;database={DatabaseName};user id=root;pwd=123root;AllowUserVariables=true;UseAffectedRows=false";

    public const string DatabaseName = "test-shoppinglist";

    public void Dispose()
    {
        _container.Dispose();
    }
}