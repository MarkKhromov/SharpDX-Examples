using System;
using SharpDX;

namespace SharpDXExamples.Examples.Transparency {
    public class Graphics {
        const bool FullScreen = false;
        const bool VSyncEnabled = true;
        const float ScreenDepth = 1000.0f;
        const float ScreenNear = 0.1f;

        Direct3D direct3D;
        Camera camera;
        Model model1;
        Model model2;
        TextureShader textureShader;
        TransparentShader transparentShader;

        public bool Initialize(int screenWidth, int screenHeight, IntPtr hwnd) {
            direct3D = new Direct3D();
            if(!direct3D.Initialize(screenWidth, screenHeight, VSyncEnabled, hwnd, FullScreen, ScreenDepth, ScreenNear))
                return false;

            camera = new Camera();
            camera.Position = new Vector3(0.0f, 0.0f, -10.0f);

            model1 = new Model();
            if(!model1.Initialize(direct3D.Device, @"Examples\Transparency\Data\Square.data", @"Examples\Transparency\Data\Dirt.png"))
                return false;

            model2 = new Model();
            if(!model2.Initialize(direct3D.Device, @"Examples\Transparency\Data\Square.data", @"Examples\Transparency\Data\Stone.png"))
                return false;

            textureShader = new TextureShader();
            if(!textureShader.Initialize(direct3D.Device))
                return false;

            transparentShader = new TransparentShader();
            if(!transparentShader.Initialize(direct3D.Device))
                return false;

            return true;
        }

        public void Shutdown() {
            transparentShader.Shutdown();
            textureShader.Shutdown();
            model2.Shutdown();
            model1.Shutdown();
            direct3D.Shutdown();
        }

        public bool Frame() {
            camera.Position = new Vector3(0.0f, 0.0f, -5.0f);

            if(!Render())
                return false;

            return true;
        }

        bool Render() {
            var blendAmount = 0.5f;
            direct3D.BeginScene(Color.Black);
            camera.Render();
            var view = camera.View;
            var world = direct3D.World;
            var projection = direct3D.Projection;
            model1.Render(direct3D.DeviceContext);
            if(!textureShader.Render(direct3D.DeviceContext, model1.IndexCount, world, view, projection, model1.ObjectTexture))
                return false;
            world = Matrix.Translation(1.0f, 0.0f, -1.0f);
            direct3D.TurnOnAlphaBlending();
            model2.Render(direct3D.DeviceContext);
            if(!transparentShader.Render(direct3D.DeviceContext, model2.IndexCount, world, view, projection, model2.ObjectTexture, blendAmount))
                return false;
            direct3D.TurnOffAlphaBlending();
            direct3D.EndScene();

            return true;
        }
    }
}