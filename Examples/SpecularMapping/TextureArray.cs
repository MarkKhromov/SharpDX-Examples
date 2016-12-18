using System.Linq;
using SharpDX.Direct3D11;
using SharpDXExamples.Examples.Core.Helpers;

namespace SharpDXExamples.Examples.SpecularMapping {
    public class TextureArray {
        public ShaderResourceView[] ObjectTextures { get; private set; }

        public bool Initialize(Device device, string[] fileNames) {
            try {
                ObjectTextures = fileNames
                    .Select(x => new ShaderResourceView(device, TextureLoader.LoadTexture2D(device, x)))
                    .ToArray()
                ;
            } catch { return false; }
            return true;
        }

        public void Shutdown() {
            foreach(var texture in ObjectTextures) {
                texture.Dispose();
            }
        }
    }
}