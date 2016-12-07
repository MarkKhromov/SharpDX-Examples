using System;
using SharpDX;

namespace SharpDXExamples.Examples.FrustumCulling {
    public class Graphics {
        const bool FullScreen = false;
        const bool VSyncEnabled = true;
        const float ScreenDepth = 1000.0f;
        const float ScreenNear = 0.1f;

        Direct3D direct3D;
        Camera camera;
        Text text;
        Model model;
        LightShader lightShader;
        Light light;
        ModelList modelList;
        Frustum frustum;

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

            model = new Model();
            if(!model.Initialize(direct3D.Device, @"Examples\FrustumCulling\Data\Sphere.data", @"Examples\FrustumCulling\Data\Seafloor.png"))
                return false;

            lightShader = new LightShader();
            if(!lightShader.Initialize(direct3D.Device))
                return false;

            light = new Light {
                Direction = new Vector3(0.0f, 0.0f, 1.0f)
            };

            modelList = new ModelList();
            if(!modelList.Initialize(25))
                return false;

            frustum = new Frustum();

            return true;
        }

        public void Shutdown() {
            lightShader.Shutdown();
            model.Shutdown();
            text.Shutdown();
            direct3D.Shutdown();
        }

        public bool Frame(float rotationY) {
            camera.Position = new Vector3(0.0f, 0.0f, -10.0f);
            camera.Rotation = new Vector3(0.0f, rotationY, 0.0f);

            return true;
        }

        public bool Render() {
            direct3D.BeginScene(Color.Black);
            camera.Render();
            var view = camera.View;
            var world = direct3D.World;
            var projection = direct3D.Projection;
            var orthogonal = direct3D.Orthogonal;
            frustum.ConstructFrustum(ScreenDepth, projection, view);
            var modelCount = modelList.Count;
            var renderCount = 0;
            for(int i = 0; i < modelCount; i++) {
                float positionX;
                float positionY;
                float positionZ;
                Color4 color;
                modelList.GetData(i, out positionX, out positionY, out positionZ, out color);
                var radius = 1.0f;
                if(frustum.CheckSphere(positionX, positionY, positionZ, radius)) {
                    world = Matrix.Translation(positionX, positionY, positionZ);
                    model.Render(direct3D.DeviceContext);
                    lightShader.Render(direct3D.DeviceContext, model.IndexCount, world, view, projection, model.ObjectTexture, light.Direction, color);
                    world = direct3D.World;
                    renderCount++;
                }
            }
            if(!text.SetRenderCount(renderCount, direct3D.DeviceContext))
                return false;
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