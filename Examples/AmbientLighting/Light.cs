using SharpDX;

namespace SharpDXExamples.Examples.AmbientLighting {
    public class Light {
        public Color4 AmbientColor { get; set; }
        public Color4 DiffuseColor { get; set; }
        public Vector3 Direction { get; set; }
    }
}