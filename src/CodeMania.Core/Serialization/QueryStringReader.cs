//using System;
//using System.Globalization;
//using System.IO;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace CodeMania.Core.Serialization
//{
//	public sealed class QueryStringReader : TextReader
//	{
//		private string _s;
//		private int _pos;
//		private int _length;

//		private static readonly NumberFormatInfo NumberFormat = NumberFormatInfo.InvariantInfo;

//		// see https://stackoverflow.com/questions/30847/regex-to-validate-uris
//		private static readonly Regex UriRegex = new Regex(
//			@"^([a-z0-9+.-]+):(?://(?:((?:[a-z0-9-._~!$&'()*+,;=:]|%[0-9A-F]{2})*)@)?((?:[a-z0-9-._~!$&'()*+,;=]|%[0-9A-F]{2})*)(?::(\d*))?(/(?:[a-z0-9-._~!$&'()*+,;=:@/]|%[0-9A-F]{2})*)?|(/?(?:[a-z0-9-._~!$&'()*+,;=:@]|%[0-9A-F]{2})+(?:[a-z0-9-._~!$&'()*+,;=:@/]|%[0-9A-F]{2})*)?)(?:\?((?:[a-z0-9-._~!$&'()*+,;=:/?@]|%[0-9A-F]{2})*))?(?:#((?:[a-z0-9-._~!$&'()*+,;=:/?@]|%[0-9A-F]{2})*))?$",
//			RegexOptions.Compiled | RegexOptions.Singleline);


//		public QueryStringReader(string str)
//		{
//			_s = str ?? throw new ArgumentNullException(nameof(str));

//			_pos = 0;
//			var anchorSeparatorPos = _s.IndexOf('#');
//			_length = anchorSeparatorPos > -1 ? anchorSeparatorPos + 1 : _s.Length;

//			if (UriRegex.IsMatch(str) || _s.IndexOf('?') > -1)
//			{
//				ReadWhile('?');
//			}
//		}

//		public string ReadPropertyName()
//		{
//			return Uri.UnescapeDataString(ReadWhile('='));
//		}

//		public string ReadValue()
//		{
//			return Uri.UnescapeDataString(ReadWhile('&'));
//		}

//		public byte ReadByte()
//		{
//			return byte.Parse(ReadValue(), NumberFormat);
//		}

//		public sbyte ReadSByte()
//		{
//			return sbyte.Parse(ReadValue(), NumberFormat);
//		}

//		public short ReadInt16()
//		{
//			return short.Parse(ReadValue(), NumberFormat);
//		}

//		public ushort ReadUInt16()
//		{
//			return ushort.Parse(ReadValue(), NumberFormat);
//		}

//		public int ReadInt32()
//		{
//			return int.Parse(ReadValue(), NumberFormat);
//		}

//		public uint ReadUInt32()
//		{
//			return uint.Parse(ReadValue(), NumberFormat);
//		}

//		public long ReadInt64()
//		{
//			return long.Parse(ReadValue(), NumberFormat);
//		}

//		public ulong ReadUInt64()
//		{
//			return ulong.Parse(ReadValue(), NumberFormat);
//		}

//		public float ReadSingle()
//		{
//			return float.Parse(ReadValue(), NumberFormat);
//		}

//		public double ReadDouble()
//		{
//			return double.Parse(ReadValue(), NumberFormat);
//		}

//		public Guid ReadGuid()
//		{
//			return Guid.Parse(ReadValue());
//		}

//		public decimal ReadDecimal()
//		{
//			return decimal.Parse(ReadValue(), NumberFormat);
//		}

//		public DateTime ReadDateTime()
//		{
//			// see also
//			// DateTime.ParseExact("       " + DateTime.Now.ToString("O") + "     ", new string[] { "s", "O" }, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AdjustToUniversal).Kind
//			return DateTime.ParseExact(ReadValue(), "s", CultureInfo.InvariantCulture);
//		}

//		public string ReadWhile(char ch)
//		{
//			if (_pos < _length)
//			{
//				int idx = _pos, startPos = _pos;

//				while (idx < _length && _s[idx] != '#' && _s[idx] != ch) idx++;

//				var result = _s.Substring(startPos, idx - startPos);

//				_pos = idx + 1;

