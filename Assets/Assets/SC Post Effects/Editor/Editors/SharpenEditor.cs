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
    public sealed class SharpenEditor : Editor {} }
#else
    [PostProcessEditor(typeof(Sharpen))]
    public sealed class SharpenEditor : PostProcessEffectEditor<Sharpen>
    {
        SerializedParameterOverride amount;
        SerializedParameterOverride radius;

        public override void OnEnable()
        {
            amount = FindParameterOverride(x => x.amount);
            radius = FindParameterOverride(x => x.radius);
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("sharpen");

            SCPE_GUI.DisplaySetupWarning<UnityEngine.Object>(true);

            PropertyField(amount);
            PropertyField(radius);
        }
    }
}
#endif