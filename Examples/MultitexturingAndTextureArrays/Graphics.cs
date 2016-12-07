using System;
using SharpDX;

namespace SharpDXExamples.Examples.MultitexturingAndTextureArrays {
    public class Graphics {
        const bool FullScreen = false;
        const bool VSyncEnabled = true;
        const float ScreenDepth = 1000.0f;
        const float ScreenNear = 0.1f;

        Direct3D direct3D;
        Camera camera;
        Model model;
        MultiTextureShader multiTextureShader;

        public bool Initialize(int screenWidth, int screenHeight, IntPtr hwnd) {
            direct3D = new Direct3D();
            if(!direct3D.Initialize(screenWidth, screenHeight, VSyncEnabled, hwnd, FullScreen, ScreenDepth, ScreenNear))
                return false;

            camera = new Camera();
            camera.Position = new Vector3(0.0f, 0.0f, -5.0f);

            model = new Model();
            if(!model.Initialize(direct3D.Device, @"Examples\MultitexturingAndTextureArrays\Data\Square.data", @"Examples\MultitexturingAndTextureArrays\Data\Stone.png", @"Examples\MultitexturingAndTextureArrays\Data\Dirt.png"))
                return false;

            multiTextureShader = new MultiTextureShader();
            if(!multiTextureShader.Initialize(direct3D.Device))
                return false;

            return true;
        }

        public void Shutdown() {
            multiTextureShader.Shutdown();
            model.Shutdown();
            direct3D.Shutdown();
        }

        public bool Frame() {
            if(!Render())
                return false;

            return true;
        }

        bool Render() {
            direct3D.BeginScene(Color.Black);
            camera.Render();
            var view = camera.View;
            var world = direct3D.World;
            var projection = direct3D.Projection;
            model.Render(direct3D.DeviceContext);
            if(!multiTextureShader.Render(direct3D.DeviceContext, model.IndexCount, world, view, projection, model.ObjectTextures))
                return false;
            direct3D.EndScene();

            return true;
        }
    }
}