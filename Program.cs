using SharpDXExamples.Examples.AmbientLighting;
using SharpDXExamples.Examples.BuffersShadersHLSL;
using SharpDXExamples.Examples.Core;
using SharpDXExamples.Examples.DiffuseLighting;
using SharpDXExamples.Examples.InitializingDirectX;
using SharpDXExamples.Examples.ModelRendering3D;
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
            Menu.Show();
        }
    }
}