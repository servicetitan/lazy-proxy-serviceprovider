[![Build LazyProxy.ServiceProvider](https://github.com/servicetitan/lazy-proxy-serviceprovider/actions/workflows/ci.yml/badge.svg)](https://github.com/servicetitan/lazy-proxy-serviceprovider/actions/workflows/ci.yml)

# Lazy Dependency Injection for Microsoft's DI ServiceProvider

A [LazyProxy](https://github.com/servicetitan/lazy-proxy) can be used for IoC containers to improve performance by changing the resolve behavior.

More info can be found in the article about [Lazy Dependency Injection for .NET](https://dev.to/hypercodeplace/lazy-dependency-injection-37en).

## Get Packages

The library provides in NuGet.

```
Install-Package LazyProxy.ServiceProvider
```

## Get Started

Consider the following service:

```CSharp
public interface IMyService
{
    void Foo();
}

public class MyService : IMyService
{
    public MyService() => Console.WriteLine("Ctor");
    public void Foo() => Console.WriteLine("Foo");
}
```

A lazy registration for this service can be added like this:

```CSharp
// Creating the service collection
var serviceCollection = new ServiceCollection();

// Adding a lazy registration
serviceCollection.AddLazyScoped<IMyService, MyService>();

// Build the Service Provider
var serviceProvider = serviceCollection.BuildServiceProvider()

Console.WriteLine("Resolving the service...");
var service = serviceProvider.GetService<IMyService>();

Console.WriteLine("Executing the 'Foo' method...");
service.Foo();
```

The output for this example:

```
Resolving the service...
Executing the 'Foo' method...
Ctor
Foo
```

## Features

Currently, `LazyProxy.ServiceProvider` supports the following:
- Registration of types by interfaces;
- Passing lifetime managers;

## Performance

Here is a result of the [Benchmark test](https://github.com/servicetitan/lazy-proxy-unity/blob/master/LazyProxy.Unity.Benchmarks/UnityExtensionBenchmark.cs)

```
BenchmarkDotNet=v0.10.14, OS=Windows 10.0.17134
Intel Core i5-6600K CPU 3.50GHz (Skylake), 1 CPU, 4 logical and 4 physical cores
Frequency=3421881 Hz, Resolution=292.2369 ns, Timer=TSC
.NET Core SDK=2.1.104
  [Host]     : .NET Core 2.0.6 (CoreCLR 4.6.26212.01, CoreFX 4.6.26212.01), 64bit RyuJIT
  Job-WRIASW : .NET Framework 4.7.1 (CLR 4.0.30319.42000), 64bit RyuJIT-v4.7.3131.0
  Job-AHTPGH : .NET Core 2.0.6 (CoreCLR 4.6.26212.01, CoreFX 4.6.26212.01), 64bit RyuJIT

InvocationCount=1  LaunchCount=10  RunStrategy=ColdStart  TargetCount=1 
  

                     Method | Runtime |        Mean |        Error |      StdDev |
--------------------------- |-------- |------------:|-------------:|------------:|
               RegisterType |     Clr |  4,137.7 us |    413.30 us |   273.37 us |
               RegisterLazy |     Clr | 20,674.2 us |  8,720.50 us | 5,768.07 us |
            ColdResolveType |     Clr | 46,964.2 us |  7,248.30 us | 4,794.30 us |
            ColdResolveLazy |     Clr | 45,114.7 us | 14,851.68 us | 9,823.47 us |
             HotResolveType |     Clr | 11,071.5 us |    537.94 us |   355.81 us |
             HotResolveLazy |     Clr |    459.6 us |     81.59 us |    53.97 us |
               InvokeMethod |     Clr |    360.1 us |    104.89 us |    69.38 us |
  InvokeLazyMethodFirstTime |     Clr | 16,164.2 us |    725.08 us |   479.60 us |
 InvokeLazyMethodSecondTime |     Clr |    315.4 us |     53.41 us |    35.33 us |
               RegisterType |    Core |  3,687.9 us |    395.56 us |   261.64 us |
               RegisterLazy |    Core | 17,750.2 us |  7,664.41 us | 5,069.54 us |
            ColdResolveType |    Core | 50,519.3 us |  2,217.58 us | 1,466.79 us |
            ColdResolveLazy |    Core | 46,829.4 us |  1,848.44 us | 1,222.63 us |
             HotResolveType |    Core | 10,911.2 us |    542.79 us |   359.02 us |
             HotResolveLazy |    Core |    471.4 us |     47.37 us |    31.34 us |
               InvokeMethod |    Core |    333.8 us |     23.26 us |    15.39 us |
  InvokeLazyMethodFirstTime |    Core | 16,226.2 us |    527.92 us |   349.18 us |
 InvokeLazyMethodSecondTime |    Core |    336.4 us |     68.45 us |    45.28 us |

// * Legends *
  Mean   : Arithmetic mean of all measurements
  Error  : Half of 99.9% confidence interval
  StdDev : Standard deviation of all measurements
  1 us   : 1 Microsecond (0.000001 sec)
```

## License

This project is licensed under the Apache License, Version 2.0. - see the [LICENSE](https://github.com/servicetitan/lazy-proxy-unity/blob/master/LICENSE) file for details.