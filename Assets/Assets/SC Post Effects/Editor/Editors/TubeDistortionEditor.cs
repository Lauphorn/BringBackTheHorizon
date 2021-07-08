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
    public sealed class TubeDistortionEditor : Editor {} }
#else
    [PostProcessEditor(typeof(TubeDistortion))]
    public sealed class TubeDistortionEditor : PostProcessEffectEditor<TubeDistortion>
    {
        SerializedParameterOverride mode;
        SerializedParameterOverride amount;
        SerializedParameterOverride luminanceThreshold;
        SerializedParameterOverride lut;

        public override void OnEnable()
        {
            mode = FindParameterOverride(x => x.mode);
            amount = FindParameterOverride(x => x.amount);
        }

        public override string GetDisplayTitle()
        {
            return base.GetDisplayTitle() + SCPE_GUI.ModeTitle(mode);
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("tube-distortion");

            SCPE_GUI.DisplaySetupWarning<UnityEngine.Object>(true);

            PropertyField(mode);
            PropertyField(amount);
        }
    }
}
#endif