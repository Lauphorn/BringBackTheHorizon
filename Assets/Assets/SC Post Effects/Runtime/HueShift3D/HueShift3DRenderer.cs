using System;
using UnityEngine;

#if PPS
using UnityEngine.Rendering.PostProcessing;
#endif

namespace SCPE
{
#if PPS
    internal sealed class HueShift3DRenderer : PostProcessEffectRenderer<HueShift3D>
    {
        Shader shader;

        public override void Init()
        {
            shader = Shader.Find(ShaderNames.HueShift3D);
        }

        public override void Release()
        {
            base.Release();
        }

        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(shader);

            HueShift3D.isOrtho = context.camera.orthographic;

            sheet.properties.SetVector("_Params", new Vector4(settings.speed, settings.size, settings.geoInfluence, settings.intensity));

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }

        public override DepthTextureMode GetCameraFlags()
        {
            return DepthTextureMode.DepthNormals;
        }
    }
#endif
}