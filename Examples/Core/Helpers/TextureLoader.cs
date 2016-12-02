using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.WIC;
using Device = SharpDX.Direct3D11.Device;

namespace SharpDXExamples.Examples.Core.Helpers {
    public static class TextureLoader {
        public static Texture2D LoadTexture2D(Device device, string fileName) {
            var factory = new ImagingFactory2();
            var bitmapSource = LoadBitmap(factory, fileName);
            factory.Dispose();
            int stride = bitmapSource.Size.Width * 4;
            using(var buffer = new DataStream(bitmapSource.Size.Height * stride, true, true)) {
                bitmapSource.CopyPixels(stride, buffer);
                var texture = new Texture2D(device, new Texture2DDescription {
                    Width = bitmapSource.Size.Width,
                    Height = bitmapSource.Size.Height,
                    ArraySize = 1,
                    BindFlags = BindFlags.ShaderResource,
                    Usage = ResourceUsage.Immutable,
                    CpuAccessFlags = CpuAccessFlags.None,
                    Format = Format.R8G8B8A8_UNorm,
                    MipLevels = 1,
                    OptionFlags = ResourceOptionFlags.None,
                    SampleDescription = new SampleDescription(1, 0)
                }, new DataRectangle(buffer.DataPointer, stride));
                bitmapSource.Dispose();
                return texture;
            }
        }

        static BitmapSource LoadBitmap(ImagingFactory2 factory, string fileName) {
            var bitmapDecoder = new BitmapDecoder(
                factory,
                fileName,
                DecodeOptions.CacheOnDemand
            );

            var formatConverter = new FormatConverter(factory);

            formatConverter.Initialize(
                bitmapDecoder.GetFrame(0),
                PixelFormat.Format32bppPRGBA,
                BitmapDitherType.None,
                null,
                0.0,
                BitmapPaletteType.Custom
            );

            return formatConverter;
        }
    }
}