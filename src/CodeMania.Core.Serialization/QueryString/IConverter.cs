namespace CodeMania.Core.Serialization.QueryString
{
	public interface IConverter<in TSource, out TDestination>
	{
		TDestination Convert(TSource source);
	}
}