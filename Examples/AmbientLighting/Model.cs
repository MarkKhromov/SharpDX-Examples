using System;
using System.IO;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDXExamples.Examples.Core.Helpers;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace SharpDXExamples.Examples.AmbientLighting {
    public class Model {
        struct VertexType {
            public Vector3 Position;
            public Vector2 Texture;
            public Vector3 Normal;
        }

        struct ModelType {
            public float X;
            public float Y;
            public float Z;
            public float Tu;
            public float Tv;
            public float Nx;
            public float Ny;
            public float Nz;
        }

        public int IndexCount { get; private set; }
        public ShaderResourceView ObjectTexture => texture.ObjectTexture;

        Buffer vertexBuffer;
        Buffer indexBuffer;
        int vertexCount;
        Texture texture;
        ModelType[] model;

        public bool Initialize(Device device, string modelFileName, string textureFileName) {
            if(!LoadModel(modelFileName))
                return false;

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
                var vertices = new VertexType[vertexCount];
                var indices = new int[IndexCount];
                for(int i = 0; i < vertexCount; i++) {
                    vertices[i].Position = new Vector3(model[i].X, model[i].Y, model[i].Z);
                    vertices[i].Texture = new Vector2(model[i].Tu, model[i].Tv);
                    vertices[i].Normal = new Vector3(model[i].Nx, model[i].Ny, model[i].Nz);

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

        bool LoadModel(string fileName) {
            try {
                using(var streamReader = File.OpenText(fileName)) {
                    var line = streamReader.ReadLine();
                    vertexCount = int.Parse(line.Split(':')[1]);
                    IndexCount = vertexCount;
                    model = new ModelType[vertexCount];
                    streamReader.ReadLine();
                    streamReader.ReadLine();
                    streamReader.ReadLine();
                    for(int i = 0; i < vertexCount; i++) {
                        line = streamReader.ReadLine();
                        if(line != null) {
                            var data = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            model[i].X = data[0].ParseFloat();
                            model[i].Y = data[1].ParseFloat();
                            model[i].Z = data[2].ParseFloat();
                            model[i].Tu = data[3].ParseFloat();
                            model[i].Tv = data[4].ParseFloat();
                            model[i].Nx = data[5].ParseFloat();
                            model[i].Ny = data[6].ParseFloat();
                            model[i].Nz = data[7].ParseFloat();
                        }
                    }
                }
            } catch { return false; }
            return true;
        }
    }
}