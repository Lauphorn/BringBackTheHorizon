using System;
using UnityEngine;
#if PPS
using UnityEngine.Rendering.PostProcessing;
#endif

namespace SCPE
{
#if PPS
    [PostProcess(typeof(KaleidoscopeRenderer), PostProcessEvent.AfterStack, "SC Post Effects/Misc/Kaleidoscope", true)]
#endif
    [Serializable]
    public sealed class Kaleidoscope : PostProcessEffectSettings
    {
#if PPS
        [Range(0f, 10f), Tooltip("The number of times the screen is split up")]
        public UnityEngine.Rendering.PostProcessing.IntParameter splits = new UnityEngine.Rendering.PostProcessing.IntParameter { value = 0 };

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            if (enabled.value)
            {
                if (splits == 0) { return false; }
                return true;
            }

            return false;
        }
#endif
    }
}