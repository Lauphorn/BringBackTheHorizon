using System.Collections;
using System.Collections.Generic;
using UnityEditor;
#if PPS
using UnityEngine.Rendering.PostProcessing;
using UnityEditor.Rendering.PostProcessing;
#endif

namespace SCPE
{
#if !PPS
    public sealed class LightStreaksEditor : Editor {} }
#else
    [PostProcessEditor(typeof(LightStreaks))]
    public class LightStreaksEditor : PostProcessEffectEditor<LightStreaks>
    {
        SerializedParameterOverride quality;
        SerializedParameterOverride debug;
        SerializedParameterOverride intensity;
        SerializedParameterOverride luminanceThreshold;
        SerializedParameterOverride direction;
        SerializedParameterOverride blur;
        SerializedParameterOverride iterations;
        SerializedParameterOverride downscaling;

        public override void OnEnable()
        {
            quality = FindParameterOverride(x => x.quality);
            debug = FindParameterOverride(x => x.debug);
            intensity = FindParameterOverride(x => x.intensity);
            luminanceThreshold = FindParameterOverride(x => x.luminanceThreshold);
            direction = FindParameterOverride(x => x.direction);
            blur = FindParameterOverride(x => x.blur);
            iterations = FindParameterOverride(x => x.iterations);
            downscaling = FindParameterOverride(x => x.downscaling);
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("light-streaks");

            SCPE_GUI.DisplaySetupWarning<UnityEngine.Object>(true);

            PropertyField(quality);
            PropertyField(debug);
            PropertyField(intensity);
            PropertyField(luminanceThreshold);
            PropertyField(direction);
            PropertyField(blur);
            PropertyField(iterations);
            PropertyField(downscaling);
           
        }
    }
}
#endif