﻿using System.Diagnostics;
using System.Management;

namespace ResourceMonitor;

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
        m_dMemoryUsagePercent = m_dUsedMemory / m_dTotalMemoryMB * 100;
        return m_dMemoryUsagePercent;
    }

    public static double GetCpuUsagePercentage()
    {
        return cpuCounter.NextValue();
    }





    public static double GetGpuUsagePercentage()
    {
        double gpuUsage = 0.0;
        var searcher    = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PerfFormattedData_GPUPerformanceCounters_GPUAdapterMemory");
        var gpuCounters = searcher.Get().Cast<ManagementObject>().FirstOrDefault();
        if (gpuCounters != null)
        {
            ulong totalGpuMemory = (ulong)gpuCounters["DedicatedLimit"];
            ulong usedGpuMemory  = totalGpuMemory - (ulong)gpuCounters["DedicatedAvailable"];
            gpuUsage             = (double)usedGpuMemory / totalGpuMemory * 100;
        }
        return gpuUsage;
    }



// using System;
// using OpenHardwareMonitor.Hardware;

// class Program
// {
//     static void Main()
//     {
//         Computer computer = new Computer { GPUEnabled = true };
//         computer.Open();
        
//         foreach (var hardwareItem in computer.Hardware)
//         {
//             if (hardwareItem.HardwareType == HardwareType.GpuNvidia || hardwareItem.HardwareType == HardwareType.GpuAti)
//             {
//                 hardwareItem.Update();
//                 foreach (var sensor in hardwareItem.Sensors)
//                 {
//                     if (sensor.SensorType == SensorType.Load)
//                     {
//                         Console.WriteLine($"{sensor.Name}: {sensor.Value}%");
//                     }
//                 }
//             }
//         }

//         computer.Close();
//     }
// }




}
