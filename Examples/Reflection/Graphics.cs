using System;
using SharpDX;

namespace SharpDXExamples.Examples.Reflection {
    public class Graphics {
        const bool FullScreen = false;
        const bool VSyncEnabled = true;
        const float ScreenDepth = 1000.0f;
        const float ScreenNear = 0.1f;

        static float Rotation1 = 0.0f;
        static float Rotation2 = 0.0f;

        Direct3D direct3D;
        Camera camera;
        Model model;
        TextureShader textureShader;
        RenderTexture renderTexture;
        Model floorModel;
        ReflectionShader reflectionShader;

        public bool Initialize(int screenWidth, int screenHeight, IntPtr hwnd) {
            direct3D = new Direct3D();
            if(!direct3D.Initialize(screenWidth, screenHeight, VSyncEnabled, hwnd, FullScreen, ScreenDepth, ScreenNear))
                return false;

            camera = new Camera();
            camera.Position = new Vector3(0.0f, 0.0f, -10.0f);

            model = new Model();
            if(!model.Initialize(direct3D.Device, @"Examples\Reflection\Data\Cube.data", @"Examples\Reflection\Data\Seafloor.png"))
                return false;

            textureShader = new TextureShader();
            if(!textureShader.Initialize(direct3D.Device))
                return false;

            renderTexture = new RenderTexture();
            if(!renderTexture.Initialize(direct3D.Device, screenWidth, screenHeight))
                return false;

            floorModel = new Model();
            if(!floorModel.Initialize(direct3D.Device, @"Examples\Reflection\Data\Floor.data", @"Examples\Reflection\Data\Blue.png"))
                return false;

            reflectionShader = new ReflectionShader();
            if(!reflectionShader.Initialize(direct3D.Device))
                return false;

            return true;
        }

        public void Shutdown() {
            reflectionShader.Shutdown();
            floorModel.Shutdown();
            renderTexture.Shutdown();
            textureShader.Shutdown();
            model.Shutdown();
            direct3D.Shutdown();
        }

        public bool Frame() {
            camera.Position = new Vector3(0.0f, 0.0f, -10.0f);

            if(!Render())
                return false;

            return true;
        }

        bool Render() {
            if(!RenderToTexture())
                return false;
            if(!RenderScene())
                return false;
            return true;
        }

        bool RenderToTexture() {
            renderTexture.SetRenderTarget(direct3D.DeviceContext, direct3D.DepthStencilView);
            renderTexture.ClearRenderTarget(direct3D.DeviceContext, direct3D.DepthStencilView, Color.Black);

            camera.RenderReflection(-1.5f);

            var reflection = camera.Reflection;
            var world = direct3D.World;
            var projection = direct3D.Projection;
            Rotation1 += MathUtil.Pi * 0.005f;
            if(Rotation1 > 360.0f)
                Rotation1 -= 360.0f;
            world = Matrix.RotationY(Rotation1);
            model.Render(direct3D.DeviceContext);
            if(!textureShader.Render(direct3D.DeviceContext, model.IndexCount, world, reflection, projection, model.ObjectTexture))
                return false;
            direct3D.SetBackBufferRenderTarget();

            return true;
        }
        static float s = 1.0f;
        bool RenderScene() {
            direct3D.BeginScene(Color.Black);
            camera.Render();
            var world = direct3D.World;
            var view = camera.View;
            var projection = direct3D.Projection;
            Rotation2 += MathUtil.Pi * 0.005f;
            if(Rotation2 > 360.0f)
                Rotation2 -= 360.0f;
            world = Matrix.RotationY(Rotation2);
            model.Render(direct3D.DeviceContext);
            if(!textureShader.Render(direct3D.DeviceContext, model.IndexCount, world, view, projection, model.ObjectTexture))
                return false;
            world = Matrix.Translation(0.0f, -1.5f, 0.0f);
            var reflection = camera.Reflection;
            floorModel.Render(direct3D.DeviceContext);
            if(!reflectionShader.Render(direct3D.DeviceContext, floorModel.IndexCount, world, view, projection, floorModel.ObjectTexture, renderTexture.ShaderResourceView, reflection))
                return false;
            direct3D.EndScene();

            return true;
        }
    }
}