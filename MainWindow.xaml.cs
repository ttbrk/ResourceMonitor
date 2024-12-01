using System.Text;
using System.Windows;
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

    private static double m_dMemoryUsagePercentage = 0.0;
    private static double m_dCPUusagePercentage;
    private static double m_dGPUusagePercentage;
    private const int m_iUpdateInterval = 100; // 1秒
    private double[] memoryUsageData = new double[60]; // 最新60秒のデータ
    // private double canvasWidth;
    // private double canvasHeight;

    public MainWindow()
    {
        InitializeComponent();
        MemoryGraph.BaseDraw_Memory(MemoryGraphBaseCanvas);
        UsageGraph.BaseDraw_Usage(CPUGraphBaseCanvas);
        UsageGraph.BaseDraw_Usage(GPUGraphBaseCanvas);
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
                    UpdateMemoryUsage(Utility.GetMemoryUsagePercentage());

                    m_dCPUusagePercentage = Utility.GetCpuUsagePercentage();
                    m_dGPUusagePercentage = 12.5;
                    UsageGraph.Draw_Usage(CPUGraphCanvas, m_dCPUusagePercentage, "CPU Usage");
                    UsageGraph.Draw_Usage(GPUGraphCanvas, m_dGPUusagePercentage, "GPU Usage");
                });

                Thread.Sleep(m_iUpdateInterval);
            }
        });
    }

    private void UpdateMemoryUsage(double usagePercentage)
    {
        // // 最新データを配列に追加
        // Array.Copy(memoryUsageData, 1, memoryUsageData, 0, memoryUsageData.Length - 1);
        // memoryUsageData[^1] = usagePercentage;

        // 使用率テキストの更新
        MemoryUsageText.Text = $"Memory Usage: {usagePercentage:F1}%";

        // グラフの描画
        // DrawGraph();
    }


    private void DrawGraph(double usagePercentage)
    {
        // GraphCanvas.Children.Clear();

        // // キャンバスのサイズを取得
        // canvasWidth = GraphCanvas.ActualWidth;
        // canvasHeight = GraphCanvas.ActualHeight;

        // // データを描画
        // if (canvasWidth <= 0 || canvasHeight <= 0) return;

        // double step = canvasWidth / (memoryUsageData.Length - 1);

        // Polyline polyline = new Polyline
        // {
        //     Stroke = Brushes.Blue,
        //     StrokeThickness = 2
        // };

        // for (int i = 0; i < memoryUsageData.Length; i++)
        // {
        //     double x = step * i;
        //     double y = canvasHeight - (memoryUsageData[i] / 100.0 * canvasHeight);
        //     polyline.Points.Add(new Point(x, y));
        // }

        // GraphCanvas.Children.Add(polyline);
    }

    // private void GraphCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
    // {
    //     // サイズ変更時にグラフを再描画
    //     // DrawGraph();
    // }

}