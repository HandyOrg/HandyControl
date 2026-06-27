using System;
using HandyControlDemo.Service;
using Microsoft.Extensions.DependencyInjection;

namespace HandyControlDemo.ViewModel;

public class ViewModelLocator
{
    private static readonly Lazy<ViewModelLocator> InstanceInternal =
        new(() => new ViewModelLocator(), isThreadSafe: true);

    public static ViewModelLocator Instance => InstanceInternal.Value;

    private readonly IServiceProvider _serviceProvider;

    private ViewModelLocator()
    {
        var services = new ServiceCollection();

        services.AddSingleton<DataService>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<InputElementDemoViewModel>();
        services.AddTransient<CardDemoViewModel>();
        services.AddTransient<DialogDemoViewModel>();

        _serviceProvider = services.BuildServiceProvider();
    }

    public MainViewModel Main => _serviceProvider.GetService<MainViewModel>()!;

    public InputElementDemoViewModel InputElementDemo => _serviceProvider.GetService<InputElementDemoViewModel>()!;

    public CardDemoViewModel CardDemo => _serviceProvider.GetService<CardDemoViewModel>()!;

    public DialogDemoViewModel DialogDemo => _serviceProvider.GetService<DialogDemoViewModel>()!;
}
