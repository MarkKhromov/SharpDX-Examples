using System;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;

namespace SharpDXExamples.Examples.RenderToTexture {
    public class Direct3D {
        public Device Device { get; private set; }
        public DeviceContext DeviceContext { get; private set; }
        public Matrix Projection { get; private set; }
        public Matrix World { get; private set; }
        public Matrix Orthogonal { get; private set; }
        public string VideoCardDescription { get; private set; }
        public int VideoCardMemory { get; private set; }
        public DepthStencilView DepthStencilView { get; private set; }

        bool vSyncEnabled;
        SwapChain swapChain;
        RenderTargetView renderTargetView;
        Texture2D depthStencilBuffer;
        DepthStencilState depthStencilState;
        RasterizerState rasterizerState;
        DepthStencilState depthDisabledStencilState;

        public bool Initialize(int screenWidth, int screenHeight, bool vSync, IntPtr hwnd, bool fullScreen, float screenDepth, float screenNear) {
            try {
                vSyncEnabled = vSync;

                Rational refreshRate = new Rational(0, 0);

                var factory = new Factory1();
                var adapter = factory.Adapters[0];
                var adapterOutput = adapter.Outputs[0];

                var modes = adapterOutput.GetDisplayModeList(Format.R8G8B8A8_UNorm, DisplayModeEnumerationFlags.Interlaced);
                for(int i = 0; i < modes.Length; i++) {
                    if(modes[i].Width == screenWidth) {
                        if(modes[i].Height == screenHeight) {
                            refreshRate = modes[i].RefreshRate;
                        }
                    }
                }

                VideoCardMemory = adapter.Description.DedicatedVideoMemory / 1024 / 1024;
                VideoCardDescription = adapter.Description.Description;

                adapterOutput.Dispose();
                adapter.Dispose();
                factory.Dispose();

                var swapChainDescription = new SwapChainDescription {
                    BufferCount = 1,
                    ModeDescription = {
                        Width = screenWidth,
                        Height = screenHeight,
                        Format = Format.R8G8B8A8_UNorm,
                        RefreshRate = vSyncEnabled ? refreshRate : new Rational(0, 0),
                        ScanlineOrdering = DisplayModeScanlineOrder.Unspecified,
                        Scaling = DisplayModeScaling.Unspecified
                    },
                    Usage = Usage.RenderTargetOutput,
                    OutputHandle = hwnd,
                    SampleDescription = { Count = 1, Quality = 0 },
                    IsWindowed = !fullScreen,
                    SwapEffect = SwapEffect.Discard,
                    Flags = SwapChainFlags.None
                };

                var featureLevel = FeatureLevel.Level_11_0;

                Device device;
                Device.CreateWithSwapChain(
                    DriverType.Hardware,
                    DeviceCreationFlags.Debug,
                    new[] { featureLevel },
                    swapChainDescription,
                    out device,
                    out swapChain
                );
                Device = device;
                DeviceContext = device.ImmediateContext;

                using(var backBuffer = swapChain.GetBackBuffer<Texture2D>(0)) {
                    renderTargetView = new RenderTargetView(device, backBuffer);
                }

                var depthBufferDescription = new Texture2DDescription {
                    Width = screenWidth,
                    Height = screenHeight,
                    MipLevels = 1,
                    ArraySize = 1,
                    Format = Format.D24_UNorm_S8_UInt,
                    SampleDescription = new SampleDescription(1, 0),
                    Usage = ResourceUsage.Default,
                    BindFlags = BindFlags.DepthStencil,
                    CpuAccessFlags = CpuAccessFlags.None,
                    OptionFlags = ResourceOptionFlags.None
                };

                depthStencilBuffer = new Texture2D(device, depthBufferDescription);

                var depthStencilStateDescription = new DepthStencilStateDescription {
                    IsDepthEnabled = true,
                    DepthWriteMask = DepthWriteMask.All,
                    DepthComparison = Comparison.Less,
                    IsStencilEnabled = true,
                    StencilReadMask = 0xFF,
                    StencilWriteMask = 0xFF,
                    FrontFace = {
                        FailOperation = StencilOperation.Keep,
                        DepthFailOperation = StencilOperation.Increment,
                        PassOperation = StencilOperation.Keep,
                        Comparison = Comparison.Always
                    },
                    BackFace = {
                        FailOperation = StencilOperation.Keep,
                        DepthFailOperation = StencilOperation.Decrement,
                        PassOperation = StencilOperation.Keep,
                        Comparison = Comparison.Always
                    }
                };

                depthStencilState = new DepthStencilState(device, depthStencilStateDescription);

                var depthStencilViewDescription = new DepthStencilViewDescription {
                    Format = Format.D24_UNorm_S8_UInt,
                    Dimension = DepthStencilViewDimension.Texture2D,
                    Texture2D = {
                        MipSlice = 0
                    }
                };

                DepthStencilView = new DepthStencilView(device, depthStencilBuffer, depthStencilViewDescription);

                DeviceContext.OutputMerger.SetRenderTargets(DepthStencilView, renderTargetView);

                var rasterizerStateDescription = new RasterizerStateDescription {
                    IsAntialiasedLineEnabled = false,
                    CullMode = CullMode.Back,
                    DepthBias = 0,
                    DepthBiasClamp = 0.0f,
                    IsDepthClipEnabled = true,
                    FillMode = FillMode.Solid,
                    IsFrontCounterClockwise = false,
                    IsMultisampleEnabled = false,
                    IsScissorEnabled = false,
                    SlopeScaledDepthBias = 0.0f
                };

                rasterizerState = new RasterizerState(device, rasterizerStateDescription);

                DeviceContext.Rasterizer.State = rasterizerState;

                var viewport = new Viewport {
                    Width = screenWidth,
                    Height = screenHeight,
                    MinDepth = 0.0f,
                    MaxDepth = 1.0f,
                    X = 0,
                    Y = 0
                };

                DeviceContext.Rasterizer.SetViewport(viewport);

                var fieldOfView = MathUtil.Pi / 4.0f;
                var screenAspect = (float)screenWidth / screenHeight;
                Projection = Matrix.PerspectiveFovLH(fieldOfView, screenAspect, screenNear, screenDepth);
                World = Matrix.Identity;
                Orthogonal = Matrix.OrthoLH(screenWidth, screenHeight, screenNear, screenDepth);

                var depthDisabledStencilStateDescription = new DepthStencilStateDescription {
                    IsDepthEnabled = false,
                    DepthWriteMask = DepthWriteMask.All,
                    DepthComparison = Comparison.Less,
                    IsStencilEnabled = true,
                    StencilReadMask = 0xFF,
                    StencilWriteMask = 0xFF,
                    FrontFace = {
                        FailOperation = StencilOperation.Keep,
                        DepthFailOperation = StencilOperation.Increment,
                        PassOperation = StencilOperation.Keep,
                        Comparison = Comparison.Always
                    },
                    BackFace = {
                        FailOperation = StencilOperation.Keep,
                        DepthFailOperation = StencilOperation.Decrement,
                        PassOperation = StencilOperation.Keep,
                        Comparison = Comparison.Always
                    }
                };

                depthDisabledStencilState = new DepthStencilState(device, depthDisabledStencilStateDescription);
            } catch { return false; }
            return true;
        }

        public void Shutdown() {
            swapChain.SetFullscreenState(false, null);
            depthDisabledStencilState.Dispose();
            rasterizerState.Dispose();
            DepthStencilView.Dispose();
            depthStencilState.Dispose();
            depthStencilBuffer.Dispose();
            renderTargetView.Dispose();
            DeviceContext.Dispose();
            Device.Dispose();
            swapChain.Dispose();
        }

        public void BeginScene(Color4 color) {
            DeviceContext.ClearRenderTargetView(renderTargetView, color);
            DeviceContext.ClearDepthStencilView(DepthStencilView, DepthStencilClearFlags.Depth, 1.0f, 0);
        }

        public void EndScene() {
            swapChain.Present(vSyncEnabled ? 1 : 0, PresentFlags.None);
        }

        public void TurnZBufferOn() {
            DeviceContext.OutputMerger.SetDepthStencilState(depthStencilState, 1);
        }

        public void TurnZBufferOff() {
            DeviceContext.OutputMerger.SetDepthStencilState(depthDisabledStencilState, 1);
        }

        public void SetBackBufferRenderTarget() {
            DeviceContext.OutputMerger.SetRenderTargets(DepthStencilView, renderTargetView);
        }
    }
}