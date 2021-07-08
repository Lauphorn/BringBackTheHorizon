using UnityEditor;
using UnityEngine;
#if PPS
using UnityEngine.Rendering.PostProcessing;
using UnityEditor.Rendering.PostProcessing;
#endif

namespace SCPE
{
#if !PPS
    public sealed class DoubleVisionEditor : Editor {} }
#else
    [PostProcessEditor(typeof(DoubleVision))]
    public sealed class DoubleVisionEditor : PostProcessEffectEditor<DoubleVision>
    {
        SerializedParameterOverride mode;
        SerializedParameterOverride intensity;
        SerializedParameterOverride amount;

        public override void OnEnable()
        {
            mode = FindParameterOverride(x => x.mode);
            intensity = FindParameterOverride(x => x.intensity);
        }
        
        public override string GetDisplayTitle()
        {
            return base.GetDisplayTitle() + SCPE_GUI.ModeTitle(mode);
        }
        
        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("double-vision");

            SCPE_GUI.DisplaySetupWarning<UnityEngine.Object>(true);

            PropertyField(mode);
            PropertyField(intensity);
        }
    }
}
#endif