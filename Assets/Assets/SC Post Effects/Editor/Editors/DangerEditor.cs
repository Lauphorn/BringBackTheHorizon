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
    public sealed class DangerEditor : Editor {} }
#else
    [PostProcessEditor(typeof(Danger))]
    public sealed class DangerEditor : PostProcessEffectEditor<Danger>
    {
        SerializedParameterOverride overlayTex;
        SerializedParameterOverride color;
        SerializedParameterOverride intensity;
        SerializedParameterOverride size;

        public override void OnEnable()
        {
            overlayTex = FindParameterOverride(x => x.overlayTex);
            color = FindParameterOverride(x => x.color);
            intensity = FindParameterOverride(x => x.intensity);
            size = FindParameterOverride(x => x.size);
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("danger");

            SCPE_GUI.DisplaySetupWarning<UnityEngine.Object>(true);

            PropertyField(overlayTex);

            if (overlayTex.overrideState.boolValue && overlayTex.value.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("Assign a texture", MessageType.Info);
            }

            PropertyField(color);
            PropertyField(intensity);
            PropertyField(size);
        }
    }
}
#endif