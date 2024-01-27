using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using Polly;
using ProjectHermes.ShoppingList.Api.Core.Converter;
using ProjectHermes.ShoppingList.Api.Core.DomainEventHandlers;
using ProjectHermes.ShoppingList.Api.Core.Extensions;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Models;
using ProjectHermes.ShoppingList.Api.Domain.ItemCategories.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Items.Models;
using ProjectHermes.ShoppingList.Api.Domain.Items.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Models;
using ProjectHermes.ShoppingList.Api.Domain.Manufacturers.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Models;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Recipes.Services.Queries;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Models;
using ProjectHermes.ShoppingList.Api.Domain.RecipeTags.Ports;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Models;
using ProjectHermes.ShoppingList.Api.Domain.ShoppingLists.Ports;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Models;
using ProjectHermes.ShoppingList.Api.Domain.Stores.Ports;
using ProjectHermes.ShoppingList.Api.Repositories.Common.Transactions;
using ProjectHermes.ShoppingList.Api.Repositories.ItemCategories.Adapters;
using ProjectHermes.ShoppingList.Api.Repositories.ItemCategories.Contexts;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Adapters;
using ProjectHermes.ShoppingList.Api.Repositories.Items.Contexts;
using ProjectHermes.ShoppingList.Api.Repositories.Manufacturers.Adapters;
using ProjectHermes.ShoppingList.Api.Repositories.Manufacturers.Contexts;
using ProjectHermes.ShoppingList.Api.Repositories.Recipes.Adapters;
using ProjectHermes.ShoppingList.Api.Repositories.Recipes.Contexts;
using ProjectHermes.ShoppingList.Api.Repositories.RecipeTags.Adapters;
using ProjectHermes.ShoppingList.Api.Repositories.RecipeTags.Contexts;
using ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Adapters;
using ProjectHermes.ShoppingList.Api.Repositories.ShoppingLists.Contexts;
using ProjectHermes.ShoppingList.Api.Repositories.Stores.Adapters;
using ProjectHermes.ShoppingList.Api.Repositories.Stores.Contexts;
using ProjectHermes.ShoppingList.Api.Vault.Configs;
using System.Data.Common;
using System.Reflection;
using Recipe = ProjectHermes.ShoppingList.Api.Repositories.Recipes.Entities.Recipe;
using RecipeTag = ProjectHermes.ShoppingList.Api.Repositories.RecipeTags.Entities.RecipeTag;

namespace ProjectHermes.ShoppingList.Api.Repositories;

public static class ServiceCollectionExtensions
{
    private static readonly MySqlServerVersion _sqlServerVersion = new(new Version(10, 3, 27));

    public static void AddRepositories(this IServiceCollection services, string? connectionString = null)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var connectionRetryPolicy = Policy.Handle<Exception>().WaitAndRetry(
                5,
                _ => TimeSpan.FromSeconds(5),
                (e, _, tryNo, _) => Console.WriteLine($"Failed to open DB connection (Try no. {tryNo}): {e}"));

        services.AddScoped<DbConnection>(provider =>
        {
            if (connectionString is null)
            {
                var cs = provider.GetRequiredService<ConnectionStrings>();
                connectionString = cs.ShoppingDatabase;
            }

            return connectionRetryPolicy.Execute(() =>
            {
                var connection = new MySqlConnection(connectionString);
                connection.Open();
                return connection;
            });
        });

        services.AddScoped<IList<DbContext>>(serviceProvider => GetAllDbContextInstances(serviceProvider).ToList());

        services.AddDbContext<ShoppingListContext>(SetDbConnection);
        services.AddDbContext<ItemCategoryContext>(SetDbConnection);
        services.AddDbContext<ManufacturerContext>(SetDbConnection);
        services.AddDbContext<ItemContext>(SetDbConnection);
        services.AddDbContext<StoreContext>(SetDbConnection);
        services.AddDbContext<RecipeContext>(SetDbConnection);
        services.AddDbContext<RecipeTagContext>(SetDbConnection);

        services.AddTransient<Func<CancellationToken, IShoppingListRepository>>(provider =>
        {
            return ct => new ShoppingListRepository(
                provider.GetRequiredService<ShoppingListContext>(),
                provider.GetRequiredService<IToDomainConverter<ShoppingLists.Entities.ShoppingList, IShoppingList>>(),
                provider.GetRequiredService<IToContractConverter<IShoppingList, ShoppingLists.Entities.ShoppingList>>(),
                provider.GetRequiredService<ILogger<ShoppingListRepository>>(),
                ct);
        });
        services.AddTransient<Func<CancellationToken, IItemRepository>>(provider =>
        {
            return ct => new ItemRepository(
                provider.GetRequiredService<ItemContext>(),
                provider.GetRequiredService<IToDomainConverter<Items.Entities.Item, IItem>>(),
                provider.GetRequiredService<IToContractConverter<IItem, Items.Entities.Item>>(),
                provider.GetRequiredService<IDomainEventDispatcher>(),
                provider.GetRequiredService<ILogger<ItemRepository>>(),
                ct);
        });
        services.AddTransient<Func<CancellationToken, IItemTypeReadRepository>>(provider =>
        {
            var dbContext = provider.GetRequiredService<ItemContext>();
            var converter = provider.GetRequiredService<IToDomainConverter<Items.Entities.ItemType, IItemType>>();
            return ct => new ItemTypeReadRepository(dbContext, converter, ct);
        });

