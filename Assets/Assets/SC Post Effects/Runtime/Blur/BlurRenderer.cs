using UnityEngine.Rendering;
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

namespace SCPE {
#if PPS
    internal sealed class BlurRenderer : PostProcessEffectRenderer<Blur>
    {
        Shader shader;
        int screenCopyID;

        enum Pass
        {
            Blend,
            Gaussian,
            Box
        }

        public override void Init()
        {
            shader = Shader.Find("Hidden/SC Post Effects/Blur");
            screenCopyID = Shader.PropertyToID("_ScreenCopyTexture");
        }

        public override void Render(PostProcessRenderContext context)
        {
            PropertySheet sheet = context.propertySheets.Get(shader);
            CommandBuffer cmd = context.command;

            cmd.GetTemporaryRT(screenCopyID, context.width, context.height, 0, FilterMode.Bilinear, context.sourceFormat);
            cmd.BlitFullscreenTriangle(context.source, screenCopyID, sheet, 0);

            // get two smaller RTs
            int blurredID = Shader.PropertyToID("_Temp1");
            int blurredID2 = Shader.PropertyToID("_Temp2");
            cmd.GetTemporaryRT(blurredID, context.screenWidth / settings.downscaling, context.screenHeight / settings.downscaling, 0, FilterMode.Bilinear);
            cmd.GetTemporaryRT(blurredID2, context.screenWidth / settings.downscaling, context.screenHeight / settings.downscaling, 0, FilterMode.Bilinear);

            // downsample screen copy into smaller RT, release screen RT
            cmd.Blit(screenCopyID, blurredID);
            cmd.ReleaseTemporaryRT(screenCopyID);

            int blurPass = (settings.mode == Blur.BlurMethod.Gaussian) ? (int)Pass.Gaussian : (int)Pass.Box;

            for (int i = 0; i < settings.iterations; i++)
            {
                //Safeguard for exploding GPUs
                if (settings.iterations > 12) return;

                // horizontal blur
                cmd.SetGlobalVector("_BlurOffsets", new Vector4(settings.amount / context.screenWidth, 0, 0, 0));
                context.command.BlitFullscreenTriangle(blurredID, blurredID2, sheet, blurPass);

                // vertical blur
                cmd.SetGlobalVector("_BlurOffsets", new Vector4(0, settings.amount / context.screenHeight, 0, 0));
                context.command.BlitFullscreenTriangle(blurredID2, blurredID, sheet, blurPass);

                //Double blur
                if (settings.highQuality)
                {
                    // horizontal blur
                    cmd.SetGlobalVector("_BlurOffsets", new Vector4(settings.amount / context.screenWidth, 0, 0, 0));
                    context.command.BlitFullscreenTriangle(blurredID, blurredID2, sheet, blurPass);

                    // vertical blur
                    cmd.SetGlobalVector("_BlurOffsets", new Vector4(0, settings.amount / context.screenHeight, 0, 0));
                    context.command.BlitFullscreenTriangle(blurredID2, blurredID, sheet, blurPass);
                }
            }

            // Render blurred texture in blend pass
            cmd.BlitFullscreenTriangle(blurredID, context.destination, sheet, (int)Pass.Blend);

            // release
            cmd.ReleaseTemporaryRT(blurredID);
            cmd.ReleaseTemporaryRT(blurredID2);
        }
    }
#endif
}