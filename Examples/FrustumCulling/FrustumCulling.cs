using System;
using SharpDXExamples.Examples.Core;
using SharpDX.DirectInput;
using SharpDX.Windows;

namespace SharpDXExamples.Examples.FrustumCulling {
    public class FrustumCulling : Example {
        public const string Title = "Frustum Culling";

        RenderForm renderForm;
        Input input;
        Graphics graphics;
        Timer timer;
        Position position;

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
            timer = new Timer();
            if(!timer.Initialize())
                return false;
            position = new Position();
            return true;
        }

        protected override void Show() {
            RenderLoop.Run(renderForm, () => {
                timer.Frame();
                if(!input.Frame())
                    throw new InvalidOperationException();
                position.SetFrameTime(timer.FrameTime);
                position.TurnLeft(input.IsPressed(Key.Left));
                position.TurnRight(input.IsPressed(Key.Right));
                var rotationY = position.RotationY;
                if(!graphics.Frame(rotationY))
                    throw new InvalidOperationException();
                if(!graphics.Render())
                    throw new InvalidOperationException();
            });
        }

        protected override void Shutdown() {
            graphics.Shutdown();
            input.Shutdown();
            renderForm.Dispose();
        }
    }
}