        services.AddTransient<Func<CancellationToken, IItemCategoryRepository>>(provider =>
        {
            var context = provider.GetRequiredService<ItemCategoryContext>();
            var toDomainConverter = provider
                .GetRequiredService<IToDomainConverter<ItemCategories.Entities.ItemCategory, IItemCategory>>();
            var toEntityConverter = provider
                .GetRequiredService<IToContractConverter<IItemCategory, ItemCategories.Entities.ItemCategory>>();
            var dispatcher = provider.GetRequiredService<Func<CancellationToken, IDomainEventDispatcher>>();
            var logger = provider.GetRequiredService<ILogger<ItemCategoryRepository>>();
            return ct => new ItemCategoryRepository(context, toDomainConverter, toEntityConverter, dispatcher(ct), logger,
                ct);
        });
        services.AddTransient<Func<CancellationToken, IManufacturerRepository>>(provider =>
        {
            return ct => new ManufacturerRepository(
                provider.GetRequiredService<ManufacturerContext>(),
                provider.GetRequiredService<IToDomainConverter<Manufacturers.Entities.Manufacturer, IManufacturer>>(),
                provider.GetRequiredService<IToContractConverter<IManufacturer, Manufacturers.Entities.Manufacturer>>(),
                provider.GetRequiredService<ILogger<ManufacturerRepository>>(),
                ct);
        });
        services.AddTransient<Func<CancellationToken, IStoreRepository>>(provider =>
        {
            return ct => new StoreRepository(
                provider.GetRequiredService<StoreContext>(),
                provider.GetRequiredService<IToDomainConverter<Stores.Entities.Store, IStore>>(),
                provider.GetRequiredService<IToContractConverter<IStore, Stores.Entities.Store>>(),
                provider.GetRequiredService<Func<CancellationToken, IDomainEventDispatcher>>(),
                provider.GetRequiredService<ILogger<StoreRepository>>(),
                ct);
        });
        services.AddTransient<Func<CancellationToken, IRecipeRepository>>(provider =>
        {
            var context = provider.GetRequiredService<RecipeContext>();
            var searchResultToDomainConverter = provider.GetRequiredService<IToDomainConverter<Recipe, RecipeSearchResult>>();
            var toDomainConverter = provider.GetRequiredService<IToDomainConverter<Recipe, IRecipe>>();
            var toContractConverter = provider.GetRequiredService<IToContractConverter<IRecipe, Recipe>>();
            var logger = provider.GetRequiredService<ILogger<RecipeRepository>>();
            return cancellationToken =>
                new RecipeRepository(context, searchResultToDomainConverter, toDomainConverter, toContractConverter,
                    logger, cancellationToken);
        });
        services.AddTransient<Func<CancellationToken, IRecipeTagRepository>>(provider =>
        {
            var context = provider.GetRequiredService<RecipeTagContext>();
            var toDomainConverter = provider.GetRequiredService<IToDomainConverter<RecipeTag, IRecipeTag>>();
            var toContractConverter = provider.GetRequiredService<IToContractConverter<IRecipeTag, RecipeTag>>();
            var logger = provider.GetRequiredService<ILogger<RecipeTagRepository>>();
            return cancellationToken =>
                new RecipeTagRepository(context, toDomainConverter, toContractConverter, logger, cancellationToken);
        });
        services.AddScoped(_ => new SemaphoreSlim(1, 1));
        services.AddScoped<ITransactionGenerator, TransactionGenerator>();

        services.AddImplementationOfGenericType(assembly, typeof(IToContractConverter<,>));
        services.AddImplementationOfGenericType(assembly, typeof(IToDomainConverter<,>));
    }

    private static void SetDbConnection(IServiceProvider serviceProvider, DbContextOptionsBuilder options)
    {
        var connection = serviceProvider.GetService<DbConnection>()!;
        options.UseMySql(connection, _sqlServerVersion);
    }

    private static IEnumerable<DbContext> GetAllDbContextInstances(IServiceProvider serviceProvider)
    {
        var types = GetAllDbContextTypes();
        var instances = types.Select(serviceProvider.GetRequiredService);
        foreach (var instance in instances)
        {
            yield return (DbContext)instance;
        }
    }

    private static IEnumerable<Type> GetAllDbContextTypes()
    {
        // The order of the types is important, because the migrations are applied in the same order
        // and some of them depend on others
        yield return typeof(ManufacturerContext);
        yield return typeof(ItemCategoryContext);
        yield return typeof(StoreContext);
        yield return typeof(ItemContext);
        yield return typeof(ShoppingListContext);
        yield return typeof(RecipeTagContext);
        yield return typeof(RecipeContext);
    }
}