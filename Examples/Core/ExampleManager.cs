namespace SharpDXExamples.Examples.Core {
    public static class ExampleManager {
        public static void Run<T>() where T : Example, new() {
            var example = new T();
            example.Run();
        }
    }
}