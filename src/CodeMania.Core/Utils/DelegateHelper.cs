using System;
using System.Reflection;
using JetBrains.Annotations;

namespace CodeMania.Core.Utils
{
	public static class DelegateHelper
	{
		public static TDelegate CreateDelegate<TDelegate>([NotNull] MethodInfo methodInfo) where TDelegate : Delegate =>
				(TDelegate) Delegate.CreateDelegate(typeof(TDelegate), methodInfo ?? throw new ArgumentNullException(nameof(methodInfo)));
	}
}