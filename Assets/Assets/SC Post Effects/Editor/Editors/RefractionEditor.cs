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
    public sealed class RefractionEditor : Editor {} }
#else
    [PostProcessEditor(typeof(Refraction))]
    public sealed class RefractionEditor : PostProcessEffectEditor<Refraction>
    {
        SerializedParameterOverride refractionTex;
        SerializedParameterOverride convertNormalMap;
        SerializedParameterOverride amount;

        public override void OnEnable()
        {
            amount = FindParameterOverride(x => x.amount);
            convertNormalMap = FindParameterOverride(x => x.convertNormalMap);
            refractionTex = FindParameterOverride(x => x.refractionTex);
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("refraction");

            SCPE_GUI.DisplaySetupWarning<UnityEngine.Object>(true);

            PropertyField(refractionTex);

            if (refractionTex.overrideState.boolValue && refractionTex.value.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("Assign a texture", MessageType.Info);
            }

            PropertyField(convertNormalMap);

            EditorGUILayout.Space();

            PropertyField(amount);
        }
    }
}
#endif