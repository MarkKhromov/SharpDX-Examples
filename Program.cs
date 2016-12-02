using SharpDXExamples.Examples.BuffersShadersHLSL;
using SharpDXExamples.Examples.Core;
using SharpDXExamples.Examples.InitializingDirectX;
using SharpDXExamples.Examples.Texturing;

namespace SharpDXExamples {
    class Program {
        static void Main(string[] args) {
            Menu.RegisterItem(InitializingDirectX.Title, ExampleManager.Run<InitializingDirectX>);
            Menu.RegisterItem(BuffersShadersHLSL.Title, ExampleManager.Run<BuffersShadersHLSL>);
            Menu.RegisterItem(Texturing.Title, ExampleManager.Run<Texturing>);
            Menu.Show();
        }
    }
}