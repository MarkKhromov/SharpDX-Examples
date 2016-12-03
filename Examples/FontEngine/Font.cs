using System;
using System.IO;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDXExamples.Examples.Core.Helpers;

namespace SharpDXExamples.Examples.FontEngine {
    public class Font {
        struct FontType {
            public float Left;
            public float Right;
            public int Size;
        }

        public ShaderResourceView ObjectTexture => texture.ObjectTexture;

        FontType[] font;
        Texture texture;

        public bool Initialize(Device device, string fontFileName, string textureFileName) {
            if(!LoadFontData(fontFileName))
                return false;

            if(!LoadTexture(device, textureFileName))
                return false;

            return true;
        }

        public void Shutdown() {
            texture.Shutdown();
        }

        public void BuildVertexArray(VertexType[] vertices, string sentence, float drawX, float drawY) {
            var index = 0;
            for(int i = 0; i < sentence.Length; i++) {
                var letter = sentence[i] - 32;
                if(letter == 0) {
                    drawX = drawX + 3.0f;
                } else {
                    vertices[index].Position = new Vector3(drawX, drawY, 0.0f);
                    vertices[index].Texture = new Vector2(font[letter].Left, 0.0f);
                    index++;

                    vertices[index].Position = new Vector3(drawX + font[letter].Size, drawY - 16, 0.0f);
                    vertices[index].Texture = new Vector2(font[letter].Right, 1.0f);
                    index++;

                    vertices[index].Position = new Vector3(drawX, drawY - 16, 0.0f);
                    vertices[index].Texture = new Vector2(font[letter].Left, 1.0f);
                    index++;

                    vertices[index].Position = new Vector3(drawX, drawY, 0.0f);
                    vertices[index].Texture = new Vector2(font[letter].Left, 0.0f);
                    index++;

                    vertices[index].Position = new Vector3(drawX + font[letter].Size, drawY, 0.0f);
                    vertices[index].Texture = new Vector2(font[letter].Right, 0.0f);
                    index++;

                    vertices[index].Position = new Vector3(drawX + font[letter].Size, drawY - 16, 0.0f);
                    vertices[index].Texture = new Vector2(font[letter].Right, 1.0f);
                    index++;

                    drawX += font[letter].Size + 1.0f;
                }
            }
        }

        bool LoadFontData(string fileName) {
            try {
                font = new FontType[95];
                using(var streamReader = File.OpenText(fileName)) {
                    for(int i = 0; i < 95; i++) {
                        var line = streamReader.ReadLine();
                        var data = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        font[i].Left = data[data.Length - 3].ParseFloat();
                        font[i].Right = data[data.Length - 2].ParseFloat();
                        font[i].Size = int.Parse(data[data.Length - 1]);
                    }
                }
            } catch { return false; }
            return true;
        }

        bool LoadTexture(Device device, string fileName) {
            texture = new Texture();
            if(!texture.Initialize(device, fileName))
                return false;

            return true;
        }
    }
}