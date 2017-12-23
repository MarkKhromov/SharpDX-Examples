using System;
using SharpDX.Windows;
using SharpDXExamples.Examples.Core;

namespace SharpDXExamples.Examples.ScreenFades {
    public class ScreenFades : Example {
        public const string Title = "Screen Fades";

        RenderForm renderForm;
        Graphics graphics;
        Timer timer;

        protected override bool Initialize() {
            renderForm = new RenderForm(Title) {
                Width = DefaultWindowWidth,
                Height = DefaultWindowHeight
            };
            graphics = new Graphics();
            if(!graphics.Initialize(DefaultWindowWidth, DefaultWindowHeight, renderForm.Handle))
                return false;
            timer = new Timer();
            if(!timer.Initialize())
                return false;

            return true;
        }

        protected override void Show() {
            RenderLoop.Run(renderForm, () => {
                timer.Frame();
                if(!graphics.Frame(timer.FrameTime))
                    throw new InvalidOperationException();
                if(!graphics.Render())
                    throw new InvalidOperationException();
            });
        }

        protected override void Shutdown() {
            graphics.Shutdown();
            renderForm.Dispose();
        }
    }
}