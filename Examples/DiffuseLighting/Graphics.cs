using System;
using SharpDX;

namespace SharpDXExamples.Examples.DiffuseLighting {
    public class Graphics {
        const bool FullScreen = false;
        const bool VSyncEnabled = true;
        const float ScreenDepth = 1000.0f;
        const float ScreenNear = 0.1f;

        static float Rotation = 0.0f;

        Direct3D direct3D;
        Camera camera;
        Model model;
        LightShader lightShader;
        Light light;

        public bool Initialize(int screenWidth, int screenHeight, IntPtr hwnd) {
            direct3D = new Direct3D();
            if(!direct3D.Initialize(screenWidth, screenHeight, VSyncEnabled, hwnd, FullScreen, ScreenDepth, ScreenNear))
                return false;

            camera = new Camera();
            camera.Position = new Vector3(0.0f, 0.0f, -10.0f);

            model = new Model();
            if(!model.Initialize(direct3D.Device, @"Examples\DiffuseLighting\Data\Seafloor.png"))
                return false;

            lightShader = new LightShader();
            if(!lightShader.Initialize(direct3D.Device))
                return false;

            light = new Light {
                DiffuseColor = new Color4(1.0f, 0.0f, 1.0f, 1.0f),
                Direction = new Vector3(0.0f, 0.0f, 1.0f)
            };

            return true;
        }

        public void Shutdown() {
            lightShader.Shutdown();
            model.Shutdown();
            direct3D.Shutdown();
        }

        public bool Frame() {
            Rotation += MathUtil.Pi * 0.01f;
            if(Rotation > 360.0f)
                Rotation -= 360.0f;

            if(!Render(Rotation))
                return false;

            return true;
        }

        bool Render(float rotation) {
            direct3D.BeginScene(Color.Black);
            camera.Render();
            var view = camera.View;
            var world = Matrix.RotationY(rotation);
            var projection = direct3D.Projection;
            model.Render(direct3D.DeviceContext);
            if(!lightShader.Render(direct3D.DeviceContext, model.IndexCount, world, view, projection, model.ObjectTexture, light.Direction, light.DiffuseColor))
                return false;
            direct3D.EndScene();

            return true;
        }
    }
}