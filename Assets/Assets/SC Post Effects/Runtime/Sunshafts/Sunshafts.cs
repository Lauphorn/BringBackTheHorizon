using System;
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
    [PostProcess(typeof(SunshaftsRenderer), PostProcessEvent.AfterStack, "SC Post Effects/Environment/Sun Shafts", true)]
#endif
    [Serializable]
    public sealed class Sunshafts : PostProcessEffectSettings
    {
#if PPS
        //SunshaftCaster
        [Tooltip("Use the color of the Directional Light that's set as the caster")]
        public BoolParameter useCasterColor = new BoolParameter { value = true };
        [Tooltip("Use the intensity of the Directional Light that's set as the caster")]
        public BoolParameter useCasterIntensity = new BoolParameter { value = false };

        [DisplayName("Intensity")]
        public FloatParameter sunShaftIntensity = new FloatParameter { value = 0f };

        [Serializable]
        public sealed class SunShaftsSourceParameter : ParameterOverride<SunshaftsBase.BlendMode> { }
        [Tooltip("Additive mode adds the sunshaft color to the image, while Screen mode perserves color values")]
        public SunShaftsSourceParameter blendMode = new SunShaftsSourceParameter { value = SunshaftsBase.BlendMode.Screen };

        [Serializable]
        public sealed class SunShaftsResolutionParameter : ParameterOverride<SunshaftsBase.SunShaftsResolution> { }
        [DisplayName("Resolution"), Tooltip("Low, quater resolution\n\nNormal, half resolution\n\nHigh, full resolution\n\nLower resolutions may induce jittering")]
        public SunShaftsResolutionParameter resolution = new SunShaftsResolutionParameter { value = SunshaftsBase.SunShaftsResolution.Normal };

        [Tooltip("Enabling this option doubles the amount of blurring performed. Resulting in smoother sunshafts at a higher performance cost.")]
        public BoolParameter highQuality = new BoolParameter { value = false };

        [Tooltip("Any color values over this threshold will contribute to the sunshafts effect")]
        [DisplayName("Sky color threshold")]
        public ColorParameter sunThreshold = new ColorParameter { value = Color.black};

        [DisplayName("Color")]
        public ColorParameter sunColor = new ColorParameter { value = new Color(1f, 1f, 1f) };
        [Range(0.1f, 1f)]
        [Tooltip("The degree to which the shafts’ brightness diminishes with distance from the caster")]
        public FloatParameter falloff = new FloatParameter { value = 0.5f };

        [Tooltip("The length of the sunrays from the caster's position to the camera")]
        [Min(0f)]
        public FloatParameter length = new FloatParameter { value = 10f };

        [Range(0f, 1f)]
        public FloatParameter noiseStrength = new FloatParameter { value = 0f };

        public static Vector3 sunPosition = Vector3.zero;

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            if (enabled.value)
            {
                if (sunShaftIntensity == 0 || length == 0) { return false; }
                return true;
            }

            return false;
        }
#endif
    }
}