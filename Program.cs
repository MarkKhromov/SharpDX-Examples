using SharpDXExamples.Examples.AmbientLighting;
using SharpDXExamples.Examples.BuffersShadersHLSL;
using SharpDXExamples.Examples.Core;
using SharpDXExamples.Examples.DiffuseLighting;
using SharpDXExamples.Examples.DirectInput;
using SharpDXExamples.Examples.DirectSound;
using SharpDXExamples.Examples.FontEngine;
using SharpDXExamples.Examples.FpsCpuUsageTimes;
using SharpDXExamples.Examples.FrustumCulling;
using SharpDXExamples.Examples.InitializingDirectX;
using SharpDXExamples.Examples.ModelRendering3D;
using SharpDXExamples.Examples.MultitexturingAndTextureArrays;
using SharpDXExamples.Examples.Rendering2D;
using SharpDXExamples.Examples.SpecularLighting;
using SharpDXExamples.Examples.Texturing;

namespace SharpDXExamples {
    class Program {
        static void Main(string[] args) {
            Menu.RegisterItem(InitializingDirectX.Title, ExampleManager.Run<InitializingDirectX>);
            Menu.RegisterItem(BuffersShadersHLSL.Title, ExampleManager.Run<BuffersShadersHLSL>);
            Menu.RegisterItem(Texturing.Title, ExampleManager.Run<Texturing>);
            Menu.RegisterItem(DiffuseLighting.Title, ExampleManager.Run<DiffuseLighting>);
            Menu.RegisterItem(ModelRendering3D.Title, ExampleManager.Run<ModelRendering3D>);
            Menu.RegisterItem(AmbientLighting.Title, ExampleManager.Run<AmbientLighting>);
            Menu.RegisterItem(SpecularLighting.Title, ExampleManager.Run<SpecularLighting>);
            Menu.RegisterItem(Rendering2D.Title, ExampleManager.Run<Rendering2D>);
            Menu.RegisterItem(FontEngine.Title, ExampleManager.Run<FontEngine>);
            Menu.RegisterItem(DirectInput.Title, ExampleManager.Run<DirectInput>);
            Menu.RegisterItem(DirectSound.Title, ExampleManager.Run<DirectSound>);
            Menu.RegisterItem(FpsCpuUsageTimes.Title, ExampleManager.Run<FpsCpuUsageTimes>);
            Menu.RegisterItem(FrustumCulling.Title, ExampleManager.Run<FrustumCulling>);
            Menu.RegisterItem(MultitexturingAndTextureArrays.Title, ExampleManager.Run<MultitexturingAndTextureArrays>);
            Menu.Show();
        }
    }
}