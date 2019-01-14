using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace CodeMania.Core.Extensions
{
	public static class MemberInfoExtensions
	{
		public static bool IsInstancePropertyOf([NotNull] this PropertyInfo propertyInfo, [NotNull] Type type)
		{
			if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));
			if (type == null) throw new ArgumentNullException(nameof(type));

			return type.GetProperties(BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance)
				.Contains(propertyInfo);
		}
	}
}