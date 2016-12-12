using System;
using System.IO;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDXExamples.Examples.Core.Helpers;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace SharpDXExamples.Examples.BumpMapping {
    public class Model {
        struct VertexType {
            public Vector3 Position;
            public Vector2 Texture;
            public Vector3 Normal;
            public Vector3 Tangent;
            public Vector3 Binormal;
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
            public float Tx;
            public float Ty;
            public float Tz;
            public float Bx;
            public float By;
            public float Bz;
        }

        struct TempVertexType {
            public float X;
            public float Y;
            public float Z;
            public float Tu;
            public float Tv;
            public float Nx;
            public float Ny;
            public float Nz;
        }

        struct VectorType {
            public float X;
            public float Y;
            public float Z;
        }

        public int IndexCount { get; private set; }
        public ShaderResourceView[] ObjectTextures => textureArray.ObjectTextures;

        Buffer vertexBuffer;
        Buffer indexBuffer;
        int vertexCount;
        TextureArray textureArray;
        ModelType[] model;

        public bool Initialize(Device device, string modelFileName, string[] textureFileNames) {
            if(!LoadModel(modelFileName))
                return false;

            CalculateModelVectors();

            if(!InitializeBuffers(device))
                return false;

            if(!LoadTexture(device, textureFileNames))
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
                    vertices[i].Tangent = new Vector3(model[i].Tx, model[i].Ty, model[i].Tz);
                    vertices[i].Binormal = new Vector3(model[i].Bx, model[i].By, model[i].Bz);

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

        bool LoadTexture(Device device, string[] fileNames) {
            textureArray = new TextureArray();
            if(!textureArray.Initialize(device, fileNames))
                return false;

            return true;
        }

        void ReleaseTexture() {
            textureArray.Shutdown();
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

        void CalculateModelVectors() {
            var faceCount = vertexCount / 3;
            for(int i = 0, index = 0; i < faceCount; i++) {
                var tangent = new VectorType();
                var binormal = new VectorType();
                var normal = new VectorType();
                var vertex1 = new TempVertexType {
                    X = model[index].X,
                    Y = model[index].Y,
                    Z = model[index].Z,
                    Tu = model[index].Tu,
                    Tv = model[index].Tv,
                    Nx = model[index].Nx,
                    Ny = model[index].Ny,
                    Nz = model[index].Nz
                };
                index++;

                var vertex2 = new TempVertexType {
                    X = model[index].X,
                    Y = model[index].Y,
                    Z = model[index].Z,
                    Tu = model[index].Tu,
                    Tv = model[index].Tv,
                    Nx = model[index].Nx,
                    Ny = model[index].Ny,
                    Nz = model[index].Nz
                };
                index++;

                var vertex3 = new TempVertexType {
                    X = model[index].X,
                    Y = model[index].Y,
                    Z = model[index].Z,
                    Tu = model[index].Tu,
                    Tv = model[index].Tv,
                    Nx = model[index].Nx,
                    Ny = model[index].Ny,
                    Nz = model[index].Nz
                };
                index++;

                CalculateTangentBinormal(vertex1, vertex2, vertex3, ref tangent, ref binormal);
                CalculateNormal(tangent, binormal, ref normal);

                model[index - 1].Nx = normal.X;
                model[index - 1].Ny = normal.Y;
                model[index - 1].Nz = normal.Z;
                model[index - 1].Tx = tangent.X;
                model[index - 1].Ty = tangent.Y;
                model[index - 1].Tz = tangent.Z;
                model[index - 1].Bx = binormal.X;
                model[index - 1].By = binormal.Y;
                model[index - 1].Bz = binormal.Z;

                model[index - 2].Nx = normal.X;
                model[index - 2].Ny = normal.Y;
                model[index - 2].Nz = normal.Z;
                model[index - 2].Tx = tangent.X;
                model[index - 2].Ty = tangent.Y;
                model[index - 2].Tz = tangent.Z;
                model[index - 2].Bx = binormal.X;
                model[index - 2].By = binormal.Y;
                model[index - 2].Bz = binormal.Z;

                model[index - 3].Nx = normal.X;
                model[index - 3].Ny = normal.Y;
                model[index - 3].Nz = normal.Z;
                model[index - 3].Tx = tangent.X;
                model[index - 3].Ty = tangent.Y;
                model[index - 3].Tz = tangent.Z;
                model[index - 3].Bx = binormal.X;
                model[index - 3].By = binormal.Y;
                model[index - 3].Bz = binormal.Z;
            }
        }

        void CalculateTangentBinormal(TempVertexType vertex1, TempVertexType vertex2, TempVertexType vertex3, ref VectorType tangent, ref VectorType binormal) {
            var vector1 = new Vector3 {
                X = vertex2.X - vertex1.X,
                Y = vertex2.Y - vertex1.Y,
                Z = vertex2.Z - vertex1.Z
            };
            var vector2 = new Vector3 {
                X = vertex3.X - vertex1.X,
                Y = vertex3.Y - vertex1.Y,
                Z = vertex3.Z - vertex1.Z
            };
            var tuVector = new Vector2 {
                X = vertex2.Tu - vertex1.Tu,
                Y = vertex3.Tu - vertex1.Tu
            };
            var tvVector = new Vector2 {
                X = vertex2.Tv - vertex1.Tv,
                Y = vertex3.Tv - vertex1.Tv
            };

            var denominator = 1.0f / (tuVector.X * tvVector.Y - tuVector.Y * tvVector.X);

            tangent.X = (tvVector.Y * vector1.X - tvVector.X * vector2.X) * denominator;
            tangent.Y = (tvVector.Y * vector1.Y - tvVector.X * vector2.Y) * denominator;
            tangent.Z = (tvVector.Y * vector1.Z - tvVector.X * vector2.Z) * denominator;

            binormal.X = (tuVector.X * vector2.X - tuVector.Y * vector1.X) * denominator;
            binormal.Y = (tuVector.X * vector2.Y - tuVector.Y * vector1.Y) * denominator;
            binormal.Z = (tuVector.X * vector2.Z - tuVector.Y * vector1.Z) * denominator;

            var tangentLength = new Vector3(tangent.X, tangent.Y, tangent.Z).Length();

            tangent.X /= tangentLength;
            tangent.Y /= tangentLength;
            tangent.Z /= tangentLength;

            var binormalLength = new Vector3(binormal.X, binormal.Y, binormal.Z).Length();

            binormal.X /= binormalLength;
            binormal.Y /= binormalLength;
            binormal.Z /= binormalLength;
        }

        void CalculateNormal(VectorType tangent, VectorType binormal, ref VectorType normal) {
            normal.X = tangent.Y * binormal.Z - tangent.Z * binormal.Y;
            normal.Y = tangent.Z * binormal.X - tangent.X * binormal.Z;
            normal.Z = tangent.X * binormal.Y - tangent.Y * binormal.X;

            var normalLength = new Vector3(normal.X, normal.Y, normal.Z).Length();

            normal.X /= normalLength;
            normal.Y /= normalLength;
            normal.Z /= normalLength;
        }
    }
}