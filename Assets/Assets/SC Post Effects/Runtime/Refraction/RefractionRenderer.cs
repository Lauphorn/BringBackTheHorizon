using System;
using UnityEngine;

#if PPS
using UnityEngine.Rendering.PostProcessing;
#endif

namespace SCPE
{
#if PPS
    internal sealed class RefractionRenderer : PostProcessEffectRenderer<Refraction>
    {
        Shader shader;

        public override void Init()
        {
            shader = Shader.Find(ShaderNames.Refraction);
        }

        public override void Release()
        {
            base.Release();
        }

        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(shader);

            sheet.properties.SetFloat("_Amount", settings.amount);
            if (settings.refractionTex.value) sheet.properties.SetTexture("_RefractionTex", settings.refractionTex);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, (settings.convertNormalMap) ? 1 : 0);
        }
    }
#endif
}