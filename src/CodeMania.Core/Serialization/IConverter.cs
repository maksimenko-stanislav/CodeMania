namespace CodeMania.Core.Serialization
{
	public interface IConverter<in TSource, out TDestination>
	{
		TDestination Convert(TSource source);
	}
}