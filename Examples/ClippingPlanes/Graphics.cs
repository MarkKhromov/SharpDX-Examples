using System;
using SharpDX;

namespace SharpDXExamples.Examples.ClippingPlanes {
    public class Graphics {
        const bool FullScreen = false;
        const bool VSyncEnabled = true;
        const float ScreenDepth = 1000.0f;
        const float ScreenNear = 0.1f;

        Direct3D direct3D;
        Camera camera;
        Model model;
        ClipPlaneShader clipPlaneShader;

        public bool Initialize(int screenWidth, int screenHeight, IntPtr hwnd) {
            direct3D = new Direct3D();
            if(!direct3D.Initialize(screenWidth, screenHeight, VSyncEnabled, hwnd, FullScreen, ScreenDepth, ScreenNear))
                return false;

            camera = new Camera();
            camera.Position = new Vector3(0.0f, 0.0f, -10.0f);

            model = new Model();
            if(!model.Initialize(direct3D.Device, @"Examples\ClippingPlanes\Data\Triangle.data", @"Examples\ClippingPlanes\Data\Seafloor.png"))
                return false;

            clipPlaneShader = new ClipPlaneShader();
            if(!clipPlaneShader.Initialize(direct3D.Device))
                return false;

            return true;
        }

        public void Shutdown() {
            clipPlaneShader.Shutdown();
            model.Shutdown();
            direct3D.Shutdown();
        }

        public bool Frame() {
            if(!Render())
                return false;

            return true;
        }

        bool Render() {
            var clipPlane = new Vector4(0.0f, -1.0f, 0.0f, 0.0f);
            direct3D.BeginScene(Color.Black);
            camera.Render();
            var world = direct3D.World;
            var view = camera.View;
            var projection = direct3D.Projection;
            model.Render(direct3D.DeviceContext);
            if(!clipPlaneShader.Render(direct3D.DeviceContext, model.IndexCount, world, view, projection, model.ObjectTexture, clipPlane))
                return false;
            direct3D.EndScene();
            return true;
        }
    }
}