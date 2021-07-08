using System;
using UnityEngine;

#if PPS
using UnityEngine.Rendering.PostProcessing;
using TextureParameter = UnityEngine.Rendering.PostProcessing.TextureParameter;
using BoolParameter = UnityEngine.Rendering.PostProcessing.BoolParameter;
using FloatParameter = UnityEngine.Rendering.PostProcessing.FloatParameter;
using IntParameter = UnityEngine.Rendering.PostProcessing.IntParameter;
using ColorParameter = UnityEngine.Rendering.PostProcessing.ColorParameter;
using MinAttribute = UnityEngine.Rendering.PostProcessing.MinAttribute;
#endif

namespace SCPE
{
#if PPS
    internal sealed class DitheringRenderer : PostProcessEffectRenderer<Dithering>
    {
        Shader shader;

        public override void Init()
        {
            shader = Shader.Find(ShaderNames.Dithering);
        }

        public override void Release()
        {
            base.Release();
        }

        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(shader);

            var lutTexture = settings.lut.value == null ? RuntimeUtilities.blackTexture : settings.lut.value;
            sheet.properties.SetTexture("_LUT", lutTexture);
            float luminanceThreshold = QualitySettings.activeColorSpace == ColorSpace.Gamma ? Mathf.LinearToGammaSpace(settings.luminanceThreshold.value) : settings.luminanceThreshold.value;

            Vector4 ditherParams = new Vector4(0f, settings.tiling, luminanceThreshold, settings.intensity);
            sheet.properties.SetVector("_Dithering_Coords", ditherParams);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
#endif
}