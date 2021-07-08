using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if PPS
using UnityEngine.Rendering.PostProcessing;
using TextureParameter = UnityEngine.Rendering.PostProcessing.TextureParameter;
using FloatParameter = UnityEngine.Rendering.PostProcessing.FloatParameter;
#endif

namespace SCPE
{
#if PPS
    [PostProcess(typeof(ColorizeRenderer), PostProcessEvent.AfterStack, "SC Post Effects/Image/Colorize", true)]
#endif
    [Serializable]
    public sealed class Colorize : PostProcessEffectSettings
    {
#if PPS
        public enum BlendMode
        {
            Linear,
            Additive,
            Multiply,
            Screen
        }

        [Serializable]
        public sealed class BlendModeParameter : ParameterOverride<BlendMode> { }

        [Tooltip("Blends the gradient through various Photoshop-like blending modes")]
        public BlendModeParameter mode = new BlendModeParameter { value = BlendMode.Linear };

        [Range(0f, 1f), Tooltip("Fades the effect in or out")]
        public FloatParameter intensity = new FloatParameter { value = 0f };

        [Tooltip("Supply a gradient texture.\n\nLuminance values are colorized from left to right")]
        public TextureParameter colorRamp = new TextureParameter { value = null };

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            if (enabled.value)
            {
                if (intensity == 0 || !colorRamp.value) return false;
                return true;
            }

            return false;
        }
#endif
    }
}