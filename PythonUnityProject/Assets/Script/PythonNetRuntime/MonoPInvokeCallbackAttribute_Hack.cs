using System;

namespace Python.Runtime
{
	[AttributeUsage(AttributeTargets.Method)]
	public class MonoPInvokeCallbackAttribute : Attribute
	{
		private Type type;

		public MonoPInvokeCallbackAttribute(Type t)
		{
			this.type = t;
		}
	}
}
