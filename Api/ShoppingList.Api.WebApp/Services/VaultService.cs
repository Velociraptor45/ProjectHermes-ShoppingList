﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProjectHermes.ShoppingList.Api.Infrastructure;
using System.IO;
using System.Threading.Tasks;
using VaultSharp;
using VaultSharp.V1.AuthMethods.UserPass;

namespace ProjectHermes.ShoppingList.Api.WebApp.Services;

public class VaultService
{
    private readonly string _uri;
    private readonly string _connectionStringsPath;
    private VaultClient _client;
    private readonly string _mountPoint;
    private readonly string _password;
    private readonly string _username;

    public VaultService(IConfiguration configuration)
    {
        _uri = configuration["KeyVault:Uri"];
        _connectionStringsPath = configuration["KeyVault:Paths:ConnectionStrings"];
        _mountPoint = configuration["KeyVault:MountPoint"];
        _username = ReadFile(Environment.GetEnvironmentVariable("VAULT_USERNAME_FILE"));
        _password = ReadFile(Environment.GetEnvironmentVariable("VAULT_PASSWORD_FILE"));
    }

    private string ReadFile(string filePath)
    {
        if (!File.Exists(filePath))
            throw new InvalidOperationException($"File {filePath} does not exist");

        return File.ReadAllText(filePath);
    }

    private VaultClient GetClient()
    {
        if (_client is not null)
            return _client;

        var authMethod = new UserPassAuthMethodInfo(_username, _password);

        var clientSettings = new VaultClientSettings(_uri, authMethod);

        _client = new VaultClient(clientSettings);

        return _client;
    }

    private async Task RegisterConnectionStringsAsync(IServiceCollection services)
    {
        var client = _client ?? GetClient();

        var result = await client.V1.Secrets.KeyValue.V2.ReadSecretAsync<ConnectionStrings>(
            _connectionStringsPath,
            mountPoint: _mountPoint);

        var connectionStrings = result.Data.Data;

        services.AddSingleton(connectionStrings);
    }

    public async Task RegisterAsync(IServiceCollection services)
    {
        await RegisterConnectionStringsAsync(services);
    }
}