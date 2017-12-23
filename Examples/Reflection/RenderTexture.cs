using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;

namespace SharpDXExamples.Examples.Reflection {
    public class RenderTexture {
        public ShaderResourceView ShaderResourceView { get; private set; }

        Texture2D renderTargetTexture;
        RenderTargetView renderTargetView;

        public bool Initialize(Device device, int textureWidth, int textureHeight) {
            var textureDescription = new Texture2DDescription {
                Width = textureWidth,
                Height = textureHeight,
                MipLevels = 1,
                ArraySize = 1,
                Format = Format.R32G32B32A32_Float,
                SampleDescription = {
                    Count = 1
                },
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            };

            renderTargetTexture = new Texture2D(device, textureDescription);

            var renderTargetViewDescription = new RenderTargetViewDescription {
                Format = textureDescription.Format,
                Dimension = RenderTargetViewDimension.Texture2D,
                Texture2D = {
                    MipSlice = 0
                }
            };

            renderTargetView = new RenderTargetView(device, renderTargetTexture, renderTargetViewDescription);

            var shaderResourceViewDescription = new ShaderResourceViewDescription {
                Format = textureDescription.Format,
                Dimension = SharpDX.Direct3D.ShaderResourceViewDimension.Texture2D,
                Texture2D = {
                    MostDetailedMip = 0,
                    MipLevels = 1
                }
            };

            ShaderResourceView = new ShaderResourceView(device, renderTargetTexture, shaderResourceViewDescription);

            return true;
        }

        public void Shutdown() {
            ShaderResourceView.Dispose();
            renderTargetView.Dispose();
            renderTargetTexture.Dispose();
        }

        public void SetRenderTarget(DeviceContext deviceContext, DepthStencilView depthStencilView) {
            deviceContext.OutputMerger.SetRenderTargets(depthStencilView, renderTargetView);
        }

        public void ClearRenderTarget(DeviceContext deviceContext, DepthStencilView depthStencilView, Color4 color) {
            deviceContext.ClearRenderTargetView(renderTargetView, color);
            deviceContext.ClearDepthStencilView(depthStencilView, DepthStencilClearFlags.Depth, 1.0f, 0);
        }
    }
}