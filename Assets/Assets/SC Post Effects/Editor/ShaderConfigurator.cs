using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SCPE
{
    public class ShaderConfigurator
    {
        static List<string> filePaths;

        private static List<string> GetShaderFilePaths()
        {
            string packageDir = SCPE.GetRootFolder();

            //Currently not needed!
            /*
            List<string> filePaths = new List<string>();

            string effectShaderDir = packageDir + "/Runtime";

            //Find all shaders in the package folder
            string[] GUIDs = AssetDatabase.FindAssets("*Shader t:Shader", new string[] { effectShaderDir });
            for (int i = 0; i < GUIDs.Length; i++)
            {
                //filePaths.Add(AssetDatabase.GUIDToAssetPath(GUIDs[i]));
            }
            */

            string pipelineLibrary = packageDir + "/Shaders/Pipeline/Pipeline.hlsl";
            filePaths.Add(pipelineLibrary);
#if SCPE_DEV
            Debug.Log("<b>ConfigureShaderPaths</b> found " + filePaths.Count + " shaders/libraries to reconfigure");
#endif

            return filePaths;
        }

        private struct CodeBlock
        {
            public int startLine;
            public int endLine;
        }

        public static void ConfigureForCurrentPipeline()
        {
            SCPE.GetRenderPipeline();

            switch (SCPE.pipeline)
            {
                case SCPE.RenderPipeline.Legacy:
                    ConfigureForStandardRP();
                    break;
                case SCPE.RenderPipeline.Universal:
                    ConfigureForURP();
                    break;
            }

            Installer.Log.Write("Activated " + SCPE.pipeline + " render pipeline shader library");
        }

#if SCPE_DEV
        [MenuItem("SCPE/Installation/ConfigureForStandardRP")]
#endif
        public static void ConfigureForStandardRP()
        {
            filePaths = GetShaderFilePaths();

            for (int i = 0; i < filePaths.Count; i++)
            {
                if (!EditorUtility.DisplayCancelableProgressBar("Installation", "Processing shaders " + i + "/" + filePaths.Count, i / (float)filePaths.Count))
                {
                    ToggleCodeBlock(filePaths[i], "urp", false);
                    ToggleCodeBlock(filePaths[i], "std", true);
                    //return;
                }
            }

            EditorUtility.ClearProgressBar();
        }

#if SCPE_DEV
        [MenuItem("SCPE/Installation/ConfigureForURP")]
#endif
        public static void ConfigureForURP()
        {
            filePaths = GetShaderFilePaths();

            for (int i = 0; i < filePaths.Count; i++)
            {
                if (!EditorUtility.DisplayCancelableProgressBar("Installation", "Processing shaders " + i + "/" + filePaths.Count, i / (float)filePaths.Count))
                {
                    ToggleCodeBlock(filePaths[i], "urp", true);
                    ToggleCodeBlock(filePaths[i], "std", false);
                    //return;
                }
            }

            EditorUtility.ClearProgressBar();
        }

        public static void ToggleCodeBlock(string filePath, string id, bool enable)
        {
            string[] lines = File.ReadAllLines(filePath);

            List<CodeBlock> codeBlocks = new List<CodeBlock>();

            //Find start and end line indices
            for (int i = 0; i < lines.Length; i++)
            {
                bool blockEndReached = false;

                if (lines[i].Contains("Configuration: " + id) && enable)
                {
                    lines[i].Replace(lines[i], "//* Configuration: " + id);
                }

                if (lines[i].Contains("start " + id))
                {
                    CodeBlock codeBlock = new CodeBlock();

                    codeBlock.startLine = i;

                    //Find related end point
                    for (int l = codeBlock.startLine; l < lines.Length; l++)
                    {
                        if (blockEndReached == false)
                        {
                            if (lines[l].Contains("end " + id))
                            {
                                codeBlock.endLine = l;

                                blockEndReached = true;
                            }
                        }
                    }

                    codeBlocks.Add(codeBlock);
                    blockEndReached = false;
                }

            }

            if (codeBlocks.Count == 0)
            {
                //Debug.Log("No code blocks with the marker \"" + id + "\" were found in file");

                return;
            }

            foreach (CodeBlock codeBlock in codeBlocks)
            {
                if (codeBlock.startLine == codeBlock.endLine) continue;

                //Debug.Log((enable ? "Enabled" : "Disabled") + " \"" + id + "\" code block. Lines " + (codeBlock.startLine + 1) + " through " + (codeBlock.endLine + 1));

                for (int i = codeBlock.startLine + 1; i < codeBlock.endLine; i++)
                {
                    //Uncomment lines
                    if (enable == true)
                    {
                        if (lines[i].StartsWith("//") == true) lines[i] = lines[i].Remove(0, 2);
                    }
                    //Comment out lines
                    else
                    {
                        if (lines[i].StartsWith("//") == false) lines[i] = "//" + lines[i];
                    }
                }
            }

            File.WriteAllLines(filePath, lines);

            AssetDatabase.ImportAsset(filePath);
        }
    }
}