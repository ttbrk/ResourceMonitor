using System.Windows;
using LibreHardwareMonitor.Hardware;
using System.Diagnostics;

using ResourceMonitor.Views;

namespace ResourceMonitor.Models;

// インタフェース定義
public interface IUsageGraphShowModel
{
    public void SetTargetWindow(MainWindow window) {}
    public void StartUsage() {}
}

// 具象クラス定義
public class UsageGraphShowModel : IUsageGraphShowModel
{
    private MainWindow? _csWindow;

    private static PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
    Computer m_computer;
    private static double m_dCPUTemperature;
    private static double m_dCPUUsage;
    private static double m_dCPUClock;

    private static double m_dGPUTemperature;
    private static double m_dGPUUsage;
    private static double m_dGPUClock;
    public UsageGraphShowModel()
    {
        m_computer = new Computer
        {
            IsCpuEnabled = true,
            IsGpuEnabled = true
        };
        m_computer.Open();
    }
    public void SetTargetWindow(MainWindow window)
    {
        _csWindow = window;
    }

    public void StartUsage()
    {
        if (_csWindow != null)
        {
            UsageGraph.BaseDraw_Usage(_csWindow.CPUGraphBaseCanvas, "CPU Usage :");
            UsageGraph.BaseDraw_Usage(_csWindow.GPUGraphBaseCanvas, "GPU Usage :");
            StartMonitoring();
        }
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
                        var tCPUInfo = GetCpuInfo(m_computer);
                        m_dCPUTemperature = tCPUInfo.dTemperature;
                        m_dCPUUsage       = tCPUInfo.dUsage;
                        m_dCPUClock       = tCPUInfo.dClock;
                        UsageGraph.Draw_Usage(_csWindow.CPUGraphCanvas, m_dCPUUsage);

                        var tGPUInfo = GetGpuInfo(m_computer);
                        m_dGPUTemperature = tGPUInfo.dTemperature;
                        m_dGPUUsage       = tGPUInfo.dUsage;
                        m_dGPUClock       = tGPUInfo.dClock;
                        UsageGraph.Draw_Usage(_csWindow.GPUGraphCanvas, m_dGPUUsage);
                    });
                    Thread.Sleep(Define.UPDATE_INTERVAL);
                }
            });
        }
    }

    private (double dTemperature, double dUsage, double dClock) GetCpuInfo(Computer computer)
    {
        double _dTemperature = 0.0;
        double _dUsage       = 0.0;
        double _dClock       = 0.0;
        foreach (IHardware hardware in computer.Hardware)
        {
            // CPU に関連するハードウェアかチェック
            if (hardware.HardwareType == HardwareType.Cpu)
            {
                hardware.Update(); // センサーを更新
                // CPU センサー情報の取得
                foreach (ISensor sensor in hardware.Sensors)
                {
                    // Console.WriteLine($"{sensor.Name}: {sensor.SensorType}  {sensor.Value}%");
                    // センサーの種類が温度、使用率、クロックなどの場合
                    if (sensor.SensorType == SensorType.Temperature)
                    {
                        if (sensor.Value != null)
                        {
                            _dTemperature = (double)sensor.Value;
                        }
                    }
                    if (sensor.SensorType == SensorType.Load && sensor.Name.Equals("CPU Total"))
                    {
                        if (sensor.Value != null)
                        {
                            _dUsage = _dUsage + (double)sensor.Value;
                        }
                    }
                    if (sensor.SensorType == SensorType.Clock)
                    {
                        if (sensor.Value != null)
                        {
                            _dClock = (double)sensor.Value;
                        }
                    }
                }
            }
        }
        return (_dTemperature, _dUsage, _dClock);
    }

    private (double dTemperature, double dUsage, double dClock) GetGpuInfo(Computer computer)
    {
        double _dTemperature = 0.0;
        double _dUsage       = 0.0;
        double _dClock       = 0.0;
        foreach (IHardware hardware in computer.Hardware)
        {
            // GPU に関連するハードウェアかチェック
            if (hardware.HardwareType == HardwareType.GpuNvidia || hardware.HardwareType == HardwareType.GpuAmd)
            {
                hardware.Update(); // センサーを更新
                // GPU センサー情報の取得
                foreach (ISensor sensor in hardware.Sensors)
                {
                    // Console.WriteLine($"{sensor.Name}: {sensor.SensorType}  {sensor.Value}%");
                    // センサーの種類が温度、使用率、クロックなどの場合
                    if (sensor.SensorType == SensorType.Temperature && sensor.Name.Equals("GPU Core"))
                    {
                        if (sensor.Value != null)
                        {
                            _dTemperature = (double)sensor.Value;
                        }
                    }
                    if (sensor.SensorType == SensorType.Load && sensor.Name.Equals("GPU Memory"))
                    {
                        if (sensor.Value != null)
                        {
                            _dUsage = (double)sensor.Value;
                        }
                    }
                    if (sensor.SensorType == SensorType.Clock && sensor.Name.Equals("GPU Core"))
                    {
                        if (sensor.Value != null)
                        {
                            _dClock = (double)sensor.Value;//MHz
                        }
                    }
                    // if (sensor.SensorType == SensorType.Clock && sensor.Name.Equals("GPU Memory"))
                    // {
                    //     if (sensor.Value != null)
                    //     {
                    //         _dClock = (double)sensor.Value;//MHz
                    //     }
                    // }
                    // if (sensor.SensorType == SensorType.Throughput && sensor.Name.Equals("GPU PCIe Rx"))
                    // {
                    //     if (sensor.Value != null)
                    //     {
                    //         _dClock = (double)sensor.Value;//
                    //     }
                    // }
                    // if (sensor.SensorType == SensorType.Throughput && sensor.Name.Equals("GPU PCIe Tx"))
                    // {
                    //     if (sensor.Value != null)
                    //     {
                    //         _dClock = (double)sensor.Value;//
                    //     }
                    // }
                }
            }
        }
        return (_dTemperature, _dUsage, _dClock);
    }
}