using System;
using SharpDX;

namespace SharpDXExamples.Examples.InitializingDirectX {
    public class Graphics {
        const bool FullScreen = false;
        const bool VSyncEnabled = true;
        const float ScreenDepth = 1000.0f;
        const float ScreenNear = 0.1f;

        Direct3D direct3D;

        public bool Initialize(int screenWidth, int screenHeight, IntPtr hwnd) {
            direct3D = new Direct3D();

            if(!direct3D.Initialize(screenWidth, screenHeight, VSyncEnabled, hwnd, FullScreen, ScreenDepth, ScreenNear))
                return false;

            return true;
        }

        public void Shutdown() {
            direct3D.Shutdown();
        }

        public bool Frame() {
            if(!Render())
                return false;

            return true;
        }

        bool Render() {
            direct3D.BeginScene(Color.Gray);
            direct3D.EndScene();

            return true;
        }
    }
}