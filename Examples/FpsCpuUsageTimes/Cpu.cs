using System;
using System.Diagnostics;

namespace SharpDXExamples.Examples.FpsCpuUsageTimes {
    public class Cpu {
        public float CpuPercentage { get; private set; }
        PerformanceCounter counter;
        long lastSampleTime;

        public void Initialize() {
            counter = new PerformanceCounter {
                CategoryName = "Processor",
                CounterName = "% Processor Time",
                InstanceName = "_Total"
            };
            lastSampleTime = DateTime.Now.Ticks;
            CpuPercentage = 0.0f;
        }

        public void Shutdown() {
            counter.Dispose();
        }

        public void Frame() {
            if(lastSampleTime + TimeSpan.TicksPerSecond < DateTime.Now.Ticks) {
                lastSampleTime = DateTime.Now.Ticks;
                CpuPercentage = counter.NextValue();
            }
        }
    }
}