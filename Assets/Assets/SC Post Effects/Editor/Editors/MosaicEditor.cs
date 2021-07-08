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
    public  class MosaicEditor : Editor {} }
#else
    [PostProcessEditor(typeof(Mosaic))]
    public sealed class MosaicEditor : PostProcessEffectEditor<Mosaic>
    {
        SerializedParameterOverride mode;
        SerializedParameterOverride size;

        public override void OnEnable()
        {
            mode = FindParameterOverride(x => x.mode);
            size = FindParameterOverride(x => x.size);
        }

        public override string GetDisplayTitle()
        {
            return base.GetDisplayTitle() + SCPE_GUI.ModeTitle(mode);
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("mosaic");

            SCPE_GUI.DisplaySetupWarning<UnityEngine.Object>(true);

            PropertyField(mode);
            PropertyField(size);
        }
    }
}
#endif