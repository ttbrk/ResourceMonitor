using System.Diagnostics;
using System.Management;
using System.Windows;
using LibreHardwareMonitor.Hardware;

namespace ResourceMonitor;

public static class Define
{
    public const Int16 VALUE_TEN         = 10;     //
    public const Int16 VALUE_HUNDRED     = 100;    //
    public const Int16 MEMORY_GAGE_COUNT = 50;     //メモリ使用率のゲージ数
    public const Int16 CPUGPU_GAGE_COUNT = 20;     //CPU、GPU使用率のゲージ数
    public const Int16 THRESHOLD_RED     = 90;     //ゲージ色（→赤）変更閾値
    public const Int16 THRESHOLD_WHITE   = 80;     //ゲージ色（→白）変更閾値
    public const Int16 THRESHOLD_YELLO   = 70;     //ゲージ色（→黄）変更閾値
    public const Int16 THRESHOLD_GREEN   = 20;     //ゲージ色（→緑）変更閾値
    public const Int16 FONTSIZE1         = 5;      //フォントサイズ（小）
    public const Int16 FONTSIZE2         = 15;     //フォントサイズ（大）
}


public class Utility
{
    //メモリ関連
    private static PerformanceCounter m_RamCounter = new PerformanceCounter("Memory", "Available MBytes");
    private static double m_dAvailableMemory;
    private static ulong m_ulTotalMemory;
    private static double m_dTotalMemoryMB;
    private static double m_dUsedMemory;
    private static double m_dMemoryUsagePercent;

    //CPU関連
    private static PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");

    public static double GetMemoryUsagePercentage()
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

    public static (double dTemperature, double dUsage, double dClock) GetCpuInfo(Computer computer)
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

    public static (double dTemperature, double dUsage, double dClock) GetGpuInfo(Computer computer)
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