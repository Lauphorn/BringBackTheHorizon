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
    public sealed class OverlayEditor : Editor {} }
#else
    [PostProcessEditor(typeof(Overlay))]
    public sealed class OverlayEditor : PostProcessEffectEditor<Overlay>
    {
        SerializedParameterOverride overlayTex;
        SerializedParameterOverride autoAspect;
        SerializedParameterOverride blendMode;
        SerializedParameterOverride intensity;
        SerializedParameterOverride tiling;

        public override void OnEnable()
        {
            overlayTex = FindParameterOverride(x => x.overlayTex);
            autoAspect = FindParameterOverride(x => x.autoAspect);
            blendMode = FindParameterOverride(x => x.blendMode);
            intensity = FindParameterOverride(x => x.intensity);
            tiling = FindParameterOverride(x => x.tiling);
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("overlay");

            SCPE_GUI.DisplaySetupWarning<UnityEngine.Object>(true);

            PropertyField(overlayTex);

            if (overlayTex.overrideState.boolValue && overlayTex.value.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("Assign a texture", MessageType.Info);
            }

            EditorGUILayout.Space();

            PropertyField(intensity);
            PropertyField(autoAspect);
            PropertyField(blendMode);
            PropertyField(tiling);
        }
    }
}
#endif