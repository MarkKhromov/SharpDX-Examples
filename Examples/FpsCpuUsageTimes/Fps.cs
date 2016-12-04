using System;

namespace SharpDXExamples.Examples.FpsCpuUsageTimes {
    public class Fps {
        public int FPS { get; private set; }
        int count;
        long startTime;

        public void Initialize() {
            FPS = 0;
            count = 0;
            startTime = DateTime.Now.Ticks;
        }

        public void Frame() {
            count++;
            if(DateTime.Now.Ticks >= startTime + TimeSpan.TicksPerSecond) {
                FPS = count;
                count = 0;
                startTime = DateTime.Now.Ticks;
            }
        }
    }
}