using System;
using SharpDX.Windows;
using SharpDXExamples.Examples.Core;

namespace SharpDXExamples.Examples.FpsCpuUsageTimes {
    public class FpsCpuUsageTimes : Example {
        public const string Title = "FPS, CPU Usage, and Timers";

        RenderForm renderForm;
        Graphics graphics;
        Fps fps;
        Cpu cpu;
        Timer timer;

        protected override bool Initialize() {
            renderForm = new RenderForm(Title) {
                Width = DefaultWindowWidth,
                Height = DefaultWindowHeight
            };
            graphics = new Graphics();
            if(!graphics.Initialize(DefaultWindowWidth, DefaultWindowHeight, renderForm.Handle))
                return false;
            fps = new Fps();
            fps.Initialize();
            cpu = new Cpu();
            cpu.Initialize();
            timer = new Timer();
            if(!timer.Initialize())
                return false;

            return true;
        }

        protected override void Show() {
            RenderLoop.Run(renderForm, () => {
                timer.Frame();
                fps.Frame();
                cpu.Frame();

                if(!graphics.Frame(fps.FPS, cpu.CpuPercentage, timer.FrameTime))
                    throw new InvalidOperationException();
                if(!graphics.Render())
                    throw new InvalidOperationException();
            });
        }

        protected override void Shutdown() {
            cpu.Shutdown();
            graphics.Shutdown();
            renderForm.Dispose();
        }
    }
}