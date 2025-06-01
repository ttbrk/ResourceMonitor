using System.Windows;
using System.Diagnostics;
using System.Management;

using ResourceMonitor.Views;
using ResourceMonitor.ViewModels;

namespace ResourceMonitor.Models;

// インタフェース定義
public interface IMemoryGraphShowModel
{
    public void SetTargetWindow(MainWindow window) {}
    public void StartMemory() {}
}

// 具象クラス定義
public class MemoryGraphShowModel : IMemoryGraphShowModel
{
    private MainWindow? _csWindow;

    //メモリ関連
    private static PerformanceCounter m_RamCounter = new PerformanceCounter("Memory", "Available MBytes");
    private static double m_dAvailableMemory;
    private static ulong m_ulTotalMemory;
    private static double m_dTotalMemoryMB;
    private static double m_dUsedMemory;
    private static double m_dMemoryUsagePercent;

    public MemoryGraphShowModel()
    {
    }
    public void SetTargetWindow(MainWindow window)
    {
        _csWindow = window;
    }

    public void StartMemory()
    {
        if (_csWindow != null)
        {
            MemoryGraph.BaseDraw_Memory(_csWindow.MemoryGraphBaseCanvas, "Memory Usage :");
        }
        StartMonitoring();
    }

    private void StartMonitoring()
    {
        if (_csWindow != null)
        {
            Task.Run(() =>
            {
                while (true)
                {
                    // メモリ使用率の取得
                    // データの更新
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        MemoryGraph.Draw_Memory(_csWindow.MemoryGraphCanvas, GetMemoryUsagePercentage());
                    });
                    Thread.Sleep(Define.MEMORY_UPDATE_INTERVAL);
                }
            });
        }
    }

    private double GetMemoryUsagePercentage()
    {
        // PerformanceCounter for available memory
        m_dAvailableMemory = m_RamCounter.NextValue();

        // Get total physical memory
        ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT TotalVisibleMemorySize FROM Win32_OperatingSystem");
        m_ulTotalMemory = 0;
        foreach (ManagementObject obj in searcher.Get())
        {
            m_ulTotalMemory = (ulong)obj["TotalVisibleMemorySize"];
        }
        // Convert total memory to MB
        m_dTotalMemoryMB     = m_ulTotalMemory / 1024f;
        // Calculate used memory
        m_dUsedMemory        = m_dTotalMemoryMB - m_dAvailableMemory;
        // Calculate memory usage percentage
        m_dMemoryUsagePercent = m_dUsedMemory / m_dTotalMemoryMB * Define.VALUE_HUNDRED;
        return m_dMemoryUsagePercent;
    }
}