//				return result;
//			}

//			return string.Empty;
//		}

//		public ReadOnlyMemory<char> ReadWhileAsMemory(char ch)
//		{
//			if (_pos < _length)
//			{
//				int idx = _pos, startPos = _pos;

//				while (idx < _length && _s[idx] != '#' && _s[idx] != ch) idx++;

//				var result = _s.AsMemory(startPos, idx - startPos);

//				_pos = idx + 1;

//				return result;
//			}

//			return string.Empty.AsMemory();
//		}

//		public override void Close()
//		{
//			Dispose(true);
//		}

//		public override int Peek()
//		{
//			if (_s == null)
//				throw new InvalidOperationException("The reader is closed.");
//			if (_pos == _length || _s[_pos] == '#')
//				return -1;
//			return _s[_pos];
//		}

//		public override int Read()
//		{
//			if (_s == null)
//				throw new InvalidOperationException("The reader is closed.");
//			if (_pos == _length || _s[_pos] == '#')
//				return -1;
//			return _s[_pos++];
//		}

//		public override int Read(char[] buffer, int index, int count)
//		{
//			if (buffer == null)
//				throw new ArgumentNullException(nameof(buffer));
//			if (index < 0)
//				throw new ArgumentOutOfRangeException(nameof(index), "");
//			if (count < 0)
//				throw new ArgumentOutOfRangeException(nameof(count), "");
//			if (buffer.Length - index < count)
//				throw new ArgumentException("Invalid offset");
//			if (_s == null)
//				throw new InvalidOperationException("The reader is closed.");

//			int availableSize = _length - _pos;

//			if (availableSize > 0)
//			{
//				if (availableSize > count)
//					availableSize = count;

//				_s.CopyTo(_pos, buffer, index, availableSize);

//				_pos += availableSize;
//			}

//			return availableSize;
//		}

//		public override string ReadLine()
//		{
//			if (_s == null)
//				throw new InvalidOperationException("The reader is closed.");
//			int pos;
//			for (pos = _pos; pos < _length; ++pos)
//			{
//				char ch = _s[pos];
//				switch (ch)
//				{
//					case '\n':
//					case '\r':
//						string str = _s.Substring(_pos, pos - _pos);
//						_pos = pos + 1;
//						if (ch == '\r' && _pos < _length && _s[_pos] == '\n')
//							++_pos;
//						return str;
//					default:
//						continue;
//				}
//			}
//			if (pos <= _pos)
//				return null;
//			string str1 = _s.Substring(_pos, pos - _pos);
//			_pos = pos;
//			return str1;
//		}

//		public override string ReadToEnd()
//		{
//			if (_s == null)
//				throw new InvalidOperationException("The reader is closed.");
//			string str = _pos != 0 ? _s.Substring(_pos, _length - _pos) : _s;
//			_pos = _length;
//			return str;
//		}

//		public override Task<string> ReadLineAsync()
//		{
//			return Task.FromResult(ReadLine());
//		}

//		public override Task<string> ReadToEndAsync()
//		{
//			return Task.FromResult(ReadToEnd());
//		}

//		public override Task<int> ReadBlockAsync(char[] buffer, int index, int count)
//		{
//			if (buffer == null)
//				throw new ArgumentNullException(nameof(buffer));
//			if (index < 0 || count < 0)
//				throw new ArgumentOutOfRangeException(index < 0 ? nameof(index) : nameof(count), "");
//			if (buffer.Length - index < count)
//				throw new ArgumentException("");
//			return Task.FromResult(ReadBlock(buffer, index, count));
//		}

//		public override Task<int> ReadAsync(char[] buffer, int index, int count)
//		{
//			if (buffer == null)
//				throw new ArgumentNullException(nameof(buffer));
//			if (index < 0 || count < 0)
//				throw new ArgumentOutOfRangeException(index < 0 ? nameof(index) : nameof(count), "");
//			if (buffer.Length - index < count)
//				throw new ArgumentException("");
//			return Task.FromResult(Read(buffer, index, count));
//		}

//		protected override void Dispose(bool disposing)
//		{
//			_s = default(string);
//			_pos = 0;
//			_length = 0;
//			base.Dispose(disposing);
//		}
//	}
//}