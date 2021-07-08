using System;
using UnityEngine;

#if PPS
using UnityEngine.Rendering.PostProcessing;
#endif

namespace SCPE
{
#if PPS
    internal sealed class SpeedLinesRenderer : PostProcessEffectRenderer<SpeedLines>
    {
        Shader shader;

        public override void Init()
        {
            shader = Shader.Find(ShaderNames.SpeedLines);
        }

        public override void Release()
        {
            base.Release();
        }

        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(shader);

            float falloff = 2f + (settings.falloff - 0.0f) * (16.0f - 2f) / (1.0f - 0.0f);
            sheet.properties.SetVector("_Params", new Vector4(settings.intensity, falloff, settings.size * 2, 0));
            if (settings.noiseTex.value) sheet.properties.SetTexture("_NoiseTex", settings.noiseTex);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
#endif
}