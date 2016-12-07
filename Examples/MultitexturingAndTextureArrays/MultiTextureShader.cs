using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;

namespace SharpDXExamples.Examples.MultitexturingAndTextureArrays {
    public class MultiTextureShader {
        struct MatrixBufferType {
            public Matrix World;
            public Matrix View;
            public Matrix Projection;
        }

        VertexShader vertexShader;
        PixelShader pixelShader;
        InputLayout inputLayout;
        Buffer matrixBuffer;
        SamplerState samplerState;

        public bool Initialize(Device device) {
            if(!InitializeShader(device, @"Examples\MultitexturingAndTextureArrays\Shaders\MultiTexture.vs", @"Examples\MultitexturingAndTextureArrays\Shaders\MultiTexture.ps"))
                return false;

            return true;
        }

        public void Shutdown() {
            ShutdownShader();
        }

        public bool Render(DeviceContext deviceContext, int indexCount, Matrix world, Matrix view, Matrix projection, ShaderResourceView[] textureArray) {
            if(!SetShaderParameters(deviceContext, world, view, projection, textureArray))
                return false;

            RenderShader(deviceContext, indexCount);

            return true;
        }

        bool InitializeShader(Device device, string vsFileName, string psFileName) {
            try {
                var compileVertexShaderResult = ShaderBytecode.CompileFromFile(vsFileName, "MultiTextureVertexShader", "vs_5_0", ShaderFlags.EnableStrictness);
                vertexShader = new VertexShader(device, compileVertexShaderResult.Bytecode);

                var compilePixelShaderResult = ShaderBytecode.CompileFromFile(psFileName, "MultiTexturePixelShader", "ps_5_0", ShaderFlags.EnableStrictness);
                pixelShader = new PixelShader(device, compilePixelShaderResult.Bytecode);

                var inputElements = new[] {
                    new InputElement("POSITION", 0, Format.R32G32B32_Float, 0, 0, InputClassification.PerVertexData, 0),
                    new InputElement("TEXCOORD", 0, Format.R32G32_Float, 12, 0, InputClassification.PerVertexData, 0)
                };

                inputLayout = new InputLayout(device, compileVertexShaderResult.Bytecode, inputElements);

                compileVertexShaderResult.Dispose();
                compilePixelShaderResult.Dispose();

                var matrixBufferDescription = new BufferDescription {
                    Usage = ResourceUsage.Default,
                    SizeInBytes = Utilities.SizeOf<MatrixBufferType>(),
                    BindFlags = BindFlags.ConstantBuffer,
                    CpuAccessFlags = CpuAccessFlags.None,
                    OptionFlags = ResourceOptionFlags.None,
                    StructureByteStride = 0
                };

                matrixBuffer = Buffer.Create(device, new[] { new MatrixBufferType() }, matrixBufferDescription);

                var samplerStateDescription = new SamplerStateDescription {
                    Filter = Filter.MinMagMipLinear,
                    AddressU = TextureAddressMode.Wrap,
                    AddressV = TextureAddressMode.Wrap,
                    AddressW = TextureAddressMode.Wrap,
                    MipLodBias = 0.0f,
                    MaximumAnisotropy = 1,
                    ComparisonFunction = Comparison.Always,
                    BorderColor = {
                        A = 0,
                        R = 0,
                        G = 0,
                        B = 0
                    },
                    MinimumLod = 0,
                    MaximumLod = float.MaxValue
                };

                samplerState = new SamplerState(device, samplerStateDescription);
            } catch { return false; }
            return true;
        }

        void ShutdownShader() {
            samplerState.Dispose();
            matrixBuffer.Dispose();
            inputLayout.Dispose();
            pixelShader.Dispose();
            vertexShader.Dispose();
        }

        bool SetShaderParameters(DeviceContext deviceContext, Matrix world, Matrix view, Matrix projection, ShaderResourceView[] textureArray) {
            try {
                var matrixBufferType = new MatrixBufferType {
                    World = Matrix.Transpose(world),
                    View = Matrix.Transpose(view),
                    Projection = Matrix.Transpose(projection)
                };

                deviceContext.UpdateSubresource(ref matrixBufferType, matrixBuffer);
                deviceContext.VertexShader.SetConstantBuffer(0, matrixBuffer);
                deviceContext.PixelShader.SetShaderResources(0, textureArray);
            } catch { return false; }
            return true;
        }

        void RenderShader(DeviceContext deviceContext, int indexCount) {
            deviceContext.InputAssembler.InputLayout = inputLayout;
            deviceContext.VertexShader.Set(vertexShader);
            deviceContext.PixelShader.Set(pixelShader);
            deviceContext.PixelShader.SetSampler(0, samplerState);
            deviceContext.DrawIndexed(indexCount, 0, 0);
        }
    }
}