using System;
using UnityEngine;
#if PPS
using UnityEngine.Rendering.PostProcessing;
#endif

namespace SCPE
{
#if PPS
    [PostProcess(typeof(PixelizeRenderer), PostProcessEvent.BeforeStack, "SC Post Effects/Retro/Pixelize", true)]
#endif
    [Serializable]
    public sealed class Pixelize : PostProcessEffectSettings
    {
#if PPS
        [Range(0f, 1f), Tooltip("Amount")]
        public UnityEngine.Rendering.PostProcessing.FloatParameter amount = new UnityEngine.Rendering.PostProcessing.FloatParameter { value = 0f };

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            if (enabled.value)
            {
                if (amount == 0f) { return false; }
                return true;
            }

            return false;
        }
#endif
    }
}