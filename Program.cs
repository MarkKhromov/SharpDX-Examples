using CLI.Menu;
using SharpDXExamples.Examples.AlphaMapping;
using SharpDXExamples.Examples.AmbientLighting;
using SharpDXExamples.Examples.BuffersShadersHLSL;
using SharpDXExamples.Examples.BumpMapping;
using SharpDXExamples.Examples.ClippingPlanes;
using SharpDXExamples.Examples.Core;
using SharpDXExamples.Examples.DiffuseLighting;
using SharpDXExamples.Examples.DirectInput;
using SharpDXExamples.Examples.DirectSound;
using SharpDXExamples.Examples.Fog;
using SharpDXExamples.Examples.FontEngine;
using SharpDXExamples.Examples.FpsCpuUsageTimes;
using SharpDXExamples.Examples.FrustumCulling;
using SharpDXExamples.Examples.InitializingDirectX;
using SharpDXExamples.Examples.LightMaps;
using SharpDXExamples.Examples.ModelRendering3D;
using SharpDXExamples.Examples.MultitexturingAndTextureArrays;
using SharpDXExamples.Examples.Rendering2D;
using SharpDXExamples.Examples.RenderToTexture;
using SharpDXExamples.Examples.SpecularLighting;
using SharpDXExamples.Examples.SpecularMapping;
using SharpDXExamples.Examples.TextureTranslation;
using SharpDXExamples.Examples.Texturing;

namespace SharpDXExamples {
    class Program {
        static void Main(string[] args) {
            MenuBuilder.Create()
                .Item(InitializingDirectX.Title, ExampleManager.Run<InitializingDirectX>)
                .Item(BuffersShadersHLSL.Title, ExampleManager.Run<BuffersShadersHLSL>)
                .Item(Texturing.Title, ExampleManager.Run<Texturing>)
                .Item(DiffuseLighting.Title, ExampleManager.Run<DiffuseLighting>)
                .Item(ModelRendering3D.Title, ExampleManager.Run<ModelRendering3D>)
                .Item(AmbientLighting.Title, ExampleManager.Run<AmbientLighting>)
                .Item(SpecularLighting.Title, ExampleManager.Run<SpecularLighting>)
                .Item(Rendering2D.Title, ExampleManager.Run<Rendering2D>)
                .Item(FontEngine.Title, ExampleManager.Run<FontEngine>)
                .Item(DirectInput.Title, ExampleManager.Run<DirectInput>)
                .Item(DirectSound.Title, ExampleManager.Run<DirectSound>)
                .Item(FpsCpuUsageTimes.Title, ExampleManager.Run<FpsCpuUsageTimes>)
                .Item(FrustumCulling.Title, ExampleManager.Run<FrustumCulling>)
                .Item(MultitexturingAndTextureArrays.Title, ExampleManager.Run<MultitexturingAndTextureArrays>)
                .Item(LightMaps.Title, ExampleManager.Run<LightMaps>)
                .Item(AlphaMapping.Title, ExampleManager.Run<AlphaMapping>)
                .Item(BumpMapping.Title, ExampleManager.Run<BumpMapping>)
                .Item(SpecularMapping.Title, ExampleManager.Run<SpecularMapping>)
                .Item(RenderToTexture.Title, ExampleManager.Run<RenderToTexture>)
                .Item(Fog.Title, ExampleManager.Run<Fog>)
                .Item(ClippingPlanes.Title, ExampleManager.Run<ClippingPlanes>)
                .Item(TextureTranslation.Title, ExampleManager.Run<TextureTranslation>)
                .Show(DisplayMode.Linear)
            ;
        }
    }
}