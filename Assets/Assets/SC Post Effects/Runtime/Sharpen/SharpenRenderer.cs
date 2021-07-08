using System;
using UnityEngine;

#if PPS
using UnityEngine.Rendering.PostProcessing;
#endif

namespace SCPE
{
#if PPS
    internal sealed class SharpenRenderer : PostProcessEffectRenderer<Sharpen>
    {
        Shader shader;

        public override void Init()
        {
            shader = Shader.Find(ShaderNames.Sharpen);
        }

        public override void Release()
        {
            base.Release();
        }

        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(shader);

            sheet.properties.SetVector("_Params", new Vector4(settings.amount, settings.radius, 0f, 0f));

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
#endif
}