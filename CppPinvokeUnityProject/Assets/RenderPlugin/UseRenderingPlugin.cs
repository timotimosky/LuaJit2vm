using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.Rendering;
using static PInvokeDelegate;
using static DLLManager;


public class UseRenderingPlugin : MonoBehaviour
{

#if (PLATFORM_IOS || PLATFORM_TVOS || PLATFORM_BRATWURST || PLATFORM_SWITCH) && !UNITY_EDITOR
     public  const string _dllName = DLLName__Internal;
#elif UNITY_IPHONE
    public  const string _dllName = DLLName__Internal;
#elif UNITY_EDITOR_OSX
    public  const string _dllName = "/RenderingPlugin.bundle/Contents/MacOS/RenderingPlugin";
#elif UNITY_EDITOR_LINUX
    public  const string _dllName = "/RenderingPlugin.so";
#elif UNITY_STANDALONE_WIN
  public const string _dllName = "libnativeparticles";
#endif



#if (PLATFORM_IOS || PLATFORM_TVOS || PLATFORM_BRATWURST || PLATFORM_SWITCH) && !UNITY_EDITOR
    [DllImport("__Internal")]
        private static extern void SetTimeFromUnity(float t);
          [DllImport(__Internal)]
  private static extern void SetTextureFromUnity(System.IntPtr texture, int w, int h);
    [DllImport(__Internal)]
  private static extern IntPtr GetRenderEventFunc();
#else
  [DllImport(_dllName)]
  private static extern void SetTimeFromUnity(float t);
  [DllImport(_dllName)]
  private static extern void SetTextureFromUnity(System.IntPtr texture, int w, int h);
  [DllImport(_dllName)]
  private static extern IntPtr GetRenderEventFunc();
#endif



  // We'll pass native pointer to the mesh vertex buffer.
  // Also passing source unmodified mesh data.
  // The plugin will fill vertex data from native code.
#if (PLATFORM_IOS || PLATFORM_TVOS || PLATFORM_BRATWURST || PLATFORM_SWITCH) && !UNITY_EDITOR
    [DllImport("__Internal")]
#else
  [DllImport(_dllName)]
#endif
    private static extern void SetMeshBuffersFromUnity(IntPtr vertexBuffer, int vertexCount, IntPtr sourceVertices, IntPtr sourceNormals, IntPtr sourceUVs);


#if PLATFORM_SWITCH && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void RegisterPlugin();
#endif

    // DX12 plugin has a few additional exported functions

#if (UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_WSA || UNITY_WSA_10_0)
    [DllImport(_dllName)]
    private static extern IntPtr GetRenderTexture();


    [DllImport(_dllName)]
    private static extern bool IsSwapChainAvailable();

    [DllImport(_dllName)]
    private static extern uint GetPresentFlags();

    [DllImport(_dllName)]
    private static extern uint GetBackBufferWidth();


//#if UNITY_EDITOR
//  DLLManager.Delegate_uint GetSyncInterval;
//  DLLManager.Delegate_void_IntPtr SetRenderTexture;
//#else
      [DllImport(_dllName)]
    private static extern uint GetSyncInterval();
        [DllImport(_dllName)]
    private static extern void SetRenderTexture(IntPtr rb);
//#endif



  [DllImport(_dllName)]
    private static extern uint GetBackBufferHeight();

    private static RenderTexture renderTex;
    private static GameObject pluginInfo;

    private void CreateRenderTexture()
    {
        renderTex = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
        renderTex.Create();
        IntPtr nativeBufPTr = renderTex.colorBuffer.GetNativeRenderBufferPtr();
        SetRenderTexture(nativeBufPTr);
        GameObject sphere = GameObject.Find("Sphere");
        sphere.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        sphere.GetComponent<Renderer>().material.mainTexture = renderTex;
    }

#else
    private void CreateRenderTexture() { }
    private void SetRenderTexture(IntPtr rb) { }
#endif

  private void CreateTextureAndPassToPlugin()
  {
    // Create a texture
    Texture2D tex = new Texture2D(256, 256, TextureFormat.ARGB32, false);
    // Set point filtering just so we can see the pixels clearly
    tex.filterMode = FilterMode.Point;
    // Call Apply() so it's actually uploaded to the GPU
    tex.Apply();

    // Set texture onto our material
    GetComponent<Renderer>().material.mainTexture = tex;

    // Pass texture pointer to the plugin
    SetTextureFromUnity(tex.GetNativeTexturePtr(), tex.width, tex.height);
  }


  public IntPtr libarayHandle;

