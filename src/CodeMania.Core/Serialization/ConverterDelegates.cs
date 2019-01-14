using System;

namespace CodeMania.Core.Serialization
{
	/// <summary>
	/// Delegate to convert property value of object <typeparamref name="TObj"/> to <see cref="String"/>.
	/// </summary>
	/// <typeparam name="TObj">Type of object which contains property of type <typeparamref name="TProperty"/>.</typeparam>
	/// <typeparam name="TProperty">Type of property.</typeparam>
	/// <typeparam name="TResult"></typeparam>
	/// <param name="obj">Instance of object which contain property <typeparamref name="TProperty"/>.</param>
	/// <param name="objPropertyValue">Property value.</param>
	/// <returns>Serialized result.</returns>
	public delegate TResult ValueConverter<in TObj, in TProperty, out TResult>(TObj obj, TProperty objPropertyValue);

	public delegate TResult ValueConverter<in TValue, out TResult>(TValue objPropertyValue);
}
