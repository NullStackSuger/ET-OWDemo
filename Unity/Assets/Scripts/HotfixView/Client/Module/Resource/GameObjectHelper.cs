using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET.Client
{
	public static class GameObjectHelper
	{
		public static T Get<T>(this GameObject gameObject, string key) where T : class
		{
			try
			{
				return gameObject.GetComponent<ReferenceCollector>().Get<T>(key);
			}
			catch (Exception e)
			{
				throw new Exception($"获取{gameObject.name}的ReferenceCollector key失败, key: {key}", e);
			}
		}

		public static Dictionary<string, T> GetAll<T>(this GameObject gameObject) where T : class
		{
			try
			{
				return gameObject.GetComponent<ReferenceCollector>().GetAll<T>();
			}
			catch (Exception e)
			{
				throw new Exception($"获取{gameObject.name}的ReferenceCollector失败", e);
			}
		}
	}
}