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
    [PostProcess(typeof(LUTRenderer), PostProcessEvent.AfterStack, "SC Post Effects/Image/Color Grading LUT", true)]
#endif
    [Serializable]
    public sealed class LUT : PostProcessEffectSettings
    {
#if PPS
        public enum Mode
        {
            Single = 0,
            DistanceBased = 1,
        }

        [Serializable]
        public sealed class ModeParam : ParameterOverride<Mode> { }

        [DisplayName("Mode"), Tooltip("Distance-based mode blends two LUTs over a distance")]
        public ModeParam mode = new ModeParam { value = Mode.Single };

        [Range(1f,3000f)]
        public FloatParameter distance = new FloatParameter { value = 1000f };

        [Range(0f, 1f), Tooltip("Fades the effect in or out")]
        public FloatParameter intensity = new FloatParameter { value = 0f };

        [Tooltip("Supply a LUT strip texture.")]
        public TextureParameter lutNear = new TextureParameter { value = null };
        [DisplayName("Far")]
        public TextureParameter lutFar = new TextureParameter { value = null };

        [Range(0f, 1f), Tooltip("Fades the effect in or out")]
        public FloatParameter invert = new FloatParameter { value = 0f };

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            return enabled.value;
        }
#endif
    }
}