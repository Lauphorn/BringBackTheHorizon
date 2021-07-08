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
    [PostProcess(typeof(KuwaharaRenderer), PostProcessEvent.AfterStack, "SC Post Effects/Stylized/Kuwahara", true)]
#endif
    [Serializable]
    public sealed class Kuwahara : PostProcessEffectSettings
    {
#if PPS
        public enum KuwaharaMode
        {
            Regular = 0,
            DepthFade = 1
        }

        [Serializable]
        public sealed class KuwaharaModeParam : ParameterOverride<KuwaharaMode> { }

        [DisplayName("Method"), Tooltip("Choose to apply the effect to the entire screen, or fade in/out over a distance")]
        public KuwaharaModeParam mode = new KuwaharaModeParam { value = KuwaharaMode.Regular };

        [Range(0, 8), DisplayName("Radius")]
        public IntParameter radius = new IntParameter { value = 0 };

        public BoolParameter invertFadeDistance = new BoolParameter { value = false };

        [DisplayName("Fade distance")]
        public FloatParameter fadeDistance = new FloatParameter { value = 1000f };

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            if (enabled.value)
            {
                if (radius == 0) { return false; }
                return true;
            }

            return false;
        }
#endif
    }
}