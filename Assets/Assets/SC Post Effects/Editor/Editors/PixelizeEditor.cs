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
    public sealed class PixelizeEditor : Editor {} }
#else
    [PostProcessEditor(typeof(Pixelize))]
    public sealed class PixelizeEditor : PostProcessEffectEditor<Pixelize>
    {
        SerializedParameterOverride amount;

        public override void OnEnable()
        {
            amount = FindParameterOverride(x => x.amount);
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("pixelize");

            SCPE_GUI.DisplaySetupWarning<UnityEngine.Object>(true);

            PropertyField(amount);
        }
    }
}
#endif