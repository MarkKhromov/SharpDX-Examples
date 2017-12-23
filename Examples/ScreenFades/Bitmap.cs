using SharpDX;
using SharpDX.Direct3D11;

namespace SharpDXExamples.Examples.ScreenFades {
    public class Bitmap {
        struct VertexType {
            public Vector3 Position;
            public Vector2 Texture;
        }

        public int IndexCount { get; private set; }

        Buffer vertexBuffer;
        Buffer indexBuffer;
        int vertexCount;
        int screenWidth;
        int screenHeight;
        int bitmapWidth;
        int bitmapHeight;
        int previousPosX;
        int previousPosY;

        public bool Initialize(Device device, int screenWidth, int screenHeight, int bitmapWidth, int bitmapHeight) {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.bitmapWidth = bitmapWidth;
            this.bitmapHeight = bitmapHeight;

            previousPosX = -1;
            previousPosY = -1;

            if(!InitializeBuffers(device))
                return false;

            return true;
        }

        public void Shutdown() {
            ShutdownBuffers();
        }

        public bool Render(DeviceContext deviceContext, int positionX, int positionY) {
            if(!UpdateBuffers(deviceContext, positionX, positionY))
                return false;

            RenderBuffers(deviceContext);

            return true;
        }

        bool InitializeBuffers(Device device) {
            try {
                vertexCount = 6;
                var vertices = new VertexType[vertexCount];
                IndexCount = vertexCount;
                var indices = new int[IndexCount];
                for(int i = 0; i < IndexCount; i++) {
                    indices[i] = i;
                }

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

        bool UpdateBuffers(DeviceContext deviceContext, int positionX, int positionY) {
            if(positionX == previousPosX && positionY == previousPosY)
                return true;

            previousPosX = positionX;
            previousPosY = positionY;

            var left = (float)-screenWidth / 2 + positionX;
            var right = left + bitmapWidth;
            var top = (float)screenHeight / 2 - positionY;
            var bottom = top - bitmapHeight;

            var vertices = new VertexType[vertexCount];
            vertices[0].Position = new Vector3(left, top, 0.0f);
            vertices[0].Texture = new Vector2(0.0f, 0.0f);
            vertices[1].Position = new Vector3(right, bottom, 0.0f);
            vertices[1].Texture = new Vector2(1.0f, 1.0f);
            vertices[2].Position = new Vector3(left, bottom, 0.0f);
            vertices[2].Texture = new Vector2(0.0f, 1.0f);
            vertices[3].Position = new Vector3(left, top, 0.0f);
            vertices[3].Texture = new Vector2(0.0f, 0.0f);
            vertices[4].Position = new Vector3(right, top, 0.0f);
            vertices[4].Texture = new Vector2(1.0f, 0.0f);
            vertices[5].Position = new Vector3(right, bottom, 0.0f);
            vertices[5].Texture = new Vector2(1.0f, 1.0f);

            deviceContext.UpdateSubresource(vertices, vertexBuffer);

            return true;
        }

        void RenderBuffers(DeviceContext deviceContext) {
            deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(vertexBuffer, Utilities.SizeOf<VertexType>(), 0));
            deviceContext.InputAssembler.SetIndexBuffer(indexBuffer, SharpDX.DXGI.Format.R32_UInt, 0);
            deviceContext.InputAssembler.PrimitiveTopology = SharpDX.Direct3D.PrimitiveTopology.TriangleList;
        }
    }
}