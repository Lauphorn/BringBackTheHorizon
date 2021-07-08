using System;
using UnityEngine;
#if PPS
using UnityEngine.Rendering.PostProcessing;
#endif

namespace SCPE
{
#if PPS
    internal sealed class PosterizeRenderer : PostProcessEffectRenderer<Posterize>
    {
        Shader shader;

        public override void Init()
        {
            shader = Shader.Find(ShaderNames.Posterize);
        }

        public override void Release()
        {
            base.Release();
        }

        public override void Render(PostProcessRenderContext context)
        {

            var sheet = context.propertySheets.Get(shader);

            sheet.properties.SetFloat("_Depth", (1 - settings.amount) * 8f);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
#endif
}