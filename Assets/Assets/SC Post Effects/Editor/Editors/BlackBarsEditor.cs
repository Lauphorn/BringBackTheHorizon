using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using SCPE;
#if HDRP_7_2_0_OR_NEWER || URP_7_2_0_OR_NEWER
using UnityEditor.Rendering;
#endif
#if PPS && !HDRP_7_2_0_OR_NEWER && !URP_7_2_0_OR_NEWER
using UnityEngine.Rendering.PostProcessing;
using UnityEditor.Rendering.PostProcessing;

namespace SCPE
{
    [PostProcessEditor(typeof(BlackBars))]
    public class BlackBarsEditor : PostProcessEffectEditor<BlackBars>
    {
        SerializedParameterOverride mode;
        SerializedParameterOverride size;
        SerializedParameterOverride maxSize;

        public override void OnEnable()
        {
            mode = FindParameterOverride(x => x.mode);
            size = FindParameterOverride(x => x.size);
            maxSize = FindParameterOverride(x => x.maxSize);
        }

        public override string GetDisplayTitle()
        {
            return "Black Bars (" + (BlackBars.Direction)mode.value.enumValueIndex + ")";
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("black-bars");

            SCPE_GUI.DisplaySetupWarning<UnityEngine.Object>(true);

            PropertyField(mode);
            PropertyField(size);
            PropertyField(maxSize);
        }
    }
}
#endif

#if HDRP_7_2_0_OR_NEWER || URP_7_2_0_OR_NEWER

namespace SCPE
{
    [VolumeComponentEditor(typeof(BlackBars))]
    sealed class ScanlinesEditor : VolumeComponentEditor
    {
        SerializedDataParameter mode;
        SerializedDataParameter size;
        SerializedDataParameter maxSize;

        public override bool hasAdvancedMode => false;

        public override void OnEnable()
        {
            base.OnEnable();

            var o = new PropertyFetcher<BlackBars>(serializedObject);

            mode = Unpack(o.Find(x => x.mode));
            size = Unpack(o.Find(x => x.size));
            maxSize = Unpack(o.Find(x => x.maxSize));
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("blackbars");

            PropertyField(mode);
            PropertyField(size);
            PropertyField(maxSize);
        }
    }
}
#endif