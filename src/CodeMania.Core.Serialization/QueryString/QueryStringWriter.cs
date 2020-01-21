using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace CodeMania.Core.Serialization.QueryString
{
	public sealed class QueryStringWriter : StringWriter
	{
		public QueryStringWriter(StringBuilder sb) : base(sb, NumberFormatInfo.InvariantInfo)
		{
		}

		public void Reset()
		{
			GetStringBuilder().Clear();
		}

		public void WriteProperty(string propertyName, IEnumerable<string> propertyValues)
		{
			if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

			WriteCollectionProperty(propertyName, propertyValues);
		}

		public void WriteProperty(string propertyName, List<string> propertyValues)
		{
			if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

			if (propertyValues != null)
			{
				WriteCollectionProperty(propertyName, propertyValues);
			}
		}

		public void WriteProperty(string propertyName, string[] propertyValues)
		{
			if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

			if (propertyValues != null)
			{
				WriteCollectionProperty(propertyName, propertyValues);
			}
		}

		public void WriteProperty(string propertyName, string propertyValue)
		{
			if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));

			if (propertyValue != null)
			{
				Write(propertyName);
				Write('=');
				Write(Uri.EscapeDataString(propertyValue));
				Write('&');
			}
		}

		private void WriteCollectionProperty<TCollection>(string propertyName, TCollection propertyValues)
			where TCollection : IEnumerable<string>
		{
			foreach (var propertyValue in propertyValues)
			{
				if (propertyValue == null) continue;

				Write(propertyName);
				Write('=');
				Write(Uri.EscapeDataString(propertyValue));
				Write('&');
			}
		}

		#region float

		public void WriteProperty(string propertyName, float item)
		{
			WriteProperty(propertyName, item.ToString(FormatProvider));
		}

		public void WriteProperty(string propertyName, IEnumerable<float> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		public void WriteProperty(string propertyName, List<float> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		public void WriteProperty(string propertyName, float[] items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		public void WriteProperty(string propertyName, float? item)
		{
			if (item.HasValue)
				WriteProperty(propertyName, item.Value.ToString(FormatProvider));
		}

		public void WriteProperty(string propertyName, IEnumerable<float?> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		public void WriteProperty(string propertyName, List<float?> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		public void WriteProperty(string propertyName, float?[] items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		#endregion

		#region double

		public void WriteProperty(string propertyName, double item)
		{
			WriteProperty(propertyName, item.ToString(FormatProvider));
		}

		public void WriteProperty(string propertyName, IEnumerable<double> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		public void WriteProperty(string propertyName, List<double> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		public void WriteProperty(string propertyName, double[] items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		public void WriteProperty(string propertyName, double? item)
		{
			if (item.HasValue)
				WriteProperty(propertyName, item.Value.ToString(FormatProvider));
		}

		public void WriteProperty(string propertyName, IEnumerable<double?> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		public void WriteProperty(string propertyName, List<double?> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		public void WriteProperty(string propertyName, double?[] items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		#endregion

		#region decimal

		public void WriteProperty(string propertyName, decimal item)
		{
			WriteProperty(propertyName, item.ToString(FormatProvider));
		}

		public void WriteProperty(string propertyName, IEnumerable<decimal> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		public void WriteProperty(string propertyName, decimal? item)
		{
			if (item.HasValue)
				WriteProperty(propertyName, item.Value.ToString(FormatProvider));
		}

		public void WriteProperty(string propertyName, IEnumerable<decimal?> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		#endregion

		#region DateTime

		public void WriteProperty(string propertyName, DateTime item)
		{
			if (item != default(DateTime))
				WriteProperty(propertyName, item.ToString("s"));
		}

		public void WriteProperty(string propertyName, IEnumerable<DateTime> items)
		{
			if (items != null)
			{
				foreach (var dateTime in items)
				{
					WriteProperty(propertyName, dateTime);
				}
			}
		}

		public void WriteProperty(string propertyName, List<DateTime> items)
		{
			if (items != null)
			{
				foreach (var dateTime in items)
				{
					WriteProperty(propertyName, dateTime);
				}
			}
		}

		public void WriteProperty(string propertyName, DateTime[] items)
		{
			if (items != null)
			{
				foreach (var dateTime in items)
				{
					WriteProperty(propertyName, dateTime);
				}
			}
		}

		public void WriteProperty(string propertyName, DateTime? item)
		{
			if (item.HasValue)
				WriteProperty(propertyName, item.Value.ToString("s"));
		}

		public void WriteProperty(string propertyName, IEnumerable<DateTime?> items)
		{
			if (items != null)
			{
				foreach (var dateTime in items)
				{
					WriteProperty(propertyName, dateTime);
				}
			}
		}

		#endregion

		#region DateTimeOffset

		public void WriteProperty(string propertyName, DateTimeOffset items)
		{
			if (items != default(DateTimeOffset))
				WriteProperty(propertyName, items.UtcDateTime.ToString("s") + "Z");
		}

		public void WriteProperty(string propertyName, IEnumerable<DateTimeOffset> items)
		{
			if (items != null)
			{
				foreach (var dateTime in items)
				{
					WriteProperty(propertyName, dateTime);
				}
			}
		}

		public void WriteProperty(string propertyName, DateTimeOffset? item)
		{
			if (item.HasValue)
				WriteProperty(propertyName, item.Value.ToString("s"));
		}

		public void WriteProperty(string propertyName, IEnumerable<DateTimeOffset?> items)
		{
			if (items != null)
			{
				foreach (var dateTime in items)
				{
					WriteProperty(propertyName, dateTime);
				}
			}
		}

		#endregion

		#region Guid

		public void WriteProperty(string propertyName, Guid item)
		{
			WriteProperty(propertyName, item.ToString("D"));
		}

		public void WriteProperty(string propertyName, IEnumerable<Guid> items)
		{
			if (items != null)
			{
				foreach (var guid in items)
				{
					WriteProperty(propertyName, guid);
				}
			}
		}

		public void WriteProperty(string propertyName, Guid? item)
		{
			if (item.HasValue)
				WriteProperty(propertyName, item.Value.ToString("D"));
		}

		public void WriteProperty(string propertyName, IEnumerable<Guid?> items)
		{
			if (items != null)
			{
				foreach (var guid in items)
				{
					WriteProperty(propertyName, guid);
				}
			}
		}

		#endregion

		#region char

		public void WriteProperty(string propertyName, char item)
		{
			WriteProperty(propertyName, item.ToString());
		}

		public void WriteProperty(string propertyName, IEnumerable<char> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		public void WriteProperty(string propertyName, char? item)
		{
			if (item.HasValue)
				WriteProperty(propertyName, item.ToString());
		}

		public void WriteProperty(string propertyName, IEnumerable<char?> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		#endregion

		#region TimeSpan

		public void WriteProperty(string propertyName, TimeSpan item)
		{
			WriteProperty(propertyName, item.ToString());
		}

		public void WriteProperty(string propertyName, IEnumerable<TimeSpan> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		public void WriteProperty(string propertyName, TimeSpan? item)
		{
			if (item.HasValue)
				WriteProperty(propertyName, item.ToString());
		}

		public void WriteProperty(string propertyName, IEnumerable<TimeSpan?> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		#endregion

		#region bool

		public void WriteProperty(string propertyName, bool item)
		{
			WriteProperty(propertyName, item.ToString());
		}

		public void WriteProperty(string propertyName, IEnumerable<bool> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		public void WriteProperty(string propertyName, List<bool> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		public void WriteProperty(string propertyName, bool[] items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		public void WriteProperty(string propertyName, bool? item)
		{
			if (item.HasValue)
				WriteProperty(propertyName, item.ToString());
		}

		public void WriteProperty(string propertyName, IEnumerable<bool?> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		#endregion

		#region sbyte

		public void WriteProperty(string propertyName, sbyte item)
		{
			WriteProperty(propertyName, item.ToString());
		}

		public void WriteProperty(string propertyName, IEnumerable<sbyte> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		public void WriteProperty(string propertyName, sbyte? item)
		{
			if (item.HasValue)
				WriteProperty(propertyName, item.ToString());
		}

		public void WriteProperty(string propertyName, IEnumerable<sbyte?> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		#endregion

		#region byte

		public void WriteProperty(string propertyName, byte item)
		{
			WriteProperty(propertyName, item.ToString());
		}

		public void WriteProperty(string propertyName, IEnumerable<byte> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		public void WriteProperty(string propertyName, List<byte> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		public void WriteProperty(string propertyName, byte[] items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		public void WriteProperty(string propertyName, byte? item)
		{
			if (item.HasValue)
				WriteProperty(propertyName, item.ToString());
		}

		public void WriteProperty(string propertyName, IEnumerable<byte?> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		#endregion

		#region short

		public void WriteProperty(string propertyName, short item)
		{
			WriteProperty(propertyName, item.ToString());
		}

		public void WriteProperty(string propertyName, IEnumerable<short> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		public void WriteProperty(string propertyName, short? item)
		{
			if (item.HasValue)
				WriteProperty(propertyName, item.ToString());
		}

		public void WriteProperty(string propertyName, IEnumerable<short?> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		#endregion

		#region ushort

		public void WriteProperty(string propertyName, ushort item)
		{
			WriteProperty(propertyName, item.ToString());
		}

		public void WriteProperty(string propertyName, IEnumerable<ushort> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		public void WriteProperty(string propertyName, ushort? item)
		{
			if (item.HasValue)
				WriteProperty(propertyName, item.ToString());
		}

		public void WriteProperty(string propertyName, IEnumerable<ushort?> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		#endregion

		#region int

		public void WriteProperty(string propertyName, int item)
		{
			WriteProperty(propertyName, item.ToString());
		}

		public void WriteProperty(string propertyName, IEnumerable<int> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		public void WriteProperty(string propertyName, List<int> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		public void WriteProperty(string propertyName, int[] items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		public void WriteProperty(string propertyName, int? item)
		{
			if (item.HasValue)
				WriteProperty(propertyName, item.ToString());
		}

		public void WriteProperty(string propertyName, IEnumerable<int?> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		#endregion

		#region uint

		public void WriteProperty(string propertyName, uint item)
		{
			WriteProperty(propertyName, item.ToString());
		}

		public void WriteProperty(string propertyName, IEnumerable<uint> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		public void WriteProperty(string propertyName, uint? item)
		{
			if (item.HasValue)
				WriteProperty(propertyName, item.ToString());
		}

		public void WriteProperty(string propertyName, IEnumerable<uint?> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		#endregion

		#region long

		public void WriteProperty(string propertyName, long item)
		{
			WriteProperty(propertyName, item.ToString());
		}

		public void WriteProperty(string propertyName, IEnumerable<long> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		public void WriteProperty(string propertyName, long? item)
		{
			if (item.HasValue)
				WriteProperty(propertyName, item.ToString());
		}

		public void WriteProperty(string propertyName, IEnumerable<long?> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		#endregion

		#region ulong

		public void WriteProperty(string propertyName, ulong item)
		{
			WriteProperty(propertyName, item.ToString());
		}

		public void WriteProperty(string propertyName, IEnumerable<ulong> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		public void WriteProperty(string propertyName, ulong? item)
		{
			if (item.HasValue)
				WriteProperty(propertyName, item.ToString());
		}

		public void WriteProperty(string propertyName, IEnumerable<ulong?> items)
		{
			if (items != null)
			{
				foreach (var item in items)
				{
					WriteProperty(propertyName, item);
				}
			}
		}

		#endregion
	}
}