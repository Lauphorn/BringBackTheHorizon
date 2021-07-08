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
using Vector2Parameter = UnityEngine.Rendering.PostProcessing.Vector2Parameter;
using MinAttribute = UnityEngine.Rendering.PostProcessing.MinAttribute;
#endif

namespace SCPE
{
#if PPS
    [PostProcess(typeof(CloudShadowsRenderer), PostProcessEvent.BeforeStack, "SC Post Effects/Environment/Cloud Shadows")]
#endif
    [Serializable]
    public sealed class CloudShadows : PostProcessEffectSettings
    {
#if PPS
        [DisplayName("Texture (R)"), Tooltip("The red channel of this texture is used to sample the clouds")]
        public TextureParameter texture = new TextureParameter { value = null };

        [Space]

        [Range(0f, 1f)]
        [DisplayName("Size")]
        public FloatParameter size = new FloatParameter { value = 0.5f };
        [Range(0f, 1f)]
        [DisplayName("Density")]
        public FloatParameter density = new FloatParameter { value = 0f };
        [Range(0f, 1f)]
        [DisplayName("Speed")]
        public FloatParameter speed = new FloatParameter { value = 0.5f };

        [DisplayName("Direction"), Tooltip("Set the X and Z world-space direction the clouds should move in")]
        public Vector2Parameter direction = new Vector2Parameter { value = new Vector2(0f, 1f) };

        public static bool isOrtho = false;

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            if (enabled.value)
            {
                if (density == 0 || texture.value == null) return false;
                return true;
            }

            return false;
        }
#endif
    }
}