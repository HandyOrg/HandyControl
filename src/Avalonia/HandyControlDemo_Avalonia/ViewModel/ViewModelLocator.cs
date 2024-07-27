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

        _serviceProvider = services.BuildServiceProvider();
    }

    public MainViewModel Main => _serviceProvider.GetService<MainViewModel>()!;
}
