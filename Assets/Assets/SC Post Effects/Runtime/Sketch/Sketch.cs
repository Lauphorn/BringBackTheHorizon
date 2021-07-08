using System;
using System.Collections;
using System.Collections.Generic;
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
    [PostProcess(typeof(SketchRenderer), PostProcessEvent.BeforeStack, "SC Post Effects/Stylized/Sketch", true)]
#endif
    [Serializable]
    public sealed class Sketch : PostProcessEffectSettings
    {
#if PPS
        [Tooltip("The Red channel is used for darker shades, whereas the Green channel is for lighter.")]
        public TextureParameter strokeTex = new TextureParameter { value = null };

        public enum SketchProjectionMode
        {
            WorldSpace,
            ScreenSpace
        }
        [Serializable]
        public sealed class SketchProjectioParameter : ParameterOverride<SketchProjectionMode> { }

        [Space]
        [Tooltip("Choose the type of UV space being used")]
        public SketchProjectioParameter projectionMode = new SketchProjectioParameter { value = SketchProjectionMode.WorldSpace };

        public enum SketchMode
        {
            EffectOnly,
            Multiply,
            Add
        }

        [Serializable]
        public sealed class SketchModeParameter : ParameterOverride<SketchMode> { }

        [Tooltip("Choose one of the different modes")]
        public SketchModeParameter blendMode = new SketchModeParameter { value = SketchMode.EffectOnly };

        [Space]

        [Range(0f, 1f)]
        public FloatParameter intensity = new FloatParameter { value = 0f };

        public Vector2Parameter brightness = new Vector2Parameter { value = new Vector2(0f, 1f) };

        [Range(1f, 32f)]
        public FloatParameter tiling = new FloatParameter { value = 8f };

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            if (enabled.value)
            {
                if (intensity == 0 || strokeTex.value == null) return false;
                return true;
            }

            return false;
        }
#endif
    }
}