using System;
using SharpDX;

namespace SharpDXExamples.Examples.SpecularMapping {
    public class Graphics {
        const bool FullScreen = false;
        const bool VSyncEnabled = true;
        const float ScreenDepth = 1000.0f;
        const float ScreenNear = 0.1f;

        static float Rotation = 0.0f;

        Direct3D direct3D;
        Camera camera;
        Model model;
        SpecularMapShader bumpMapShader;
        Light light;

        public bool Initialize(int screenWidth, int screenHeight, IntPtr hwnd) {
            direct3D = new Direct3D();
            if(!direct3D.Initialize(screenWidth, screenHeight, VSyncEnabled, hwnd, FullScreen, ScreenDepth, ScreenNear))
                return false;

            camera = new Camera();
            camera.Position = new Vector3(0.0f, 0.0f, -5.0f);

            model = new Model();
            var textures = new[] {
                @"Examples\SpecularMapping\Data\Stone.png",
                @"Examples\SpecularMapping\Data\Bump.png",
                @"Examples\SpecularMapping\Data\Specular.png"
            };
            if(!model.Initialize(direct3D.Device, @"Examples\SpecularMapping\Data\Cube.data", textures))
                return false;

            bumpMapShader = new SpecularMapShader();
            if(!bumpMapShader.Initialize(direct3D.Device))
                return false;

            light = new Light {
                DiffuseColor = new Color4(1.0f, 1.0f, 1.0f, 1.0f),
                Direction = new Vector3(0.0f, 0.0f, 1.0f),
                SpecularColor = Color4.White,
                SpecularPower = 16.0f
            };

            return true;
        }

        public void Shutdown() {
            bumpMapShader.Shutdown();
            model.Shutdown();
            direct3D.Shutdown();
        }

        public bool Frame() {
            if(!Render())
                return false;

            return true;
        }

        bool Render() {
            Rotation += MathUtil.Pi * 0.005f;
            if(Rotation > 360.0f)
                Rotation -= 360.0f;
            direct3D.BeginScene(Color.Black);
            camera.Render();
            var view = camera.View;
            var world = Matrix.RotationY(Rotation);
            var projection = direct3D.Projection;
            model.Render(direct3D.DeviceContext);
            if(!bumpMapShader.Render(direct3D.DeviceContext, model.IndexCount, world, view, projection, model.ObjectTextures, light.Direction, light.DiffuseColor, camera.Position, light.SpecularColor, light.SpecularPower))
                return false;
            direct3D.EndScene();

            return true;
        }
    }
}