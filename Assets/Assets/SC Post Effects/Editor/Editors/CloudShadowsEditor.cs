using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
#if PPS
using UnityEditor.Rendering.PostProcessing;
using UnityEngine.Rendering.PostProcessing;
#endif

namespace SCPE
{
#if !PPS
    public sealed class CloudShadowsEditor : Editor {}
    }
#else
    [PostProcessEditor(typeof(CloudShadows))]
    public sealed class CloudShadowsEditor : PostProcessEffectEditor<CloudShadows>
    {
        SerializedParameterOverride texture;
        SerializedParameterOverride size;
        SerializedParameterOverride density;
        SerializedParameterOverride speed;
        SerializedParameterOverride direction;

        public override void OnEnable()
        {
            texture = FindParameterOverride(x => x.texture);
            size = FindParameterOverride(x => x.size);
            density = FindParameterOverride(x => x.density);
            speed = FindParameterOverride(x => x.speed);
            direction = FindParameterOverride(x => x.direction);
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayVRWarning();

            if (CloudShadows.isOrtho) EditorGUILayout.HelpBox("Not available for orthographic cameras", MessageType.Warning);

            SCPE_GUI.DisplaySetupWarning<UnityEngine.Object>(true);

            PropertyField(texture);

            PropertyField(size);
            PropertyField(density);
            PropertyField(speed);
            PropertyField(direction);

            EditorGUI.EndDisabledGroup();
        }
    }
}
#endif