using System;
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
    [PostProcess(typeof(DitheringRenderer), PostProcessEvent.AfterStack, "SC Post Effects/Retro/Dithering", true)]
#endif
    [Serializable]
    public sealed class Dithering : PostProcessEffectSettings
    {
#if PPS
        [DisplayName("Pattern"), Tooltip("Note that the texture's filter mode (Point or Bilinear) greatly affects the behavior of the pattern")]
        public TextureParameter lut = new TextureParameter { value = null };

        [Range(0f, 1f), Tooltip("Fades the effect in or out")]
        public FloatParameter intensity = new FloatParameter { value = 0f };

        [Range(0f, 1f), Tooltip("The screen's luminance values control the density of the dithering matrix")]
        public FloatParameter luminanceThreshold = new FloatParameter { value = 0.5f };


        [Range(0f,2f), DisplayName("Tiling")]
        public FloatParameter tiling = new FloatParameter { value = 1f };

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            if (enabled.value)
            {
                if (intensity == 0) return false;
                return true;
            }

            return false;
        }
#endif
    }
}