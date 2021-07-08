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
    public sealed class KaleidoscopeEditor : Editor {} }
#else
    [PostProcessEditor(typeof(Kaleidoscope))]
    public sealed class KaleidoscopeEditor : PostProcessEffectEditor<Kaleidoscope>
    {
        SerializedParameterOverride splits;

        public override void OnEnable()
        {
            splits = FindParameterOverride(x => x.splits);
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("kaleidoscope");

            SCPE_GUI.DisplaySetupWarning<UnityEngine.Object>(true);

            PropertyField(splits);
        }
    }
}
#endif