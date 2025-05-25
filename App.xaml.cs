using System.Windows;
using Microsoft.Extensions.DependencyInjection;

using ResourceMonitor.Views;
using ResourceMonitor.ViewModels;
using ResourceMonitor.Models;

namespace ResourceMonitor;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private readonly ServiceProvider _serviceProvider;

    public App()
    {
        // サービスコレクションの作成
        var serviceCollection = new ServiceCollection();
        ConfigureServices(serviceCollection);

        // サービスプロバイダーのビルド
        _serviceProvider = serviceCollection.BuildServiceProvider();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        // ViewModelの登録
        services.AddSingleton<IMainWindowViewModel, MainWindowViewModel>();

        // Modelの登録
        services.AddSingleton<IDateShowModel, DateShowModel>();
        services.AddSingleton<IMemoryGraphShowModel, MemoryGraphShowModel>();
        services.AddSingleton<IUsageGraphShowModel, UsageGraphShowModel>();

        // Viewの登録
        services.AddTransient<MainWindow>();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // MainWindowインスタンス生成
        MainWindow csMainWindow = _serviceProvider.GetRequiredService<MainWindow>();

        // 日付表示
        var csDateShowModel = _serviceProvider.GetRequiredService<IDateShowModel>();
        csDateShowModel.StartDate();

        // メモリ使用量グラフ表示
        var csMemoryGraphShowModel = _serviceProvider.GetRequiredService<IMemoryGraphShowModel>();
        csMemoryGraphShowModel.SetTargetWindow(csMainWindow);
        csMemoryGraphShowModel.StartMemory();

        // CPU/GPU利用率グラフ表示
        var csUsageGraphShowModel = _serviceProvider.GetRequiredService<IUsageGraphShowModel>();
        csUsageGraphShowModel.SetTargetWindow(csMainWindow);
        csUsageGraphShowModel.StartUsage();

        // 画面表示
        csMainWindow.Show();
    }
}

