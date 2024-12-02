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
    private const int m_iUpdateInterval = 1000; // 1秒

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
                    // if (99.0 < m_dMemoryUsagePercentage)
                    // {
                    //     m_dMemoryUsagePercentage = 0.0;
                    // }
                    // m_dMemoryUsagePercentage = m_dMemoryUsagePercentage + 0.5;
                    // MemoryGraph.Draw_Memory(MemoryGraphCanvas, m_dMemoryUsagePercentage);
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
}