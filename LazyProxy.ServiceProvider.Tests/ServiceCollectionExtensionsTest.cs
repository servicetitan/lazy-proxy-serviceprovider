[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("LazyProxy.DynamicTypes")]

namespace LazyProxy.ServiceProvider.Tests
{
    using System;
    using Xunit;
    using Microsoft.Extensions.DependencyInjection;
    using LazyProxy.ServiceProvider;

    public class ServiceCollectionExtensionsTest
    {

        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        [InlineData(ServiceLifetime.Transient)]
        [Theory]
        public void ServiceCtorMustBeExecutedAfterMethodIsCalledForTheFirstTime(ServiceLifetime lifetime)
        {
            _service1Id = string.Empty;
            _service2Id = string.Empty;

            var serviceCollection = new ServiceCollection();
            switch (lifetime)
            {
                case ServiceLifetime.Scoped:
                    serviceCollection.AddLazyScoped<IService1, Service1>();
                    serviceCollection.AddScoped<IService2, Service2>();
                    break;
                case ServiceLifetime.Singleton:
                    serviceCollection.AddLazySingleton<IService1, Service1>();
                    serviceCollection.AddSingleton<IService2, Service2>();
                    break;
                case ServiceLifetime.Transient:
                    serviceCollection.AddLazyTransient<IService1, Service1>();
                    serviceCollection.AddTransient<IService2, Service2>();
                    break;
            }
            
            using (var serviceProvider = serviceCollection.BuildServiceProvider())
            {
                var service = serviceProvider.GetService<IService1>();

                Assert.Empty(_service1Id);
                Assert.Empty(_service2Id);

                var result1 = service.Method();

                Assert.Equal(Service1MethodValue, result1);
                Assert.NotEmpty(_service1Id);
                Assert.NotEmpty(_service2Id);

                var prevService1Id = _service1Id;
                var prevService2Id = _service2Id;

                var result2 = service.Method();

                Assert.Equal(Service1MethodValue, result2);
                Assert.Equal(prevService1Id, _service1Id);
                Assert.Equal(prevService2Id, _service2Id);
            }
        }

        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        [InlineData(ServiceLifetime.Transient)]
        [Theory]
        public void ServiceCtorMustBeExecutedAfterPropertyGetterIsCalledForTheFirstTime(ServiceLifetime lifetime)
        {
            _service1Id = string.Empty;
            _service2Id = string.Empty;

            var serviceCollection = new ServiceCollection();
            switch (lifetime)
            {
                case ServiceLifetime.Scoped:
                    serviceCollection.AddLazyScoped<IService1, Service1>();
                    serviceCollection.AddScoped<IService2, Service2>();
                    break;
                case ServiceLifetime.Singleton:
                    serviceCollection.AddLazySingleton<IService1, Service1>();
                    serviceCollection.AddSingleton<IService2, Service2>();
                    break;
                case ServiceLifetime.Transient:
                    serviceCollection.AddLazyTransient<IService1, Service1>();
                    serviceCollection.AddTransient<IService2, Service2>();
                    break;
            }

            using (var serviceProvider = serviceCollection.BuildServiceProvider())
            {
                var service = serviceProvider.GetService<IService1>();

                Assert.Empty(_service1Id);
                Assert.Empty(_service2Id);

                var result1 = service.Property;

                Assert.Equal(Service1PropertyValue, result1);
                Assert.NotEmpty(_service1Id);
                Assert.NotEmpty(_service2Id);

                var prevService1Id = _service1Id;
                var prevService2Id = _service2Id;

                var result2 = service.Property;

                Assert.Equal(Service1PropertyValue, result2);
                Assert.Equal(prevService1Id, _service1Id);
                Assert.Equal(prevService2Id, _service2Id);
            }
        }

        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        [InlineData(ServiceLifetime.Transient)]
        [Theory]
        public void ServiceCtorMustBeExecutedAfterPropertySetterIsCalledForTheFirstTime(ServiceLifetime lifetime)
        {
            _service1Id = string.Empty;
            _service2Id = string.Empty;

            var serviceCollection = new ServiceCollection();
            switch (lifetime)
            {
                case ServiceLifetime.Scoped:
                    serviceCollection.AddLazyScoped<IService1, Service1>();
                    serviceCollection.AddScoped<IService2, Service2>();
                    break;
                case ServiceLifetime.Singleton:
                    serviceCollection.AddLazySingleton<IService1, Service1>();
                    serviceCollection.AddSingleton<IService2, Service2>();
                    break;
                case ServiceLifetime.Transient:
                    serviceCollection.AddLazyTransient<IService1, Service1>();
                    serviceCollection.AddTransient<IService2, Service2>();
                    break;
            }

            using (var serviceProvider = serviceCollection.BuildServiceProvider())
            {
                var service = serviceProvider.GetService<IService1>();

                Assert.Empty(_service1Id);
                Assert.Empty(_service2Id);

                service.Property = "newValue1";

                Assert.NotEmpty(_service1Id);
                Assert.NotEmpty(_service2Id);

                var prevService1Id = _service1Id;
                var prevService2Id = _service2Id;

                service.Property = "newValue2";

                Assert.Equal(prevService1Id, _service1Id);
                Assert.Equal(prevService2Id, _service2Id);
            }
        }

        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        [InlineData(ServiceLifetime.Transient)]
        [Theory]
        public void ArgumentsMustBePassedToMethodCorrectly(ServiceLifetime lifetime)
        {
            const string arg1 = nameof(arg1);
            const string arg2 = nameof(arg2);

            var serviceCollection = new ServiceCollection();
            switch (lifetime)
            {
                case ServiceLifetime.Scoped:
                    serviceCollection.AddLazyScoped<IService1, Service1>();
                    serviceCollection.AddScoped<IService2, Service2>();
                    break;
                case ServiceLifetime.Singleton:
                    serviceCollection.AddLazySingleton<IService1, Service1>();
                    serviceCollection.AddSingleton<IService2, Service2>();
                    break;
                case ServiceLifetime.Transient:
                    serviceCollection.AddLazyTransient<IService1, Service1>();
                    serviceCollection.AddTransient<IService2, Service2>();
                    break;
            }

            using (var serviceProvider = serviceCollection.BuildServiceProvider())
            {
                var service = serviceProvider.GetService<IService1>();

                var result1 = service.Method(arg: arg1);
                Assert.Equal($"{Service1MethodValue}{arg1}", result1);

                var result2 = service.Method(s => s.Method(arg2), arg1);
                Assert.Equal($"{Service1MethodValue}{Service2MethodValue}{arg2}{arg1}", result2);
            }
        }

        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        [InlineData(ServiceLifetime.Transient)]
        [Theory]
        public void DependencyResolutionExceptionMustBeThrownAfterMethodIsCalledWhenDependentServiceIsNotRegistered(ServiceLifetime lifetime)
        {
            var serviceCollection = new ServiceCollection();
            switch (lifetime)
            {
                case ServiceLifetime.Scoped:
                    serviceCollection.AddLazyScoped<IService1, Service1>();
                    break;
                case ServiceLifetime.Singleton:
                    serviceCollection.AddLazySingleton<IService1, Service1>();
                    break;
                case ServiceLifetime.Transient:
                    serviceCollection.AddLazyTransient<IService1, Service1>();
                    break;
            }

            using (var serviceProvider = serviceCollection.BuildServiceProvider())
            {
                var service = serviceProvider.GetService<IService1>();

                Assert.Throws<InvalidOperationException>(() => service.Method());
            }
        }

        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        [InlineData(ServiceLifetime.Transient)]
        [Theory]
        public void InternalServiceMustBeResolvedFromAssemblyMarkedAsVisibleForProxy(ServiceLifetime lifetime)
        {
            var serviceCollection = new ServiceCollection();
            switch (lifetime)
            {
                case ServiceLifetime.Scoped:
                    serviceCollection.AddLazyScoped<IInternalService, InternalService>();
                    break;
                case ServiceLifetime.Singleton:
                    serviceCollection.AddLazySingleton<IInternalService, InternalService>();
                    break;
                case ServiceLifetime.Transient:
                    serviceCollection.AddLazyTransient<IInternalService, InternalService>();
                    break;
            }

            using (var serviceProvider = serviceCollection.BuildServiceProvider())
            {
                var service = serviceProvider.GetService<IInternalService>();
                var result = service.Method();

                Assert.Equal(InternalServiceMethodValue, result);
            }
        }

        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        [InlineData(ServiceLifetime.Transient)]
        [Theory]
        public void ClosedGenericServiceMustBeResolved(ServiceLifetime lifetime)
        {
            var serviceCollection = new ServiceCollection();
            switch (lifetime)
            {
                case ServiceLifetime.Scoped:
                    serviceCollection.AddLazyScoped(
                typeof(IGenericService<ParameterType1, ParameterType2, ParameterType3>),
                typeof(GenericService<ParameterType1, ParameterType2, ParameterType3>));
                    break;
                case ServiceLifetime.Singleton:
                    serviceCollection.AddLazySingleton(
                typeof(IGenericService<ParameterType1, ParameterType2, ParameterType3>),
                typeof(GenericService<ParameterType1, ParameterType2, ParameterType3>));
                    break;
                case ServiceLifetime.Transient:
                    serviceCollection.AddLazyTransient(
                typeof(IGenericService<ParameterType1, ParameterType2, ParameterType3>),
                typeof(GenericService<ParameterType1, ParameterType2, ParameterType3>));
                    break;
            }

            serviceCollection.AddLazyScoped(
                typeof(IGenericService<ParameterType1, ParameterType2, ParameterType3>),
                typeof(GenericService<ParameterType1, ParameterType2, ParameterType3>));

            using (var serviceProvider = serviceCollection.BuildServiceProvider())
            {
                var service = serviceProvider.GetService<IGenericService<ParameterType1, ParameterType2, ParameterType3>>();

                var result = service.Get(new ParameterType1(), new ParameterType2(), 42);

                Assert.Equal(
                    $"{typeof(ParameterType1).Name}_" +
                    $"{typeof(ParameterType2).Name}_" +
                    $"{typeof(int).Name}",
                    result.Value);
            }
        }


