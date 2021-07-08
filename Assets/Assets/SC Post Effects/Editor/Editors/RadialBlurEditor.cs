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
#if PPS
    [PostProcessEditor(typeof(RadialBlur))]
    public sealed class RadialBlurEditor : PostProcessEffectEditor<RadialBlur>
    {
        SerializedParameterOverride amount;
        SerializedParameterOverride iterations;

        public override void OnEnable()
        {
            amount = FindParameterOverride(x => x.amount);
            iterations = FindParameterOverride(x => x.iterations);
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("radial-blur");

            SCPE_GUI.DisplaySetupWarning<UnityEngine.Object>(true);

            PropertyField(amount);
            PropertyField(iterations);
        }
    }
#endif
}