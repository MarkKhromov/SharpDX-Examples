using SharpDXExamples.Examples.Core;
using SharpDXExamples.Examples.InitializingDirectX;

namespace SharpDXExamples {
    class Program {
        static void Main(string[] args) {
            Menu.RegisterItem(InitializingDirectX.Title, ExampleManager.Run<InitializingDirectX>);
            Menu.Show();
        }
    }
}