using System;
using System.Reflection;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace CodeMania.Core.Serialization.QueryString
{
	public static class QueryStringSerializerBuilderExtensions
	{
		public static QueryStringSerializerBuilder<T> UseDataContracts<T>([NotNull] this QueryStringSerializerBuilder<T> builder)
		{
			if (builder == null) throw new ArgumentNullException(nameof(builder));

			return builder.WithNames(p => p.GetCustomAttribute<DataMemberAttribute>()?.Name ?? p.Name);
		}
	}
}