using System;
using UnityEngine;
#if PPS
using UnityEngine.Rendering.PostProcessing;
#endif

namespace SCPE
{
#if PPS
    internal sealed class PixelizeRenderer : PostProcessEffectRenderer<Pixelize>
    {
        Shader shader;

        public override void Init()
        {
            shader = Shader.Find(ShaderNames.Pixelize);
        }

        public override void Release()
        {
            base.Release();
        }

        public override void Render(PostProcessRenderContext context)
        {

            var sheet = context.propertySheets.Get(shader);

            sheet.properties.SetFloat("_Resolution", settings.amount / 10f);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
#endif
}