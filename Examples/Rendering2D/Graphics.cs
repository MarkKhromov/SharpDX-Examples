using System;
using SharpDX;

namespace SharpDXExamples.Examples.Rendering2D {
    public class Graphics {
        const bool FullScreen = false;
        const bool VSyncEnabled = true;
        const float ScreenDepth = 1000.0f;
        const float ScreenNear = 0.1f;

        Direct3D direct3D;
        Camera camera;
        TextureShader textureShader;
        Bitmap bitmap;

        public bool Initialize(int screenWidth, int screenHeight, IntPtr hwnd) {
            direct3D = new Direct3D();
            if(!direct3D.Initialize(screenWidth, screenHeight, VSyncEnabled, hwnd, FullScreen, ScreenDepth, ScreenNear))
                return false;

            camera = new Camera();
            camera.Position = new Vector3(0.0f, 0.0f, -10.0f);

            textureShader = new TextureShader();
            if(!textureShader.Initialize(direct3D.Device))
                return false;

            bitmap = new Bitmap();
            if(!bitmap.Initialize(direct3D.Device, screenWidth, screenHeight, @"Examples\Rendering2D\Data\Seafloor.png", 256, 256))
                return false;

            return true;
        }

        public void Shutdown() {
            bitmap.Shutdown();
            textureShader.Shutdown();
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
            var orthogonal = direct3D.Orthogonal;
            direct3D.TurnZBufferOff();
            if(!bitmap.Render(direct3D.DeviceContext, 100, 100))
                return false;
            if(!textureShader.Render(direct3D.DeviceContext, bitmap.IndexCount, world, view, orthogonal, bitmap.ObjectTexture))
                return false;
            direct3D.TurnZBufferOn();
            direct3D.EndScene();

            return true;
        }
    }
}