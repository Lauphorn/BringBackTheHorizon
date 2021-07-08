#undef LWRP //Currently broken

#if PPS 
using UnityEngine.Rendering.PostProcessing;
#endif

using UnityEngine;
using UnityEngine.Rendering;

namespace SCPE
{
#if PPS
    internal sealed class EdgeDetectionRenderer : PostProcessEffectRenderer<EdgeDetection>
    {
        Shader shader;
#if LWRP
        int depthNormalsID;
#endif
        public override void Init()
        {
            shader = Shader.Find(ShaderNames.EdgeDetection);
#if LWRP
            depthNormalsID = Shader.PropertyToID("_CameraDepthNormalsTexture");
#endif
        }

        public override void Release()
        {
            base.Release();
        }

        public override void Render(PostProcessRenderContext context)
        {
            var sheet = context.propertySheets.Get(shader);
            CommandBuffer cmd = context.command;

#if LWRP
            if (RenderPipelineManager.currentPipeline != null)
            {
                //Reconstruct geometry normals from depth texture
                cmd.GetTemporaryRT(depthNormalsID, context.width, context.height, 0, FilterMode.Bilinear, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
                cmd.Blit(BuiltinRenderTextureType.Depth, depthNormalsID);
            }
#endif

            Vector2 sensitivity = new Vector2(settings.sensitivityDepth, settings.sensitivityNormals);
            sheet.properties.SetVector("_Sensitivity", sensitivity);
            sheet.properties.SetFloat("_BackgroundFade", (settings.debug) ? 1f : 0f);
            sheet.properties.SetFloat("_EdgeSize", settings.edgeSize);
            sheet.properties.SetFloat("_Exponent", settings.edgeExp);
            sheet.properties.SetFloat("_Threshold", settings.lumThreshold);
            sheet.properties.SetColor("_EdgeColor", settings.edgeColor);
            sheet.properties.SetFloat("_EdgeOpacity", settings.edgeOpacity);

            float fadeDist = (context.camera.orthographic) ? settings.fadeDistance * (float)1e-10 : settings.fadeDistance;
            sheet.properties.SetVector("_DistanceParams", new Vector4(fadeDist, (settings.invertFadeDistance) ? 1 : 0, 0, 0));

            sheet.properties.SetVector("_SobelParams", new Vector4((settings.sobelThin) ? 1 : 0, 0, 0, 0));

            context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, (int)settings.mode.value);
        }

        public override DepthTextureMode GetCameraFlags()
        {
            return DepthTextureMode.DepthNormals;
        }
    }
#endif
}