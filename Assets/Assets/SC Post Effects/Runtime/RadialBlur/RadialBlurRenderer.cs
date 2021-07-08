using System;
using UnityEngine;
using UnityEngine.Rendering;

#if PPS
using UnityEngine.Rendering.PostProcessing;
#endif

namespace SCPE
{
#if PPS
    internal sealed class RadialBlurRenderer : PostProcessEffectRenderer<RadialBlur>
    {
        Shader shader;

        public override void Init()
        {
            shader = Shader.Find(ShaderNames.RadialBlur);
        }

        public override void Release()
        {
            base.Release();
        }

        public override void Render(PostProcessRenderContext context)
        {
            PropertySheet sheet = context.propertySheets.Get(shader);
            CommandBuffer cmd = context.command;

            sheet.properties.SetFloat("_Amount", settings.amount / 50);
            sheet.properties.SetFloat("_Iterations", settings.iterations);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
#endif
}