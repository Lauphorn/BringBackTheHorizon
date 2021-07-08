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
    [PostProcess(typeof(TubeDistortionRenderer), PostProcessEvent.AfterStack, "SC Post Effects/Screen/Tube Distortion", true)]
#endif
    [Serializable]
    public sealed class TubeDistortion : PostProcessEffectSettings
    {
#if PPS
        public enum DistortionMode
        {
            Buldged = 0,
            Pinched = 1,
            Beveled = 2
        }

        [Serializable]
        public sealed class DistortionModeParam : ParameterOverride<DistortionMode> { }

        public DistortionModeParam mode = new DistortionModeParam { value = DistortionMode.Buldged };

        [Range(0f, 1f)]
        public FloatParameter amount = new FloatParameter { value = 0f };

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