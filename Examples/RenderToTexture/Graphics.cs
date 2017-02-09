using System;
using SharpDX;

namespace SharpDXExamples.Examples.RenderToTexture {
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
        RenderTexture renderTexture;
        DebugWindow debugWindow;
        TextureShader textureShader;

        public bool Initialize(int screenWidth, int screenHeight, IntPtr hwnd) {
            direct3D = new Direct3D();
            if(!direct3D.Initialize(screenWidth, screenHeight, VSyncEnabled, hwnd, FullScreen, ScreenDepth, ScreenNear))
                return false;

            camera = new Camera();
            camera.Position = new Vector3(0.0f, 0.0f, -10.0f);

            model = new Model();
            if(!model.Initialize(direct3D.Device, @"Examples\RenderToTexture\Data\Cube.data", @"Examples\RenderToTexture\Data\Seafloor.png"))
                return false;

            lightShader = new LightShader();
            if(!lightShader.Initialize(direct3D.Device))
                return false;

            light = new Light {
                DiffuseColor = Color.White,
                Direction = new Vector3(0.0f, 0.0f, 1.0f)
            };

            renderTexture = new RenderTexture();
            if(!renderTexture.Initialize(direct3D.Device, screenWidth, screenHeight))
                return false;

            debugWindow = new DebugWindow();
            if(!debugWindow.Initialize(direct3D.Device, screenWidth, screenHeight, 100, 100))
                return false;

            textureShader = new TextureShader();
            if(!textureShader.Initialize(direct3D.Device))
                return false;

            return true;
        }

        public void Shutdown() {
            textureShader.Shutdown();
            renderTexture.Shutdown();
            lightShader.Shutdown();
            model.Shutdown();
            direct3D.Shutdown();
        }

        public bool Frame() {
            if(!Render())
                return false;

            return true;
        }

        bool Render() {
            if(!RenderToTexture())
                return false;
            direct3D.BeginScene(Color.Black);
            if(!RenderScene())
                return false;
            direct3D.TurnZBufferOff();
            var world = direct3D.World;
            var view = camera.View;
            var orthogonal = direct3D.Orthogonal;
            if(!debugWindow.Render(direct3D.DeviceContext, 50, 50))
                return false;
            if(!textureShader.Render(direct3D.DeviceContext, debugWindow.IndexCount, world, view, orthogonal, renderTexture.ShaderResourceView))
                return false;
            direct3D.TurnZBufferOn();
            direct3D.EndScene();
            return true;
        }

        bool RenderToTexture() {
            renderTexture.SetRenderTarget(direct3D.DeviceContext, direct3D.DepthStencilView);
            renderTexture.ClearRenderTarget(direct3D.DeviceContext, direct3D.DepthStencilView, Color.Blue);

            if(!RenderScene())
                return false;

            direct3D.SetBackBufferRenderTarget();

            return true;
        }

        bool RenderScene() {
            camera.Render();
            var world = direct3D.World;
            var view = camera.View;
            var projection = direct3D.Projection;

            Rotation += MathUtil.Pi * 0.005f;
            if(Rotation > 360.0f)
                Rotation -= 360.0f;

            world = Matrix.RotationY(Rotation);

            model.Render(direct3D.DeviceContext);

            if(!lightShader.Render(direct3D.DeviceContext, model.IndexCount, world, view, projection, model.ObjectTexture, light.Direction, light.DiffuseColor))
                return false;

            return true;
        }
    }
}