using System;
using UnityEngine;

#if PPS
using UnityEngine.Rendering.PostProcessing;
#endif

namespace SCPE
{
#if PPS
    internal sealed class KuwaharaRenderer : PostProcessEffectRenderer<Kuwahara>
    {
        Shader shader;

        public override void Init()
        {
            shader = Shader.Find(ShaderNames.Kuwahara);
        }

        public override void Release()
        {
            base.Release();
        }

        public override void Render(PostProcessRenderContext context)
        {
            if (context.camera.orthographic) settings.mode.value = Kuwahara.KuwaharaMode.Regular;

            var sheet = context.propertySheets.Get(shader);

            sheet.properties.SetFloat("_Radius", (float)settings.radius);

            sheet.properties.SetFloat("_FadeDistance", settings.fadeDistance);
            sheet.properties.SetVector("_DistanceParams", new Vector4(settings.fadeDistance, (settings.invertFadeDistance) ? 1 : 0, 0, 0));

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, (int)settings.mode.value);
        }

        public override DepthTextureMode GetCameraFlags()
        {
            if ((int)settings.mode.value == 1)
            {
                return DepthTextureMode.Depth;
            }
            else
            {
                return DepthTextureMode.None;
            }
        }
    }
#endif
}
