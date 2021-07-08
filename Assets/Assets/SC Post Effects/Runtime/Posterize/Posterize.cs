using System;
using UnityEngine;
#if PPS
using UnityEngine.Rendering.PostProcessing;
#endif

namespace SCPE
{
#if PPS
    [PostProcess(typeof(PosterizeRenderer), PostProcessEvent.BeforeStack, "SC Post Effects/Retro/Posterize", true)]
#endif
    [Serializable]
    public sealed class Posterize : PostProcessEffectSettings
    {
#if PPS
        [Range(0f, 1f)]
        public UnityEngine.Rendering.PostProcessing.FloatParameter amount = new UnityEngine.Rendering.PostProcessing.FloatParameter { value = 0f };

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