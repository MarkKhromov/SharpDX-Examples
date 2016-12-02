using SharpDX;
using SharpDX.Direct3D11;

namespace SharpDXExamples.Examples.Texturing {
    public class Model {
        struct VertexType {
            public Vector3 Position;
            public Vector2 Texture;
        }

        public int IndexCount { get; private set; }
        public ShaderResourceView ObjectTexture => texture.ObjectTexture;

        Buffer vertexBuffer;
        Buffer indexBuffer;
        int vertexCount;
        Texture texture;

        public bool Initialize(Device device, string textureFileName) {
            if(!InitializeBuffers(device))
                return false;

            if(!LoadTexture(device, textureFileName))
                return false;

            return true;
        }

        public void Shutdown() {
            ReleaseTexture();
            ShutdownBuffers();
        }

        public void Render(DeviceContext deviceContext) {
            RenderBuffers(deviceContext);
        }

        bool InitializeBuffers(Device device) {
            try {
                var vertices = new[] {
                    new VertexType {
                        Position = new Vector3(-1.0f, -1.0f, 0.0f),
                        Texture = new Vector2(0.0f, 1.0f)
                    },
                    new VertexType {
                        Position = new Vector3(0.0f, 1.0f, 0.0f),
                        Texture = new Vector2(0.5f, 0.0f)
                    },
                    new VertexType {
                        Position = new Vector3(1.0f, -1.0f, 0.0f),
                        Texture = new Vector2(1.0f, 1.0f)
                    }
                };
                vertexCount = vertices.Length;

                var indices = new[] { 0, 1, 2 };
                IndexCount = indices.Length;

                var vertexBufferDescription = new BufferDescription {
                    Usage = ResourceUsage.Default,
                    SizeInBytes = Utilities.SizeOf<VertexType>() * vertexCount,
                    BindFlags = BindFlags.VertexBuffer,
                    CpuAccessFlags = CpuAccessFlags.None,
                    OptionFlags = ResourceOptionFlags.None,
                    StructureByteStride = 0
                };

                vertexBuffer = Buffer.Create(device, vertices, vertexBufferDescription);

                var indexBufferDescription = new BufferDescription {
                    Usage = ResourceUsage.Default,
                    SizeInBytes = Utilities.SizeOf<int>() * IndexCount,
                    BindFlags = BindFlags.IndexBuffer,
                    CpuAccessFlags = CpuAccessFlags.None,
                    OptionFlags = ResourceOptionFlags.None,
                    StructureByteStride = 0
                };

                indexBuffer = Buffer.Create(device, indices, indexBufferDescription);
            } catch { return false; }
            return true;
        }

        void ShutdownBuffers() {
            indexBuffer.Dispose();
            vertexBuffer.Dispose();
        }

        void RenderBuffers(DeviceContext deviceContext) {
            deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer, Utilities.SizeOf<VertexType>(), 0));
            deviceContext.InputAssembler.SetIndexBuffer(indexBuffer, SharpDX.DXGI.Format.R32_UInt, 0);
            deviceContext.InputAssembler.PrimitiveTopology = SharpDX.Direct3D.PrimitiveTopology.TriangleList;
        }

        bool LoadTexture(Device device, string fileName) {
            texture = new Texture();
            if(!texture.Initialize(device, fileName))
                return false;

            return true;
        }

        void ReleaseTexture() {
            texture.Shutdown();
        }
    }
}