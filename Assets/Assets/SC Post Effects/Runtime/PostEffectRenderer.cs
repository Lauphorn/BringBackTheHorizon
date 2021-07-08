#if URP
using UnityEngine.Rendering.Universal;
#endif

using UnityEngine;
using UnityEngine.Rendering;

namespace SCPE
{
#if URP
    /// <summary>
    /// Base class for post processing ScriptableRenderPass
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PostEffectRenderer<T> : ScriptableRenderPass
    {
        /// <summary>
        /// VolumeComponent settings instance
        /// </summary>
        public T settings;
        public bool requireDepthNormals;

        public string shaderName;
        public string ProfilerTag;

        private Material m_Material;
        public Material Material
        {
            get
            {
                if (m_Material == null)
                {
                    if (!Shader.Find(shaderName)) Debug.LogError("Shader with the name <i>" + shaderName + "</i> could not be found, ensure all effect are files imported");
                    m_Material = CoreUtils.CreateEngineMaterial(Shader.Find(shaderName));
                }
                return m_Material;
            }
        }

        public RenderTextureDescriptor mainTexDesc;
        public int mainTexID;
        public int depthNormalsID;
        public RenderTargetIdentifier source;

        public static void RecreateBuffer(CommandBuffer cmd, int id, int downsampling)
        {
            
        }

        /// <summary>
        /// Sets up MainTex RT and depth normals if needed. Check if settings are valid before calling this base implementation
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="cameraTextureDescriptor"></param>
        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            mainTexDesc = cameraTextureDescriptor;
            cmd.GetTemporaryRT(mainTexID, mainTexDesc);

            if (requireDepthNormals)
            {
                depthNormalsID = Shader.PropertyToID("_DepthNormalsID");
                cmd.GetTemporaryRT(depthNormalsID, cameraTextureDescriptor);
            }
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {

        }

        /// <summary>
        /// Base implementation releases the MainTex RT
        /// </summary>
        /// <param name="cmd"></param>
        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(mainTexID);
        }

        private static Material m_DepthNormalsMat;
        private static Material DepthNormalsMat
        {
            get
            {
                if (m_DepthNormalsMat == null)
                {
                    if (!Shader.Find(ShaderNames.DepthNormals)) Debug.LogError("Shader with the name <i>" + ShaderNames.DepthNormals + "</i> could not be found, ensure all effect are files imported");
                    m_DepthNormalsMat = CoreUtils.CreateEngineMaterial(Shader.Find(ShaderNames.DepthNormals));
                }
                return m_DepthNormalsMat;
            }
        }

        /// <summary>
        /// Reconstructs geometry normals from depth
        /// </summary>
        /// <param name="pass"></param>
        /// <param name="cmd"></param>
        /// <param name="dest"></param>
        public static void GenerateDepthNormals(ScriptableRenderPass pass, CommandBuffer cmd, RenderTargetIdentifier dest)
        {
            pass.Blit(cmd, pass.depthAttachment, dest, DepthNormalsMat, 0);
            cmd.SetGlobalTexture("_CameraDepthNormalsTexture", dest);
        }

        /// <summary>
        /// Wrapper for ScriptableRenderPass.Blit but allows shaders to keep using _MainTex across render pipelines
        /// </summary>
        /// <param name="cmd">Command buffer to record command for execution.</param>
        /// <param name="source">Source texture or target identifier to blit from.</param>
        /// <param name="destination">Destination texture or target identifier to blit into. This becomes the renderer active render target.</param>
        /// <param name="material">Material to use.</param>
        /// <param name="passIndex">Shader pass to use. Default is 0.</param>
        public static void Blit(ScriptableRenderPass pass, CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier dest, Material mat, int passIndex)
        {
            cmd.SetGlobalTexture("_MainTex", source);
            pass.Blit(cmd, source, dest, mat, passIndex);
        }

        /// <summary>
        /// Blits and executes the command buffer
        /// </summary>
        /// <param name="pass"></param>
        /// <param name="context"></param>
        /// <param name="cmd"></param>
        /// <param name="source"></param>
        /// <param name="dest"></param>
        /// <param name="mat"></param>
        /// <param name="passIndex"></param>
        public static void FinalBlit(ScriptableRenderPass pass, ScriptableRenderContext context, CommandBuffer cmd, RenderTargetIdentifier source, RenderTargetIdentifier dest, Material mat, int passIndex)
        {
            Blit(pass, cmd, source, dest, mat, passIndex);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }
#endif
}