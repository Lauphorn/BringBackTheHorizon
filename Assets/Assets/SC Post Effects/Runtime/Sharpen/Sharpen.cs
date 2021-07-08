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
    [PostProcess(typeof(SharpenRenderer), PostProcessEvent.AfterStack, "SC Post Effects/Image/Sharpen", true)]
#endif
    [Serializable]
    public sealed class Sharpen : PostProcessEffectSettings
    {
#if PPS
        [Range(0f, 1f), DisplayName("Amount"), Tooltip("Amount")]
        public FloatParameter amount = new FloatParameter { value = 0f };
        [Range(0.5f,2f)]
        public FloatParameter radius = new FloatParameter { value = 1f };

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