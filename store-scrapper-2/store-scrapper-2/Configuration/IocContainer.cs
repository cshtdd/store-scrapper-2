using System;
using System.Reflection;
using Autofac;
using store_scrapper_2.DAL;

namespace store_scrapper_2.Configuration
{
  public static class IocContainer
  {
    private static bool _isInitialized;
    private static readonly IContainer Resolver = InitializeResolver();

    public static void Initialize()
    {
      if (!Resolver.IsRegistered<IIocValidationTest>())
      {
        throw new InvalidOperationException($"IocContainerValidation; reason=IsRegistered;");
      }
      
      var expectedValidator = ResolveInternal<IIocValidationTest>();
      if (expectedValidator == null)
      {
        throw new InvalidOperationException($"IocContainerValidation; reason=ExpectedValidator;");
      }

      if (expectedValidator.GetSampleValue() != IocValidationTest.ExpectedConstant)
      {
        throw new InvalidOperationException($"IocContainerValidation; reason=GetSampleValue;");
      }

      _isInitialized = true;
    }
    
    public static T Resolve<T>()
    {
      if (!_isInitialized)
      {
        throw new InvalidOperationException("Cannot invoke Resolve using unitialized container");
      }
      
      return ResolveInternal<T>();
    }

    private static T ResolveInternal<T>() => Resolver.Resolve<T>();

    private static IContainer InitializeResolver()
    {
      var builder = new ContainerBuilder();
      
      builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());
      builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
        .Where(_ => true)
        .AsImplementedInterfaces();

      builder.RegisterInstance(new StoreDataContextFactory("Server=localhost;Database=stores", SupportedDatabases.Postgres)).As<IStoreDataContextFactory>();
      
      return builder.Build();
    }

    private interface IIocValidationTest
    {
      int GetSampleValue();
    }
    
    // ReSharper disable once ClassNeverInstantiated.Local
    private class IocValidationTest : IIocValidationTest
    {
      public const int ExpectedConstant = 42;
      public int GetSampleValue() => ExpectedConstant;
    }
  }
}