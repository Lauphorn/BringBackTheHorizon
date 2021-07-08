using System;
using UnityEngine;

#if PPS
using UnityEngine.Rendering.PostProcessing;
#endif

namespace SCPE
{
#if PPS
    internal sealed class OverlayRenderer : PostProcessEffectRenderer<Overlay>
    {
        Shader shader;

        public override void Init()
        {
            shader = Shader.Find(ShaderNames.Overlay);
        }

        public override void Release()
        {
            base.Release();
        }

        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(shader);

            if (settings.overlayTex.value) sheet.properties.SetTexture("_OverlayTex", settings.overlayTex);
            sheet.properties.SetVector("_Params", new Vector4(settings.intensity, Mathf.Pow(settings.tiling + 1, 2), settings.autoAspect ? 1f : 0f, (int)settings.blendMode.value));

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
#endif
}