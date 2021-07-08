using System;
using UnityEngine;

#if PPS
using UnityEngine.Rendering.PostProcessing;
#endif

namespace SCPE
{
#if PPS
    internal sealed class DoubleVisionRenderer : PostProcessEffectRenderer<DoubleVision>
    {
        Shader DoubleVisionShader;

        public override void Init()
        {
            DoubleVisionShader = Shader.Find(ShaderNames.DoubleVision);
        }

        public override void Release()
        {
            base.Release();
        }

        public override void Render(PostProcessRenderContext context)
        {

            var sheet = context.propertySheets.Get(DoubleVisionShader);

            sheet.properties.SetFloat("_Amount", settings.intensity / 10);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, (int)settings.mode.value);
        }
    }
#endif
}