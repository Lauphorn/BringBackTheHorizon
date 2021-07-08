using System;
using UnityEngine;

#if PPS
using UnityEngine.Rendering.PostProcessing;
#endif

namespace SCPE
{
#if PPS
    internal sealed class ColorSplitRenderer : PostProcessEffectRenderer<ColorSplit>
    {
        Shader shader;

        public override void Init()
        {
            shader = Shader.Find(ShaderNames.ColorSplit);
        }

        public override void Release()
        {
            base.Release();
        }

        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(shader);

            sheet.properties.SetFloat("_Offset", settings.offset / 100);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, (int)settings.mode.value);
        }
    }
#endif
}