using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class UnityNativeParticles : MonoBehaviour {
#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport("__Internal")]
#else
	[DllImport("libnativeparticles")]
#endif
	static extern void flx_init(System.Int32 particle_qty);

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport("__Internal")]
#else
	[DllImport("libnativeparticles")]
#endif
	static extern void flx_update(int eventId, float time, float delta_time);

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport("__Internal")]
#else
	[DllImport("libnativeparticles")]
#endif
	static extern System.IntPtr flx_get_render_event_func();

#if (UNITY_IPHONE || UNITY_WEBGL) && !UNITY_EDITOR
	[DllImport("__Internal")]
#else
	[DllImport("libnativeparticles")]
#endif
	static extern void flx_set_mvp(float[] model, float[] view, float[] projection);

	//void Awake()
	//{
	//	flx_init(10000);
	//}

	IEnumerator Start()
	{
		yield return StartCoroutine(CallPluginAtEndOfFrames());
	}

	void Update()
	{
		flx_update(1, Time.time, Time.deltaTime);
	}

	void LateUpdate()
	{ }

	IEnumerator CallPluginAtEndOfFrames()
	{
		while (true) {
			yield return new WaitForEndOfFrame();

			flx_set_mvp(Mat4ToFloat16Row(transform.localToWorldMatrix),
					Mat4ToFloat16Row(Camera.main.worldToCameraMatrix),
					Mat4ToFloat16Row(Camera.main.projectionMatrix));

			// Issue a plugin event with arbitrary integer identifier.
			// The plugin can distinguish between different
			// things it needs to do based on this ID.
			// For our simple plugin, it does not matter which ID we pass here.
			GL.IssuePluginEvent(flx_get_render_event_func(), 1);
		}
	}

	float[] Mat4ToFloat16(Matrix4x4 mat)
	{
		return new float[16] {
			mat.m00, mat.m01, mat.m02, mat.m03,
			mat.m10, mat.m11, mat.m12, mat.m13,
			mat.m20, mat.m21, mat.m22, mat.m23,
			mat.m30, mat.m31, mat.m32, mat.m33
		};
	}

	float[] Mat4ToFloat16Row(Matrix4x4 mat)
	{
		return new float[16] {
			mat.m00, mat.m10, mat.m20, mat.m30,
			mat.m01, mat.m11, mat.m21, mat.m31,
			mat.m02, mat.m12, mat.m22, mat.m32,
			mat.m03, mat.m13, mat.m23, mat.m33
		};
	}
}
