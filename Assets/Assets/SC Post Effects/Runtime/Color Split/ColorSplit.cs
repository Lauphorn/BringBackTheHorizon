using System;
using UnityEngine;
#if PPS
using UnityEngine.Rendering.PostProcessing;
using FloatParameter = UnityEngine.Rendering.PostProcessing.FloatParameter;
#endif

namespace SCPE
{
#if PPS
    [PostProcess(typeof(ColorSplitRenderer), PostProcessEvent.AfterStack, "SC Post Effects/Retro/Color Split", true)]
#endif
    [Serializable]
    public sealed class ColorSplit : PostProcessEffectSettings
    {
#if PPS
        public enum SplitMode
        {
            Single = 0,
            SingleBoxFiltered = 1,
            Double = 2,
            DoubleBoxFiltered = 3
        }

        [Serializable]
        public sealed class SplitModeParam : ParameterOverride<SplitMode> { }

        [DisplayName("Method"), Tooltip("Box filtered methods provide a subtle blur effect and are less efficient")]
        public SplitModeParam mode = new SplitModeParam { value = SplitMode.Single };

        [Range(0f, 1f), Tooltip("The amount by which the color channels offset")]
        public FloatParameter offset = new FloatParameter { value = 0f };

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            if (enabled.value)
            {
                if (offset == 0) { return false; }
                return true;
            }

            return false;
        }
#endif
    }
}