using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
#if PPS
using UnityEngine.Rendering.PostProcessing;
using UnityEditor.Rendering.PostProcessing;
using SCPE;
#endif

namespace SCPE
{
#if !PPS
    public sealed class GradientEditor : Editor {} }
#else
    [PostProcessEditor(typeof(Gradient))]
    public class GradientEditor : PostProcessEffectEditor<Gradient>
    {
        SerializedParameterOverride intensity;
        SerializedParameterOverride input;
        SerializedParameterOverride color1;
        SerializedParameterOverride color2;
        SerializedParameterOverride rotation;
        SerializedParameterOverride gradientTex;
        SerializedParameterOverride mode;

        public override void OnEnable()
        {
            intensity = FindParameterOverride(x => x.intensity);
            input = FindParameterOverride(x => x.input);
            color1 = FindParameterOverride(x => x.color1);
            color2 = FindParameterOverride(x => x.color2);
            rotation = FindParameterOverride(x => x.rotation);
            gradientTex = FindParameterOverride(x => x.gradientTex);
            mode = FindParameterOverride(x => x.mode);
        }

        public override string GetDisplayTitle()
        {
            return base.GetDisplayTitle() + SCPE_GUI.ModeTitle(mode);
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("gradient");

            SCPE_GUI.DisplaySetupWarning<UnityEngine.Object>(true);

            PropertyField(intensity);
            PropertyField(input);

            //If Radial
            if (input.value.intValue == 1)
            {
                PropertyField(gradientTex);

                if (gradientTex.value.objectReferenceValue)
                {
                    SCPE.CheckGradientImportSettings(gradientTex);
                }

            }
            else
            {
                PropertyField(color1);
                PropertyField(color2);
            }

            PropertyField(mode);
            PropertyField(rotation);

        }

    }
}
#endif