#if HDRP_7_2_0_OR_NEWER
#define HDRP
#endif

#if URP_7_2_0_OR_NEWER
#define URP
#endif

using UnityEngine;
using UnityEditor;
#if PPS
#if HDRP || URP
using UnityEditor.Rendering;
using UnityEngine.Rendering;
#else
using UnityEditor.Rendering.PostProcessing;
using UnityEngine.Rendering.PostProcessing;
#endif //SRP
#endif //SCPE

namespace SCPE
{
#if !PPS
    public sealed class SunshaftsEditor : Editor {} }
#else

    public class SunshaftsEditorBase
    {
        public static void DrawCasterWarning()
        {
            if (Sunshafts.sunPosition == Vector3.zero)
            {
                EditorGUILayout.HelpBox("No source Directional Light found!\n\nAdd the \"SunshaftCaster\" script to your main light", MessageType.Warning);

                GUILayout.Space(-32);
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Add", GUILayout.Width(60)))
                    {
                        SunshaftsBase.AddShaftCaster();
                    }
                    GUILayout.Space(8);
                }
                GUILayout.Space(11);
            }
        }
    }

#if !HDRP && !URP
    [PostProcessEditor(typeof(Sunshafts))]
    public sealed class SunshaftsEditor : PostProcessEffectEditor<Sunshafts>
    {
        SerializedParameterOverride useCasterColor;
        SerializedParameterOverride useCasterIntensity;

        SerializedParameterOverride resolution;
        SerializedParameterOverride sunThreshold;
        SerializedParameterOverride blendMode;
        SerializedParameterOverride sunColor;
        SerializedParameterOverride sunShaftIntensity;
        SerializedParameterOverride falloff;

        SerializedParameterOverride length;
        SerializedParameterOverride highQuality;

        public override void OnEnable()
        {
            useCasterColor = FindParameterOverride(x => x.useCasterColor);
            useCasterIntensity = FindParameterOverride(x => x.useCasterIntensity);

            resolution = FindParameterOverride(x => x.resolution);
            sunThreshold = FindParameterOverride(x => x.sunThreshold);
            blendMode = FindParameterOverride(x => x.blendMode);
            sunColor = FindParameterOverride(x => x.sunColor);
            sunShaftIntensity = FindParameterOverride(x => x.sunShaftIntensity);
            falloff = FindParameterOverride(x => x.falloff);
            length = FindParameterOverride(x => x.length);
            highQuality = FindParameterOverride(x => x.highQuality);
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("sunshafts");

            SCPE_GUI.DisplayVRWarning();

            SCPE_GUI.DisplaySetupWarning<UnityEngine.Object>(true);

            SunshaftsEditorBase.DrawCasterWarning();

            EditorUtilities.DrawHeaderLabel("Quality");
            PropertyField(resolution);
            PropertyField(highQuality, new GUIContent("High quality"));

            EditorGUILayout.Space();

            EditorUtilities.DrawHeaderLabel("Use values from caster");
            PropertyField(useCasterColor, new GUIContent("Color"));
            PropertyField(useCasterIntensity, new GUIContent("Intensity"));

            EditorGUILayout.Space();

            EditorUtilities.DrawHeaderLabel("Sunshafts");
            PropertyField(blendMode);
            PropertyField(sunThreshold);
            PropertyField(falloff);
            PropertyField(length);
            if (useCasterColor.value.boolValue == false) PropertyField(sunColor);
            if (useCasterIntensity.value.boolValue == false) PropertyField(sunShaftIntensity);

        }

    }
#else //SRP
    [VolumeComponentEditor(typeof(Sunshafts))]
    sealed class SunshaftsEditor : VolumeComponentEditor
    {
    }
#endif
}
#endif