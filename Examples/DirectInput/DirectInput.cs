using System;
using SharpDX.DirectInput;
using SharpDX.Windows;
using SharpDXExamples.Examples.Core;

namespace SharpDXExamples.Examples.DirectInput {
    public class DirectInput : Example {
        public static string Title = "Direct Input";

        RenderForm renderForm;
        Input input;
        Graphics graphics;

        protected override bool Initialize() {
            renderForm = new RenderForm(Title) {
                Width = DefaultWindowWidth,
                Height = DefaultWindowHeight
            };
            input = new Input();
            if(!input.Initialize(renderForm.Handle, DefaultWindowWidth, DefaultWindowHeight))
                return false;
            graphics = new Graphics();
            if(!graphics.Initialize(DefaultWindowWidth, DefaultWindowHeight, renderForm.Handle))
                return false;

            return true;
        }

        protected override void Show() {
            RenderLoop.Run(renderForm, () => {
                int mouseX;
                int mouseY;

                input.Frame();
                input.GetMouseLocation(out mouseX, out mouseY);

                if(!graphics.Frame(mouseX, mouseY))
                    throw new InvalidOperationException();
                if(!graphics.Render())
                    throw new InvalidOperationException();

                if(input.IsPressed(Key.Escape))
                    renderForm.Close();
            });
        }

        protected override void Shutdown() {
            graphics.Shutdown();
            input.Shutdown();
            renderForm.Dispose();
        }
    }
}