using System.Collections.Generic;
using SharpDX.Direct3D11;
using SharpDXExamples.Examples.Core.Helpers;

namespace SharpDXExamples.Examples.MultitexturingAndTextureArrays {
    public class TextureArray {
        public ShaderResourceView[] ObjectTextures { get; private set; }

        public bool Initialize(Device device, string fileName1, string fileName2) {
            try {
                var textures = new List<ShaderResourceView> {
                    new ShaderResourceView(device, TextureLoader.LoadTexture2D(device, fileName1)),
                    new ShaderResourceView(device, TextureLoader.LoadTexture2D(device, fileName2))
                };
                ObjectTextures = textures.ToArray();
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