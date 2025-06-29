//#pragma once

//#include "RenderLoopEnums.h"
//#include "Runtime/GfxDevice/GfxDeviceTypes.h"

//class Shader;
//struct RenderLoop;
//class Camera;
//class ImageFilters;
//class RenderTexture;
//struct ShadowCullData;
//struct CullResults;
//struct ShadowMapCache;
//struct SharedRendererScene;
//namespace ShaderLab { struct FastPropertyName; }


//RenderLoop* CreateRenderLoop(Camera& camera);

//void InitializeRenderLoopContext(Camera* camera, const SharedRendererScene& rendererScene, RenderLoop* renderLoop);

//void DeleteRenderLoop(RenderLoop* loop);
//void DoRenderLoop(
//    RenderLoop& loop,
//    RenderingPath renderPath,
//    CullResults& contents,
//    ShadowMapCache& shadowCache);
//void CleanupAfterRenderLoop(RenderLoop& loop);
//bool IsInitializedRenderLoop(RenderLoop& loop);

//ImageFilters & GetRenderLoopImageFilters(RenderLoop & loop);
//void RenderImageFilters(RenderLoop& loop, bool afterOpaque);

//UNITY_INLINE int GetRenderLoopDefaultDepthSlice(SinglePassStereo spsMode)
//{
//#if GFX_SUPPORTS_SINGLE_PASS_STEREO
//    bool bindAllSlices = (spsMode == kSinglePassStereoInstancing) || (spsMode == kSinglePassStereoMultiview);
//    return bindAllSlices ? kTextureArraySliceAll : 0;
//#else
//    return 0;
//#endif
//}
//// return the next power-of-two of a 64bit number
//inline UInt64 NextPowerOfTwo64(UInt64 v)
//{
//    v -= 1;
//    v |= v >> 32;
//    v |= v >> 16;
//    v |= v >> 8;
//    v |= v >> 4;
//    v |= v >> 2;
//    v |= v >> 1;
//    return v + 1;
//}