  private void Awake()
  {
#if UNITY_EDITOR
    //open the native library
   // libarayHandle = DLLManager.OpenLibrary(Application.dataPath + "\\Plugins\\x86_64\\" + _dllName);

   // GetSyncInterval = DLLManager.GetDelegate<Delegate_uint>(libarayHandle, "GetSyncInterval");
  //  SetRenderTexture = DLLManager.GetDelegate<Delegate_void_IntPtr>(libarayHandle, "SetRenderTexture");
#endif
  }

  void OnApplicationQuit()
  {
#if UNITY_EDITOR
   // FreeLibrary(libarayHandle);
    //libarayHandle = IntPtr.Zero;
#endif
  }

  IEnumerator Start()
    {
#if PLATFORM_SWITCH && !UNITY_EDITOR
        RegisterPlugin();
#endif

    if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D12)
    {
      Debug.LogError("现在只兼容了DX11.DX12会闪退");
      yield break;
      CreateRenderTexture();
    }
    else
    {
      Debug.LogError(SystemInfo.graphicsDeviceType);
     // yield break;  
    }

        CreateTextureAndPassToPlugin();
        SendMeshBuffersToPlugin();
        yield return StartCoroutine(CallPluginAtEndOfFrames());
    }

    void OnDisable()
    {
        if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D12)
        {
            // Signals the plugin that renderTex will be destroyed
            SetRenderTexture(IntPtr.Zero);
        }
    }



    private void SendMeshBuffersToPlugin()
    {
        var filter = GetComponent<MeshFilter>();
        var mesh = filter.mesh;

        // This is equivalent to MeshVertex in RenderingPlugin.cpp
        var desiredVertexLayout = new[]
        {
            new VertexAttributeDescriptor(VertexAttribute.Position, VertexAttributeFormat.Float32, 3),
            new VertexAttributeDescriptor(VertexAttribute.Normal, VertexAttributeFormat.Float32, 3),
            new VertexAttributeDescriptor(VertexAttribute.Color, VertexAttributeFormat.Float32, 4),
            new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2)
        };

        // Let's be certain we'll get the vertex buffer layout we want in native code
        mesh.SetVertexBufferParams(mesh.vertexCount, desiredVertexLayout);

        // The plugin will want to modify the vertex buffer -- on many platforms
        // for that to work we have to mark mesh as "dynamic" (which makes the buffers CPU writable --
        // by default they are immutable and only GPU-readable).
        mesh.MarkDynamic();

        // However, mesh being dynamic also means that the CPU on most platforms can not
        // read from the vertex buffer. Our plugin also wants original mesh data,
        // so let's pass it as pointers to regular C# arrays.
        // This bit shows how to pass array pointers to native plugins without doing an expensive
        // copy: you have to get a GCHandle, and get raw address of that.
        var vertices = mesh.vertices;
        var normals = mesh.normals;
        var uvs = mesh.uv;
        GCHandle gcVertices = GCHandle.Alloc(vertices, GCHandleType.Pinned);
        GCHandle gcNormals = GCHandle.Alloc(normals, GCHandleType.Pinned);
        GCHandle gcUV = GCHandle.Alloc(uvs, GCHandleType.Pinned);

        SetMeshBuffersFromUnity(mesh.GetNativeVertexBufferPtr(0), mesh.vertexCount, gcVertices.AddrOfPinnedObject(), gcNormals.AddrOfPinnedObject(), gcUV.AddrOfPinnedObject());

        gcVertices.Free();
        gcNormals.Free();
        gcUV.Free();
    }

    // custom "time" for deterministic results
    int updateTimeCounter = 0;

    private IEnumerator CallPluginAtEndOfFrames()
    {
        while (true)
        {
            // Wait until all frame rendering is done
            yield return new WaitForEndOfFrame();

            // Set time for the plugin
            // Unless it is D3D12 whose renderer is allowed to overlap with the next frame update and would cause instabilities due to no synchronization.
            // On Switch, with multithreaded mode, setting the time for frame 1 may overlap with the render thread calling the plugin event for frame 0 resulting in the plugin writing data for frame 1 at frame 0.
            if (SystemInfo.graphicsDeviceType != GraphicsDeviceType.Direct3D12 && SystemInfo.graphicsDeviceType != GraphicsDeviceType.Switch)
            {
                ++updateTimeCounter;
                SetTimeFromUnity((float)updateTimeCounter * 0.016f);
            }

            // Issue a plugin event with arbitrary integer identifier.
            // The plugin can distinguish between different
            // things it needs to do based on this ID.
            // On some backends the choice of eventID matters e.g on DX12 where
            // eventID == 1 means the plugin callback will be called from the render thread
            // and eventID == 2 means the callback is called from the submission thread
            GL.IssuePluginEvent(GetRenderEventFunc(), 1);

            if (SystemInfo.graphicsDeviceType == GraphicsDeviceType.Direct3D12)
            {
                GL.IssuePluginEvent(GetRenderEventFunc(), 2);
            }
        }
    }
}
