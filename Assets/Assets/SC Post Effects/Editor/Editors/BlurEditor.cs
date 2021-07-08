using System.Collections;
using System.Collections.Generic;
using UnityEditor;
#if PPS
using UnityEngine.Rendering.PostProcessing;
using UnityEditor.Rendering.PostProcessing;

namespace SCPE
{
    [PostProcessEditor(typeof(Blur))]
    public class BlurEditor : PostProcessEffectEditor<Blur>
    {
        SerializedParameterOverride mode;
        SerializedParameterOverride highQuality;
        SerializedParameterOverride amount;
        SerializedParameterOverride iterations;
        SerializedParameterOverride downscaling;

        public override void OnEnable()
        {
            mode = FindParameterOverride(x => x.mode);
            highQuality = FindParameterOverride(x => x.highQuality);
            amount = FindParameterOverride(x => x.amount);
            iterations = FindParameterOverride(x => x.iterations);
            downscaling = FindParameterOverride(x => x.downscaling);
        }

        public override string GetDisplayTitle()
        {
            return base.GetDisplayTitle() + " (" + mode.value.enumDisplayNames[mode.value.intValue] + ")";
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("blur");

            SCPE_GUI.DisplaySetupWarning<UnityEngine.Object>(true);

            PropertyField(mode);
            PropertyField(highQuality);
            PropertyField(amount);
            PropertyField(iterations);
            PropertyField(downscaling);

        }
    }
}
#endif