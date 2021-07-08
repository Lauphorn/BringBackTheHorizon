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
using MinAttribute = UnityEngine.Rendering.PostProcessing.MinAttribute;
#endif

namespace SCPE
{
#if PPS
    [PostProcess(typeof(RadialBlurRenderer), PostProcessEvent.AfterStack, "SC Post Effects/Blurring/Radial Blur", true)]
#endif
    [Serializable]
    public sealed class RadialBlur : PostProcessEffectSettings
    {
#if PPS
        [Range(0f, 1f)]
        public FloatParameter amount = new FloatParameter { value = 0f };
        [Range(3, 12)]
        public IntParameter iterations = new IntParameter { value = 6 };

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            if (enabled.value)
            {
                if (amount == 0) { return false; }
                return true;
            }

            return false;
        }
#endif
    }
}