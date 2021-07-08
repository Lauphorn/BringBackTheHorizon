using System;
using System.Collections;
using System.Collections.Generic;
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
    [PostProcess(typeof(TiltShiftRenderer), PostProcessEvent.AfterStack, "SC Post Effects/Blurring/Tilt Shift")]
#endif
    [Serializable]
    public class TiltShift : PostProcessEffectSettings
    {
#if PPS
        public enum TiltShiftMethod
        {
            Horizontal,
            Radial,
        }

        [Serializable]
        public sealed class TiltShifMethodParameter : ParameterOverride<TiltShiftMethod> { }

        [DisplayName("Method")]
        public TiltShifMethodParameter mode = new TiltShifMethodParameter { value = TiltShiftMethod.Horizontal };

        public enum Quality
        {
            Performance,
            Appearance
        }

        [Serializable]
        public sealed class TiltShiftQualityParameter : ParameterOverride<Quality> { }

        [DisplayName("Quality"), Tooltip("Choose to use more texture samples, for a smoother blur when using a high blur amout")]
        public TiltShiftQualityParameter quality = new TiltShiftQualityParameter { value = Quality.Appearance };

        [Range(0f, 1f)]
        public FloatParameter areaSize = new FloatParameter { value = 1f };
        [Range(0f, 1f)]
        public FloatParameter areaFalloff = new FloatParameter { value = 1f };

        [Range(0f, 1f), Tooltip("The amount of blurring that must be performed")]
        public FloatParameter amount = new FloatParameter { value = 0f };

        public static bool debug;

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            if (enabled.value)
            {
                if ((areaSize == 0f && areaFalloff == 0f) || amount == 0f) { return false; }
                return true;
            }

            return false;
        }
#endif
    }
}