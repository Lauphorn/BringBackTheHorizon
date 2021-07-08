using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if PPS
using UnityEngine.Rendering.PostProcessing;
#endif

namespace SCPE
{
#if PPS
    internal sealed class ColorizeRenderer : PostProcessEffectRenderer<Colorize>
    {
        Shader shader;

        public override void Init()
        {
            shader = Shader.Find(ShaderNames.Colorize);
        }

        public override void Release()
        {
            base.Release();
        }

        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(shader);

            if (settings.colorRamp.value) sheet.properties.SetTexture("_ColorRamp", settings.colorRamp);
            sheet.properties.SetFloat("_Intensity", settings.intensity);
            sheet.properties.SetFloat("_BlendMode", (int)settings.mode.value);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }
    }
#endif
}