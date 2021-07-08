using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if PPS
using UnityEditor.Rendering.PostProcessing;
#endif

namespace SCPE
{
#if !PPS
    public sealed class ColorizeEditor : Editor {} }
#else
    [PostProcessEditor(typeof(Colorize))]
    public sealed class ColorizeEditor : PostProcessEffectEditor<Colorize>
    {
        SerializedParameterOverride mode;
        SerializedParameterOverride intensity;
        SerializedParameterOverride colorRamp;

        public override void OnEnable()
        {
            mode = FindParameterOverride(x => x.mode);
            intensity = FindParameterOverride(x => x.intensity);
            colorRamp = FindParameterOverride(x => x.colorRamp);
        }

        public override string GetDisplayTitle()
        {
            return "Colorize (" + mode.value.enumDisplayNames[mode.value.intValue] + ")";
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("colorize");

            SCPE_GUI.DisplaySetupWarning<UnityEngine.Object>(true);

            PropertyField(mode);
            PropertyField(intensity);
            PropertyField(colorRamp);

            if (colorRamp.value.objectReferenceValue)
            {
                SCPE.CheckGradientImportSettings(colorRamp);
            }
        }
    }
}
#endif