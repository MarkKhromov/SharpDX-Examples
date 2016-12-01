namespace SharpDXExamples.Examples.Core {
    public static class ExampleManager {
        public static void Show<T>() where T : Example, new() {
            var example = new T();
            example.Show();
        }
    }
}