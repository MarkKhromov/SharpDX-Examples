using System;
using SharpDX;

namespace SharpDXExamples.Examples.Fog {
    public class Graphics {
        const bool FullScreen = false;
        const bool VSyncEnabled = true;
        const float ScreenDepth = 1000.0f;
        const float ScreenNear = 0.1f;

        static float Rotation = 0.0f;

        Direct3D direct3D;
        Camera camera;
        Model model;
        FogShader fogShader;

        public bool Initialize(int screenWidth, int screenHeight, IntPtr hwnd) {
            direct3D = new Direct3D();
            if(!direct3D.Initialize(screenWidth, screenHeight, VSyncEnabled, hwnd, FullScreen, ScreenDepth, ScreenNear))
                return false;

            camera = new Camera();
            camera.Position = new Vector3(0.0f, 0.0f, -10.0f);

            model = new Model();
            if(!model.Initialize(direct3D.Device, @"Examples\Fog\Data\Cube.data", @"Examples\Fog\Data\Seafloor.png"))
                return false;

            fogShader = new FogShader();
            if(!fogShader.Initialize(direct3D.Device))
                return false;

            return true;
        }

        public void Shutdown() {
            fogShader.Shutdown();
            model.Shutdown();
            direct3D.Shutdown();
        }

        public bool Frame() {
            if(!Render())
                return false;

            return true;
        }

        bool Render() {
            var fogStart = 0.0f;
            var fogEnd = 10.0f;
            direct3D.BeginScene(new Color4(0.5f, 0.5f, 0.5f, 1.0f));
            camera.Render();
            var world = direct3D.World;
            var view = camera.View;
            var projection = direct3D.Projection;
            Rotation += MathUtil.Pi * 0.005f;
            if(Rotation > 360.0f)
                Rotation -= 360.0f;
            world = Matrix.RotationY(Rotation);
            model.Render(direct3D.DeviceContext);
            if(!fogShader.Render(direct3D.DeviceContext, model.IndexCount, world, view, projection, model.ObjectTexture, fogStart, fogEnd))
                return false;
            direct3D.EndScene();
            return true;
        }
    }
}