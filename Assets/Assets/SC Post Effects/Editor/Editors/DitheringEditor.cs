using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if PPS
using UnityEditor.Rendering.PostProcessing;
using UnityEngine.Rendering.PostProcessing;
#endif

namespace SCPE
{
#if !PPS
    public sealed class DitheringEditor : Editor {} }
#else
    [PostProcessEditor(typeof(Dithering))]
    public sealed class DitheringEditor : PostProcessEffectEditor<Dithering>
    {
        SerializedParameterOverride intensity;
        SerializedParameterOverride tiling;
        SerializedParameterOverride luminanceThreshold;
        SerializedParameterOverride lut;

        public override void OnEnable()
        {
            lut = FindParameterOverride(x => x.lut);
            intensity = FindParameterOverride(x => x.intensity);
            tiling = FindParameterOverride(x => x.tiling);
            luminanceThreshold = FindParameterOverride(x => x.luminanceThreshold);
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("dithering");

            SCPE_GUI.DisplaySetupWarning<UnityEngine.Object>(true);

            PropertyField(lut);

            if (lut.overrideState.boolValue && lut.value.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("Assign a pattern texture", MessageType.Info);
            }

            EditorGUILayout.Space();

            PropertyField(luminanceThreshold);
            PropertyField(intensity);
            PropertyField(tiling);
        }
    }
}
#endif