using System.Runtime.InteropServices;

namespace SharpDXExamples.Examples.FpsCpuUsageTimes {
    public class Timer {
        [DllImport("Kernel32.dll")]
        static extern bool QueryPerformanceCounter(out long lpPerformanceCount);

        [DllImport("Kernel32.dll")]
        static extern bool QueryPerformanceFrequency(out long lpFrequency);

        public float FrameTime { get; private set; }

        long frequency;
        float ticksPerMs;
        long startTime;

        public bool Initialize() {
            QueryPerformanceFrequency(out frequency);
            if(frequency == 0)
                return false;

            ticksPerMs = (float)frequency / 1000;
            QueryPerformanceCounter(out startTime);

            return true;
        }

        public void Frame() {
            long currentTime;
            QueryPerformanceCounter(out currentTime);
            var timeDifference = currentTime - startTime;
            FrameTime = timeDifference / ticksPerMs;
            startTime = currentTime;
        }
    }
}