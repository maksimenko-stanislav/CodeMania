using System;

namespace CodeMania.Core.Serialization.QueryString
{
	public delegate TResult ValueConverter<in TObj, in TProperty, out TResult>(TObj obj, TProperty objPropertyValue);

	public delegate TResult ValueConverter<in TValue, out TResult>(TValue objPropertyValue);

	public delegate bool TryParseDelegate<in TSettings, TResult>(ReadOnlyMemory<char> value, TSettings settings, out TResult result);
}