        #region Private members

        [ThreadStatic] private static string _service1Id;

        [ThreadStatic] private static string _service2Id;

        private const string Service1PropertyValue = nameof(Service1PropertyValue);
        private const string Service1MethodValue = nameof(Service1MethodValue);
        private const string Service2MethodValue = nameof(Service2MethodValue);
        private const string Service2ExMethodValue = nameof(Service2ExMethodValue);
        private const string InternalServiceMethodValue = nameof(InternalServiceMethodValue);

        #endregion

        #region Test classes

        public interface IHasId
        {
            Guid Id { get; }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public interface IService : IHasId
        {
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public class Service : IService
        {
            public Guid Id { get; } = Guid.NewGuid();
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public interface IService1
        {
            string Property { get; set; }
            string Method(Func<IService2, string> getDependentServiceValue = null, string arg = null);
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class Service1 : IService1
        {
            private readonly IService2 _otherService;

            public Service1(IService2 otherService)
            {
                _service1Id = Guid.NewGuid().ToString();
                _otherService = otherService;
            }

            public string Property { get; set; } = Service1PropertyValue;

            public string Method(Func<IService2, string> getDependentServiceValue, string arg) =>
                $"{Service1MethodValue}" +
                $"{(getDependentServiceValue == null ? string.Empty : getDependentServiceValue(_otherService))}" +
                $"{arg ?? string.Empty}";
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public interface IService2
        {
            string Method(string arg = null);
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class Service2 : IService2
        {
            public Service2()
            {
                _service2Id = Guid.NewGuid().ToString();
            }

            public string Method(string arg) => $"{Service2MethodValue}{arg ?? string.Empty}";
        }

        private class OtherService2 : IService2
        {
            public string Method(string arg) => $"{Service2ExMethodValue}{arg ?? string.Empty}";
        }

        // ReSharper disable once MemberCanBePrivate.Global
        internal interface IInternalService
        {
            string Method(string arg = null);
        }

        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once ClassNeverInstantiated.Global
        internal class InternalService : IInternalService
        {
            public string Method(string arg) => $"{InternalServiceMethodValue}{arg ?? string.Empty}";
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public interface IServiceToTestOverrides
        {
            string Method(string arg);
        }

        // ReSharper disable once ClassNeverInstantiated.Local
        private class ServiceToTestOverrides : IServiceToTestOverrides
        {
            // ReSharper disable once MemberCanBePrivate.Global
            public class Argument
            {
                public string Value { get; }

                public Argument(string value)
                {
                    Value = value;
                }
            }

            public enum SomeEnum
            {
                // ReSharper disable once UnusedMember.Local
                Value1,
                Value2
            }

            private readonly SomeEnum _someEnum;
            private readonly IService2 _service;
            private readonly Argument _argument;

            public string Method(string arg) => $"{_someEnum}_{_service.Method(arg)}_{_argument.Value}";

            public ServiceToTestOverrides(SomeEnum someEnum, IService2 service, Argument argument)
            {
                _someEnum = someEnum;
                _service = service;
                _argument = argument;
            }
        }

        public interface IParameterType
        {
        }

        public abstract class ParameterTypeBase
        {
            public string Value { get; set; }
        }

        public class ParameterType1 : IParameterType
        {
        }

        public struct ParameterType2
        {
        }

        public class ParameterType3 : ParameterTypeBase, IParameterType
        {
        }

        public interface IGenericService<T, in TIn, out TOut> : IHasId
            where T : class, IParameterType, new()
            where TIn : struct
            where TOut : ParameterTypeBase, IParameterType
        {
            TOut Get<TArg>(T arg1, TIn arg2, TArg arg3) where TArg : struct;
        }

        private class GenericService<T, TIn, TOut> : IGenericService<T, TIn, TOut>
            where T : class, IParameterType, new()
            where TIn : struct
            where TOut : ParameterTypeBase, IParameterType, new()
        {
            protected virtual string Get() => "";
            public Guid Id { get; }

            public TOut Get<TArg>(T arg1, TIn arg2, TArg arg3) where TArg : struct
            {
                return new TOut
                {
                    Value = $"{arg1.GetType().Name}_{arg2.GetType().Name}_{arg3.GetType().Name}{Get()}"
                };
            }

            public GenericService()
            {
                Id = Guid.NewGuid();
            }
        }

        #endregion
    }
}
