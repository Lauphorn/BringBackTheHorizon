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
    [PostProcess(typeof(HueShift3DRenderer), PostProcessEvent.AfterStack, "SC Post Effects/Image/3D Hue Shift", true)]
#endif
    [Serializable]
    public sealed class HueShift3D : PostProcessEffectSettings
    {
#if PPS
        [Range(0f, 1f), DisplayName("Opacity")]
        public FloatParameter intensity = new FloatParameter { value = 0f };

        [Range(0f, 1f), Tooltip("Speed")]
        public FloatParameter speed = new FloatParameter { value = 0.3f };

        [Range(0f, 3f), Tooltip("Size")]
        public FloatParameter size = new FloatParameter { value = 1f };

        [DisplayName("Geometry normal influence"), Range(0f, 10f), Tooltip("Bends the effect over the scene's geometry normals\n\nHigh values may induce banding artifacts")]
        public FloatParameter geoInfluence = new FloatParameter { value = 5f };

        public static bool isOrtho = false;

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            if (enabled.value)
            {
                if (intensity == 0) { return false; }
                return true;
            }

            return false;
        }
#endif
    }
}