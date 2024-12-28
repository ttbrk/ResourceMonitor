using System.Text;
using System.Windows;
using LibreHardwareMonitor.Hardware;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Management;
using System.Diagnostics;
using System.Windows.Threading;
namespace ResourceMonitor;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>

public partial class MainWindow : Window
{

    // private static double m_dMemoryUsagePercentage = 0.0; //デバッグ用

    Computer m_computer;
    private static double m_dCPUTemperature;
    private static double m_dCPUUsage;
    private static double m_dCPUClock;

    private static double m_dGPUTemperature;
    private static double m_dGPUUsage;
    private static double m_dGPUClock;
    private const int m_iUpdateInterval = 500; // 1秒
    private DispatcherTimer timer;
    public MainWindow()
    {
        InitializeComponent();
        m_computer = new Computer
        {
            IsCpuEnabled = true,
            IsGpuEnabled = true
        };
        m_computer.Open();

        MemoryGraph.BaseDraw_Memory(MemoryGraphBaseCanvas, "Memory Usage :");
        UsageGraph.BaseDraw_Usage(CPUGraphBaseCanvas, "CPU Usage :");
        UsageGraph.BaseDraw_Usage(GPUGraphBaseCanvas, "GPU Usage :");

        // タイマーの初期化
        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromMilliseconds(100); // 1秒ごとに更新
        timer.Tick += Timer_Tick;
        timer.Start();
        // 初期表示
        UpdateTime();

        StartMonitoring();
    }

    private void StartMonitoring()
    {
        Task.Run(() =>
        {
            while (true)
            {
                // メモリ使用率の取得
                // データの更新
                Dispatcher.Invoke(() =>
                {
                    MemoryGraph.Draw_Memory(MemoryGraphCanvas, Utility.GetMemoryUsagePercentage());

                    var tCPUInfo = Utility.GetCpuInfo(m_computer);
                    m_dCPUTemperature = tCPUInfo.dTemperature;
                    m_dCPUUsage       = tCPUInfo.dUsage;
                    m_dCPUClock       = tCPUInfo.dClock;
                    UsageGraph.Draw_Usage(CPUGraphCanvas, m_dCPUUsage);

                    var tGPUInfo = Utility.GetGpuInfo(m_computer);
                    m_dGPUTemperature = tGPUInfo.dTemperature;
                    m_dGPUUsage       = tGPUInfo.dUsage;
                    m_dGPUClock       = tGPUInfo.dClock;
                    UsageGraph.Draw_Usage(GPUGraphCanvas, m_dGPUUsage);
                });

                Thread.Sleep(m_iUpdateInterval);
            }
        });
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        // タイマーごとに時刻を更新
        UpdateTime();
    }

    private void UpdateTime()
    {
        // 現在時刻を取得してTextBlockに表示
        date.Text = DateTime.Now.ToString("yyyy年MM月dd日 dddd HH時mm分ss秒");
    }

}