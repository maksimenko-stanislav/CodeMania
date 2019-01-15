using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace CodeMania.Core.Benchmarks.Utils
{
	public static class DeepCopy
	{
		public static T Create<T>(T source)
		{
			BinaryFormatter formatter = new BinaryFormatter();

			using (var ms = new MemoryStream())
			{
				formatter.Serialize(ms, source);

				ms.Position = 0;

				return (T) formatter.Deserialize(ms);
			}
		}
	}
}