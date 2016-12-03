using System.Globalization;

namespace SharpDXExamples.Examples.Core.Helpers {
    public static class DataParser {
        public static float ParseFloat(this string data) {
            return float.Parse(data, CultureInfo.InvariantCulture);
        }
    }
}