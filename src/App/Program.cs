using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;

var serviceCollection =
    new ServiceCollection()
        .AddSingleton<SingletonDependency>()
        .AddScoped<ScopedDependency>()
        .AddTransient<TransientDependency>();

using var services = serviceCollection.BuildServiceProvider();

AnsiConsole.MarkupLine("[purple]Starting first scope[/]");
using (var scope = services.CreateScope())
{
    var singleton1 = scope.ServiceProvider.GetRequiredService<SingletonDependency>();
    singleton1.PrintInfo();
    var singleton2 = scope.ServiceProvider.GetRequiredService<SingletonDependency>();
    singleton2.PrintInfo();
    
    var scoped1 = scope.ServiceProvider.GetRequiredService<ScopedDependency>();
    scoped1.PrintInfo();
    var scoped2 = scope.ServiceProvider.GetRequiredService<ScopedDependency>();
    scoped2.PrintInfo();
    
    var transient1 = scope.ServiceProvider.GetRequiredService<TransientDependency>();
    transient1.PrintInfo();
    var transient2 = scope.ServiceProvider.GetRequiredService<TransientDependency>();
    transient2.PrintInfo();
}

AnsiConsole.MarkupLine("[purple]Starting second scope[/]");
using (var scope = services.CreateScope())
{
    var singleton1 = scope.ServiceProvider.GetRequiredService<SingletonDependency>();
    singleton1.PrintInfo();
    var singleton2 = scope.ServiceProvider.GetRequiredService<SingletonDependency>();
    singleton2.PrintInfo();
    
    var scoped1 = scope.ServiceProvider.GetRequiredService<ScopedDependency>();
    scoped1.PrintInfo();
    var scoped2 = scope.ServiceProvider.GetRequiredService<ScopedDependency>();
    scoped2.PrintInfo();
    
    var transient1 = scope.ServiceProvider.GetRequiredService<TransientDependency>();
    transient1.PrintInfo();
    var transient2 = scope.ServiceProvider.GetRequiredService<TransientDependency>();
    transient2.PrintInfo();
}

AnsiConsole.MarkupLine("[purple]Exiting application[/]");

public class SingletonDependency : IDisposable
{
    private readonly int _number;

    public SingletonDependency() => _number = DependencyCounts.GetCount<SingletonDependency>();

    public void PrintInfo() => AnsiConsole.MarkupLine($"[teal]\tSingleton dependency number {_number}[/]");

    public void Dispose() => AnsiConsole.MarkupLine($"[teal]\tDisposing singleton dependency number {_number}[/]");
}

public class ScopedDependency : IDisposable
{
    private readonly int _number;

    public ScopedDependency() => _number = DependencyCounts.GetCount<ScopedDependency>();

    public void PrintInfo() => AnsiConsole.MarkupLine($"[lime]\tScoped dependency number {_number}[/]");

    public void Dispose() => AnsiConsole.MarkupLine($"[lime]\tDisposing scoped dependency number {_number}[/]");
}

public class TransientDependency : IDisposable
{
    private readonly int _number;

    public TransientDependency() => _number = DependencyCounts.GetCount<TransientDependency>();

    public void PrintInfo() => AnsiConsole.MarkupLine($"[yellow]\tTransient dependency number {_number}[/]");

    public void Dispose() => AnsiConsole.MarkupLine($"[yellow]\tDisposing transient dependency number {_number}[/]");
}

public static class DependencyCounts
{
    private static readonly Dictionary<Type, int> Counts = new();

    public static int GetCount<T>()
    {
        var newCount = Counts.TryGetValue(typeof(T), out var currentCount)
            ? currentCount + 1
            : 1;

        Counts[typeof(T)] = newCount;
        return newCount;
    }
}