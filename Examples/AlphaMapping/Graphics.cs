using System;
using SharpDX;

namespace SharpDXExamples.Examples.AlphaMapping {
    public class Graphics {
        const bool FullScreen = false;
        const bool VSyncEnabled = true;
        const float ScreenDepth = 1000.0f;
        const float ScreenNear = 0.1f;

        Direct3D direct3D;
        Camera camera;
        Model model;
        AlphaMapShader alphaMapShader;

        public bool Initialize(int screenWidth, int screenHeight, IntPtr hwnd) {
            direct3D = new Direct3D();
            if(!direct3D.Initialize(screenWidth, screenHeight, VSyncEnabled, hwnd, FullScreen, ScreenDepth, ScreenNear))
                return false;

            camera = new Camera();
            camera.Position = new Vector3(0.0f, 0.0f, -5.0f);

            model = new Model();
            var textures = new[] {
                @"Examples\AlphaMapping\Data\Stone.png",
                @"Examples\AlphaMapping\Data\Dirt.png",
                @"Examples\AlphaMapping\Data\Alpha.png"
            };
            if(!model.Initialize(direct3D.Device, @"Examples\AlphaMapping\Data\Square.data", textures))
                return false;

            alphaMapShader = new AlphaMapShader();
            if(!alphaMapShader.Initialize(direct3D.Device))
                return false;

            return true;
        }

        public void Shutdown() {
            alphaMapShader.Shutdown();
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
            if(!alphaMapShader.Render(direct3D.DeviceContext, model.IndexCount, world, view, projection, model.ObjectTextures))
                return false;
            direct3D.EndScene();

            return true;
        }
    }
}