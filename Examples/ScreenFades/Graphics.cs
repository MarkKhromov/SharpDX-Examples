using System;
using SharpDX;

namespace SharpDXExamples.Examples.ScreenFades {
    public class Graphics {
        const bool FullScreen = false;
        const bool VSyncEnabled = true;
        const float ScreenDepth = 1000.0f;
        const float ScreenNear = 0.1f;

        static float Rotation = 0.0f;

        Direct3D direct3D;
        Camera camera;
        Model model;
        TextureShader textureShader;
        RenderTexture renderTexture;
        Bitmap bitmap;
        float fadeInTime;
        float accumulatedTime;
        float fadePercentage;
        bool fadeDone;
        FadeShader fadeShader;

        public bool Initialize(int screenWidth, int screenHeight, IntPtr hwnd) {
            direct3D = new Direct3D();
            if(!direct3D.Initialize(screenWidth, screenHeight, VSyncEnabled, hwnd, FullScreen, ScreenDepth, ScreenNear))
                return false;

            camera = new Camera();
            camera.Position = new Vector3(0.0f, 0.0f, -10.0f);

            model = new Model();
            if(!model.Initialize(direct3D.Device, @"Examples\ScreenFades\Data\Cube.data", @"Examples\ScreenFades\Data\Seafloor.png"))
                return false;

            textureShader = new TextureShader();
            if(!textureShader.Initialize(direct3D.Device))
                return false;

            renderTexture = new RenderTexture();
            if(!renderTexture.Initialize(direct3D.Device, screenWidth, screenHeight))
                return false;

            bitmap = new Bitmap();
            if(!bitmap.Initialize(direct3D.Device, screenWidth, screenHeight, screenWidth, screenHeight))
                return false;

            fadeInTime = 3000.0f;
            accumulatedTime = 0.0f;
            fadePercentage = 0.0f;
            fadeDone = false;

            fadeShader = new FadeShader();
            if(!fadeShader.Initialize(direct3D.Device))
                return false;

            return true;
        }

        public void Shutdown() {
            fadeShader.Shutdown();
            bitmap.Shutdown();
            renderTexture.Shutdown();
            textureShader.Shutdown();
            model.Shutdown();
            direct3D.Shutdown();
        }

        public bool Frame(float frameTime) {
            if(!fadeDone) {
                accumulatedTime += frameTime;
                if(accumulatedTime < fadeInTime) {
                    fadePercentage = accumulatedTime / fadeInTime;
                } else {
                    fadeDone = true;
                    fadePercentage = 1.0f;
                }
            }

            camera.Position = new Vector3(0.0f, 0.0f, -10.0f);

            return true;
        }

        public bool Render() {
            Rotation += MathUtil.Pi * 0.005f;
            if(Rotation > 360.0f) {
                Rotation -= 360.0f;
            }

            if(fadeDone) {
                if(!RenderNormalScene(Rotation))
                    return false;
            } else {
                if(!RenderToTexture(Rotation))
                    return false;
                if(!RenderFadingScene())
                    return false;
            }

            return true;
        }

        bool RenderToTexture(float rotation) {
            renderTexture.SetRenderTarget(direct3D.DeviceContext, direct3D.DepthStencilView);
            renderTexture.ClearRenderTarget(direct3D.DeviceContext, direct3D.DepthStencilView, Color.Black);
            camera.Render();

            var world = Matrix.RotationY(rotation);
            var view = camera.View;
            var projection = direct3D.Projection;
            model.Render(direct3D.DeviceContext);
            if(!textureShader.Render(direct3D.DeviceContext, model.IndexCount, world, view, projection, model.ObjectTexture))
                return false;
            direct3D.SetBackBufferRenderTarget();

            return true;
        }

        bool RenderFadingScene() {
            direct3D.BeginScene(Color.Black);
            camera.Render();

            var world = direct3D.World;
            var view = camera.View;
            var orthogonal = direct3D.Orthogonal;
            direct3D.TurnZBufferOff();
            if(!bitmap.Render(direct3D.DeviceContext, 0, 0))
                return false;
            if(!fadeShader.Render(direct3D.DeviceContext, bitmap.IndexCount, world, view, orthogonal, renderTexture.ShaderResourceView, fadePercentage))
                return false;
            direct3D.TurnZBufferOn();
            direct3D.EndScene();

            return true;
        }

        bool RenderNormalScene(float rotation) {
            direct3D.BeginScene(Color.Black);
            camera.Render();
            var world = Matrix.RotationY(rotation);
            var view = camera.View;
            var projection = direct3D.Projection;

            model.Render(direct3D.DeviceContext);
            if(!textureShader.Render(direct3D.DeviceContext, model.IndexCount, world, view, projection, model.ObjectTexture))
                return false;
            direct3D.EndScene();

            return true;
        }
    }
}