using System;
using UnityEngine;
using UnityEngine.Rendering;

#if PPS
using UnityEngine.Rendering.PostProcessing;
using TextureParameter = UnityEngine.Rendering.PostProcessing.TextureParameter;
using BoolParameter = UnityEngine.Rendering.PostProcessing.BoolParameter;
using FloatParameter = UnityEngine.Rendering.PostProcessing.FloatParameter;
using IntParameter = UnityEngine.Rendering.PostProcessing.IntParameter;
using ColorParameter = UnityEngine.Rendering.PostProcessing.ColorParameter;
using Vector2Parameter = UnityEngine.Rendering.PostProcessing.Vector2Parameter;
using MinAttribute = UnityEngine.Rendering.PostProcessing.MinAttribute;
#endif

namespace SCPE
{
#if PPS
    internal sealed class CloudShadowsRenderer : PostProcessEffectRenderer<CloudShadows>
    {
       Shader shader;

        public override void Init()
        {
            shader = Shader.Find(ShaderNames.CloudShadows);
        }

        public override void Release()
        {
            base.Release();
        }

        public override void Render(PostProcessRenderContext context)
        {
            PropertySheet sheet = context.propertySheets.Get(shader);
            CommandBuffer cmd = context.command;

            Camera cam = context.camera;
            CloudShadows.isOrtho = context.camera.orthographic;

            var noiseTexture = settings.texture.value == null ? RuntimeUtilities.whiteTexture : settings.texture.value;
            sheet.properties.SetTexture("_NoiseTex", noiseTexture);

            var p = GL.GetGPUProjectionMatrix(cam.projectionMatrix, false);
            p[2, 3] = p[3, 2] = 0.0f;
            p[3, 3] = 1.0f;
            var clipToWorld = Matrix4x4.Inverse(p * cam.worldToCameraMatrix) * Matrix4x4.TRS(new Vector3(0, 0, -p[2, 2]), Quaternion.identity, Vector3.one);
            sheet.properties.SetMatrix("clipToWorld", clipToWorld);

            float cloudsSpeed = settings.speed * 0.1f;
            sheet.properties.SetVector("_CloudParams", new Vector4(settings.size * 0.01f, settings.direction.value.x * cloudsSpeed, settings.direction.value.y * cloudsSpeed, settings.density));

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
        }

        public override DepthTextureMode GetCameraFlags()
        {
            return DepthTextureMode.Depth;
        }
    }
#endif
}