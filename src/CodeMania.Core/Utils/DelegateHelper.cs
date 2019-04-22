using System;
using System.Reflection;

namespace CodeMania.Core.Utils
{
	internal static class DelegateHelper
	{
		public static TDelegate CreateDelegate<TDelegate>(MethodInfo methodInfo)
			where TDelegate : Delegate =>
				(TDelegate) Delegate.CreateDelegate(typeof(TDelegate), methodInfo);
	}
}