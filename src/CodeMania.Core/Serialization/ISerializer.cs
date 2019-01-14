namespace CodeMania.Core.Serialization
{
	public interface ISerializer<TSerializationData>
	{
		TSerializationData Serialize<T>(T item);
		T Deserialize<T>(TSerializationData serializationData);
	}

	public interface IStringSerializer : ISerializer<string>
	{
	}
}