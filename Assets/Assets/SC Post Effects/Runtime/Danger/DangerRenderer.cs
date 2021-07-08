using System;
using UnityEngine;
#if PPS
using UnityEngine.Rendering.PostProcessing;
using TextureParameter = UnityEngine.Rendering.PostProcessing.TextureParameter;
using ColorParameter = UnityEngine.Rendering.PostProcessing.ColorParameter;
using FloatParameter = UnityEngine.Rendering.PostProcessing.FloatParameter;
#endif

namespace SCPE
{
#if PPS
    internal sealed class DangerRenderer : PostProcessEffectRenderer<Danger>
    {
        Shader shader;

        public override void Init()
        {
            shader = Shader.Find(ShaderNames.Danger);
        }

        public override void Release()
        {
            base.Release();
        }

        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(shader);

            sheet.properties.SetVector("_Params", new Vector4(settings.intensity, settings.size, 0, 0));
            sheet.properties.SetColor("_Color", settings.color);
            var overlayTexture = settings.overlayTex.value == null ? RuntimeUtilities.blackTexture : settings.overlayTex.value;
            sheet.properties.SetTexture("_Overlay", overlayTexture);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
#endif
}