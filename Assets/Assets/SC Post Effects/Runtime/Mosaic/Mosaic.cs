using System;
using UnityEngine;
#if PPS
using UnityEngine.Rendering.PostProcessing;
#endif

namespace SCPE
{
#if PPS
    [PostProcess(typeof(MosaicRenderer), PostProcessEvent.AfterStack, "SC Post Effects/Stylized/Mosaic", true)]
#endif
    [Serializable]
    public sealed class Mosaic : PostProcessEffectSettings
    {
#if PPS
        public enum MosaicMode
        {
            Triangles = 0,
            Hexagons = 1,
            Circles
        }

        [Serializable]
        public sealed class MosaicModeParam : ParameterOverride<MosaicMode> { }

        [DisplayName("Method"), Tooltip("")]
        public MosaicModeParam mode = new MosaicModeParam { value = MosaicMode.Hexagons };

        [Range(0f, 1f), Tooltip("Size")]
        public UnityEngine.Rendering.PostProcessing.FloatParameter size = new UnityEngine.Rendering.PostProcessing.FloatParameter { value = 0f };

        public override bool IsEnabledAndSupported(PostProcessRenderContext context)
        {
            if (enabled.value)
            {
                if (size == 0) { return false; }
                return true;
            }

            return false;
        }
#endif
    }
}