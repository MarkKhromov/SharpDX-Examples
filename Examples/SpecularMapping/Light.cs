using SharpDX;

namespace SharpDXExamples.Examples.SpecularMapping {
    public class Light {
        public Color4 DiffuseColor { get; set; }
        public Vector3 Direction { get; set; }
        public Color4 SpecularColor { get; set; }
        public float SpecularPower { get; set; }
    }
}