using System;
using UnityEngine;

namespace Python.Runtime
{
	internal static partial class Util
	{
		/// <summary>
		/// 读取Resources下的文本资源
		/// </summary>
		/// <param name="resources_relative_path"></param>
		/// <exception cref="ArgumentNullException"></exception>
		internal static string ReadResourcesText(string resources_relative_path)
		{
			if (string.IsNullOrEmpty(resources_relative_path))
			{
				throw new ArgumentNullException(nameof(resources_relative_path));
			}
			
			TextAsset ta = Resources.Load<TextAsset>(resources_relative_path);
			if (ta == null)
			{
                Debug.LogError($"[Util::ReadResourcesText] path error [{resources_relative_path}]");
				return "";
			}
			
			string result = ta.text;
			Resources.UnloadAsset(ta);
			return result;
		}
	}
}
