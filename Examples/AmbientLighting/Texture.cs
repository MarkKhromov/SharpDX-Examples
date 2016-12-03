using SharpDX.Direct3D11;
using SharpDXExamples.Examples.Core.Helpers;

namespace SharpDXExamples.Examples.AmbientLighting {
    public class Texture {
        public ShaderResourceView ObjectTexture { get; private set; }

        public bool Initialize(Device device, string fileName) {
            try {
                var texture = TextureLoader.LoadTexture2D(device, fileName);
                ObjectTexture = new ShaderResourceView(device, texture);
            } catch { return false; }
            return true;
        }

        public void Shutdown() {
            ObjectTexture.Dispose();
        }
    }
}