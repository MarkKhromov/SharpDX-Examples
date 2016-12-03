using System;
using SharpDX;

namespace SharpDXExamples.Examples.FontEngine {
    public class Graphics {
        const bool FullScreen = false;
        const bool VSyncEnabled = true;
        const float ScreenDepth = 1000.0f;
        const float ScreenNear = 0.1f;

        Direct3D direct3D;
        Camera camera;
        Text text;

        public bool Initialize(int screenWidth, int screenHeight, IntPtr hwnd) {
            direct3D = new Direct3D();
            if(!direct3D.Initialize(screenWidth, screenHeight, VSyncEnabled, hwnd, FullScreen, ScreenDepth, ScreenNear))
                return false;

            camera = new Camera();
            camera.Position = new Vector3(0.0f, 0.0f, -1.0f);
            camera.Render();
            var baseViewMatrix = camera.View;

            text = new Text();
            if(!text.Initialize(direct3D.Device, direct3D.DeviceContext, screenWidth, screenHeight, baseViewMatrix))
                return false;

            return true;
        }

        public void Shutdown() {
            text.Shutdown();
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
            direct3D.TurnOnAlphaBlending();
            if(!text.Render(direct3D.DeviceContext, world, orthogonal))
                return false;
            direct3D.TurnOffAlphaBlending();
            direct3D.TurnZBufferOn();
            direct3D.EndScene();

            return true;
        }
    }
}