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
    [PostProcess(typeof(SpeedLinesRenderer), PostProcessEvent.AfterStack, "SC Post Effects/Screen/Speed Lines", true)]
#endif
    [Serializable]
    public sealed class SpeedLines : PostProcessEffectSettings
    {
#if PPS
        [Range(0f, 1f)]
        public FloatParameter intensity = new FloatParameter { value = 0f };
        [Range(0f, 1f)]

        [Tooltip("Determines the radial tiling of the noise texture")]
        public FloatParameter size = new FloatParameter { value = 0.5f };

        [Range(0f, 1f)]
        public FloatParameter falloff = new FloatParameter { value = 0.25f };

        [Tooltip("Assign any grayscale texture with a vertically repeating pattern and a falloff from left to right")]
        public TextureParameter noiseTex = new TextureParameter { value = null };

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            if (enabled.value)
            {
                if (intensity == 0 || noiseTex.value == null) { return false; }
                return true;
            }

            return false;
        }
#endif
    }
}