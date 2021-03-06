﻿using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;

namespace SharpDXExamples.Examples.FrustumCulling {
    public class Text {
        struct SentenceType {
            public Buffer VertexBuffer;
            public Buffer IndexBuffer;
            public int VertexCount;
            public int IndexCount;
            public int MaxLength;
            public float Red;
            public float Green;
            public float Blue;
        }

        Font font;
        FontShader fontShader;
        int screenWidth;
        int screenHeight;
        Matrix baseViewMatrix;
        SentenceType sentence;

        public bool Initialize(Device device, DeviceContext deviceContext, int screenWidth, int screenHeight, Matrix baseViewMatrix) {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;
            this.baseViewMatrix = baseViewMatrix;

            font = new Font();
            if(!font.Initialize(device, @"Examples\FrustumCulling\Data\Font.data", @"Examples\FrustumCulling\Data\Font.png"))
                return false;

            fontShader = new FontShader();
            if(!fontShader.Initialize(device))
                return false;

            if(!InitializeSentence(out sentence, 16, device))
                return false;

            if(!UpdateSentence(ref sentence, "Hello", 100, 100, 1.0f, 1.0f, 1.0f, deviceContext))
                return false;

            return true;
        }

        public void Shutdown() {
            ReleaseSentence(sentence);
            fontShader.Shutdown();
            font.Shutdown();
        }

        public bool Render(DeviceContext deviceContext, Matrix world, Matrix orthogonal) {
            if(!RenderSentence(deviceContext, sentence, world, orthogonal))
                return false;

            return true;
        }

        public bool SetRenderCount(int renderCount, DeviceContext deviceContext) {
            if(!UpdateSentence(ref sentence, $"Render count: {renderCount}", 20, 20, 1.0f, 1.0f, 1.0f, deviceContext))
                return false;

            return true;
        }

        bool InitializeSentence(out SentenceType sentence, int maxLength, Device device) {
            sentence = new SentenceType() {
                MaxLength = maxLength,
                VertexCount = maxLength * 6
            };
            sentence.IndexCount = sentence.VertexCount;

            try {
                var vertices = new VertexType[sentence.VertexCount];
                var indices = new int[sentence.IndexCount];

                for(int i = 0; i < sentence.IndexCount; i++) {
                    indices[i] = i;
                }

                var vertexBufferDescription = new BufferDescription {
                    Usage = ResourceUsage.Default,
                    SizeInBytes = Utilities.SizeOf<VertexType>() * sentence.VertexCount,
                    BindFlags = BindFlags.VertexBuffer,
                    CpuAccessFlags = CpuAccessFlags.None,
                    OptionFlags = ResourceOptionFlags.None,
                    StructureByteStride = 0
                };

                sentence.VertexBuffer = Buffer.Create(device, vertices, vertexBufferDescription);

                var indexBufferDescription = new BufferDescription {
                    Usage = ResourceUsage.Default,
                    SizeInBytes = Utilities.SizeOf<int>() * sentence.IndexCount,
                    BindFlags = BindFlags.IndexBuffer,
                    CpuAccessFlags = CpuAccessFlags.None,
                    OptionFlags = ResourceOptionFlags.None,
                    StructureByteStride = 0
                };

                sentence.IndexBuffer = Buffer.Create(device, indices, indexBufferDescription);
            } catch { return false; }
            return true;
        }

        bool UpdateSentence(ref SentenceType sentence, string text, int positionX, int positionY, float red, float green, float blue, DeviceContext deviceContext) {
            try {
                sentence.Red = red;
                sentence.Green = green;
                sentence.Blue = blue;

                if(text.Length > sentence.MaxLength)
                    return false;

                var vertices = new VertexType[sentence.VertexCount];

                var drawX = (float)-screenWidth / 2 + positionX;
                var drawY = (float)screenHeight / 2 - positionY;

                font.BuildVertexArray(vertices, text, drawX, drawY);

                deviceContext.UpdateSubresource(vertices, sentence.VertexBuffer);
            } catch { return false; }
            return true;
        }

        void ReleaseSentence(SentenceType sentence) {
            sentence.VertexBuffer.Dispose();
            sentence.IndexBuffer.Dispose();
        }

        bool RenderSentence(DeviceContext deviceContext, SentenceType sentence, Matrix world, Matrix orthogonal) {
            deviceContext.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(sentence.VertexBuffer, Utilities.SizeOf<VertexType>(), 0));
            deviceContext.InputAssembler.SetIndexBuffer(sentence.IndexBuffer, Format.R32_UInt, 0);
            deviceContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

            var pixelColor = new Color4(sentence.Red, sentence.Green, sentence.Blue, 1.0f);

            if(!fontShader.Render(deviceContext, sentence.IndexCount, world, baseViewMatrix, orthogonal, font.ObjectTexture, pixelColor))
                return false;

            return true;
        }
    }
}