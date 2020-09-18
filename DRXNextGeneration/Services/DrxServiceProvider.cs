using System;
using Windows.Storage;
using MessagePack.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DRXNextGeneration.Services
{
    internal static class DrxServiceProvider
    {
        private static IServiceProvider _serviceProvider;

        private static IServiceProvider BuildServiceProvider()
        {
            // Register the MessagePack resolvers
            // TODO: should this be here?
            CompositeResolver.RegisterAndSetAsDefault(
                CoreLibrary.Generated.Resolvers.CoreLibraryGeneratedResolver.Instance,
                DRXLibrary.Generated.Resolvers.DRXLibraryGeneratedResolver.Instance,
                BuiltinResolver.Instance,
                AttributeFormatterResolver.Instance,
                PrimitiveObjectResolver.Instance
            );

            var collection = new ServiceCollection();
            collection.AddLogging();
            collection.AddSingleton(typeof(StoreService));
            collection.AddSingleton(typeof(UserService));

            _serviceProvider = collection.BuildServiceProvider();
            var factory = _serviceProvider.GetService<ILoggerFactory>();

            factory.AddFile($"{ApplicationData.Current.LocalFolder.Path}\\logs\\app.log");
            App.RegisterCrashLogger(factory.CreateLogger("ExceptionHandler"));

            return _serviceProvider;
        }

        public static IServiceProvider GetServiceProvider() => _serviceProvider ?? BuildServiceProvider();
    }
}
