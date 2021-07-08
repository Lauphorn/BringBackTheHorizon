using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

#if PPS
using UnityEditor.Rendering.PostProcessing;
using UnityEngine.Rendering.PostProcessing;
#endif

using UnityEditor.Rendering;

namespace SCPE
{
#if PPS
    [PostProcessEditor(typeof(EdgeDetection))]
    public sealed class EdgeDetectionEditor : PostProcessEffectEditor<EdgeDetection>
    {
        SerializedParameterOverride mode;

        SerializedParameterOverride sensitivityDepth;
        SerializedParameterOverride sensitivityNormals;
        SerializedParameterOverride lumThreshold;

        SerializedParameterOverride edgeExp;
        SerializedParameterOverride sampleDist;

        SerializedParameterOverride edgesOnly;
        SerializedParameterOverride edgeColor;
        SerializedParameterOverride edgeOpacity;

        SerializedParameterOverride invertFadeDistance;
        SerializedParameterOverride fadeDistance;
        SerializedParameterOverride sobelThin;

        private static bool showHelp;

        public override string GetDisplayTitle()
        {
            return "Edge Detection (" + mode.value.enumDisplayNames[mode.value.intValue] + ")";
        }

        public override void OnEnable()
        {

            mode = FindParameterOverride(x => x.mode);
            sensitivityDepth = FindParameterOverride(x => x.sensitivityDepth);
            sensitivityNormals = FindParameterOverride(x => x.sensitivityNormals);
            lumThreshold = FindParameterOverride(x => x.lumThreshold);
            edgeExp = FindParameterOverride(x => x.edgeExp);
            sampleDist = FindParameterOverride(x => x.edgeSize);
            edgesOnly = FindParameterOverride(x => x.debug);
            edgeColor = FindParameterOverride(x => x.edgeColor);
            edgeOpacity = FindParameterOverride(x => x.edgeOpacity);
            invertFadeDistance = FindParameterOverride(x => x.invertFadeDistance);
            fadeDistance = FindParameterOverride(x => x.fadeDistance);
            sobelThin = FindParameterOverride(x => x.sobelThin);
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("edge-detection");

            SCPE_GUI.DisplayVRWarning();

            SCPE_GUI.DisplaySetupWarning<UnityEngine.Object>(true);

            PropertyField(edgesOnly);

            PropertyField(mode);

            if (mode.overrideState.boolValue)
            {
                EditorGUILayout.BeginHorizontal();
                switch (mode.value.intValue)
                {
                    case 0:
                        EditorGUILayout.HelpBox("Checks pixels for differences between geometry normals and their distance from the camera", MessageType.None);
                        break;
                    case 1:
                        EditorGUILayout.HelpBox("Same as Depth Normals but also uses vertical sampling for improved accuracy", MessageType.None);
                        break;
                    case 2:
                        EditorGUILayout.HelpBox("Draws edges only where neighboring pixels greatly differ in their depth value.", MessageType.None);
                        break;
                    case 3:
                        EditorGUILayout.HelpBox("Creates an edge where the luminance value of a pixel differs from its neighbors, past the threshold", MessageType.None);
                        break;
                }
                EditorGUILayout.EndHorizontal();
            }

#if LWRP
            if (mode.value.intValue < 2 && mode.overrideState.boolValue == true && RenderPipelineManager.currentPipeline != null)
            {
                EditorGUILayout.HelpBox("Not supported in LWRP", MessageType.Error);
            }
#endif

            using (new EditorGUILayout.HorizontalScope())
            {
                // Override checkbox
                var overrideRect = GUILayoutUtility.GetRect(17f, 17f, GUILayout.ExpandWidth(false));
                overrideRect.yMin += 4f;
                EditorUtilities.DrawOverrideCheckbox(overrideRect, fadeDistance.overrideState);

                EditorGUILayout.PrefixLabel(fadeDistance.displayName);

                GUILayout.FlexibleSpace();

                fadeDistance.value.floatValue = EditorGUILayout.FloatField(fadeDistance.value.floatValue);

                bool enabled = invertFadeDistance.value.boolValue;
                enabled = GUILayout.Toggle(enabled, "Start", EditorStyles.miniButtonLeft, GUILayout.Width(50f), GUILayout.ExpandWidth(false));
                enabled = !GUILayout.Toggle(!enabled, "End", EditorStyles.miniButtonRight, GUILayout.Width(50f), GUILayout.ExpandWidth(false));

                invertFadeDistance.value.boolValue = enabled;
            }

            if (mode.value.intValue < 2)
            {
                PropertyField(sensitivityDepth);
                PropertyField(sensitivityNormals);
            }
            else if (mode.value.intValue == 2)
            {
                PropertyField(edgeExp);
            }
            else
            {
                // lum based mode
                PropertyField(lumThreshold);
            }

            //Edges
            PropertyField(edgeColor);
            PropertyField(edgeOpacity);
            PropertyField(sampleDist);
            if (mode.value.intValue == 2)
            {
                PropertyField(sobelThin);
            }
        }
    }
#endif
}