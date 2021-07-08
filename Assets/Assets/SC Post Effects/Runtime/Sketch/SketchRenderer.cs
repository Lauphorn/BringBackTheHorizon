using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if PPS
using UnityEngine.Rendering.PostProcessing;
using TextureParameter = UnityEngine.Rendering.PostProcessing.TextureParameter;
using BoolParameter = UnityEngine.Rendering.PostProcessing.BoolParameter;
using FloatParameter = UnityEngine.Rendering.PostProcessing.FloatParameter;
using IntParameter = UnityEngine.Rendering.PostProcessing.IntParameter;
using ColorParameter = UnityEngine.Rendering.PostProcessing.ColorParameter;
using MinAttribute = UnityEngine.Rendering.PostProcessing.MinAttribute;
#endif

namespace SCPE
{
#if PPS
    internal sealed class SketchRenderer : PostProcessEffectRenderer<Sketch>
    {
        Shader shader;

        public override void Init()
        {
            shader = Shader.Find(ShaderNames.Sketch);
        }

        public override void Release()
        {
            base.Release();
        }

        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(shader);

            var p = GL.GetGPUProjectionMatrix(context.camera.projectionMatrix, false);
            p[2, 3] = p[3, 2] = 0.0f;
            p[3, 3] = 1.0f;
            var clipToWorld = Matrix4x4.Inverse(p * context.camera.worldToCameraMatrix) * Matrix4x4.TRS(new Vector3(0, 0, -p[2, 2]), Quaternion.identity, Vector3.one);
            sheet.properties.SetMatrix("clipToWorld", clipToWorld);

            if (settings.strokeTex.value) sheet.properties.SetTexture("_Strokes", settings.strokeTex);

            sheet.properties.SetVector("_Params", new Vector4(0, (int)settings.blendMode.value, settings.intensity, ((int)settings.projectionMode.value == 1) ? settings.tiling * 0.1f : settings.tiling));
            sheet.properties.SetVector("_Brightness", settings.brightness);

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, (int)settings.projectionMode.value);
        }
    }
#endif
}