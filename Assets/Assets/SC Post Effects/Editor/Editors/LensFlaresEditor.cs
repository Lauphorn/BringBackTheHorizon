using System.Collections;
using System.Collections.Generic;
using UnityEditor;
#if PPS
using UnityEngine.Rendering.PostProcessing;
using UnityEditor.Rendering.PostProcessing;
#endif

namespace SCPE
{
#if !PPS
    public sealed class LensFlaresEditor : Editor {} }
#else
    [PostProcessEditor(typeof(LensFlares))]
    public class LensFlaresEditor : PostProcessEffectEditor<LensFlares>
    {
        SerializedParameterOverride intensity;
        SerializedParameterOverride luminanceThreshold;
        SerializedParameterOverride maskTex;
        SerializedParameterOverride chromaticAbberation;
        SerializedParameterOverride colorTex;

        //Flares
        SerializedParameterOverride iterations;
        SerializedParameterOverride distance;
        SerializedParameterOverride falloff;

        //Halo
        SerializedParameterOverride haloSize;
        SerializedParameterOverride haloWidth;

        //Blur
        SerializedParameterOverride blur;
        SerializedParameterOverride passes;

        public override void OnEnable()
        {
            intensity = FindParameterOverride(x => x.intensity);
            luminanceThreshold = FindParameterOverride(x => x.luminanceThreshold);
            maskTex = FindParameterOverride(x => x.maskTex);
            chromaticAbberation = FindParameterOverride(x => x.chromaticAbberation);
            colorTex = FindParameterOverride(x => x.colorTex);

            //Flares
            iterations = FindParameterOverride(x => x.iterations);
            distance = FindParameterOverride(x => x.distance);
            falloff = FindParameterOverride(x => x.falloff);

            //Halo
            haloSize = FindParameterOverride(x => x.haloSize);
            haloWidth = FindParameterOverride(x => x.haloWidth);

            //Blur
            blur = FindParameterOverride(x => x.blur);
            passes = FindParameterOverride(x => x.passes);
        }

        public override void OnInspectorGUI()
        {
            SCPE_GUI.DisplayDocumentationButton("lens-flares");

            SCPE_GUI.DisplaySetupWarning<UnityEngine.Object>(true);

            SCPE_GUI.DisplayVRWarning();

            PropertyField(intensity);
            PropertyField(luminanceThreshold);

            //Flares
            PropertyField(iterations);
            if(iterations.value.intValue > 1) PropertyField(distance);
            PropertyField(falloff);

            //Halo
            PropertyField(haloSize);
            PropertyField(haloWidth);

            PropertyField(maskTex);
            PropertyField(chromaticAbberation);
            PropertyField(colorTex);
            if (colorTex.value.objectReferenceValue)
            {
                SCPE.CheckGradientImportSettings(colorTex);
            }

            //Blur
            PropertyField(blur);
            PropertyField(passes);
        }
    }
}
#endif