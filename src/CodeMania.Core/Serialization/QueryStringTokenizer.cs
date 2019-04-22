using System;
using System.Text.RegularExpressions;

namespace CodeMania.Core.Serialization
{
	public sealed class QueryStringTokenizer
	{
		private string queryString;
		private int pos;
		private int length;

		private static readonly Regex UriRegex = new Regex(
			@"^([a-z0-9+.-]+):(?://(?:((?:[a-z0-9-._~!$&'()*+,;=:]|%[0-9A-F]{2})*)@)?((?:[a-z0-9-._~!$&'()*+,;=]|%[0-9A-F]{2})*)(?::(\d*))?(/(?:[a-z0-9-._~!$&'()*+,;=:@/]|%[0-9A-F]{2})*)?|(/?(?:[a-z0-9-._~!$&'()*+,;=:@]|%[0-9A-F]{2})+(?:[a-z0-9-._~!$&'()*+,;=:@/]|%[0-9A-F]{2})*)?)(?:\?((?:[a-z0-9-._~!$&'()*+,;=:/?@]|%[0-9A-F]{2})*))?(?:#((?:[a-z0-9-._~!$&'()*+,;=:/?@]|%[0-9A-F]{2})*))?$",
			RegexOptions.Compiled | RegexOptions.Singleline);

		internal QueryStringTokenizer()
		{
		}

		public QueryStringTokenizer(string queryString)
		{
			Initialize(queryString);
		}

		internal void Initialize(string str)
		{
			queryString = str ?? throw new ArgumentNullException(nameof(str));

			pos = 0;

			var anchorSeparatorPos = queryString.IndexOf('#');
			length = anchorSeparatorPos > -1 ? anchorSeparatorPos + 1 : queryString.Length;

			if (UriRegex.IsMatch(queryString) || queryString.IndexOf('?') > -1)
			{
				ReadWhile('?');
			}
		}

		internal void Reset()
		{
			queryString = null;
			pos = length = 0;
		}

		public void ResetPosition()
		{
			pos = 0;
		}

		/// <summary>
		/// Reads content of internal string and provides parsed key and value as output parameters if available.
		/// </summary>
		/// <param name="key">Key from query string as output parameter.</param>
		/// <param name="value">Value from query string for <paramref name="key"/> as output parameter.</param>
		/// <returns>True if reading was successful and there is content in the rest of query string, otherwise false</returns>
		/// <remarks>
		/// To read all key/value pairs from query string consumers of this method should call it while it returns true.
		/// </remarks>
		public bool Read(out ReadOnlyMemory<char> key, out ReadOnlyMemory<char> value)
		{
			ReadOnlyMemory<char> temp;

			var token = ReadToken(out temp);

			switch (token)
			{
				case TokenType.EndOfString:
					{
						key = temp;
						ReadToken(out value);
						return !key.IsEmpty;
					}
				case TokenType.Key:
					{
						key = temp;
						ReadToken(out value);
						break;
					}
				case TokenType.Value:
					{
						value = temp;
						key = ReadOnlyMemory<char>.Empty;
						break;
					}
				default:
					{
						// unexpected case
						key = value = ReadOnlyMemory<char>.Empty;
						break;
					}
			}

			return token != TokenType.EndOfString;
		}

		private void ReadWhile(char ch)
		{
			if (pos < length)
			{
				int idx = pos;

				while (idx < length && queryString[idx] != '#' && queryString[idx] != ch) idx++;

				pos = idx + 1;
			}
		}

		private TokenType ReadToken(out ReadOnlyMemory<char> result)
		{
			TokenType tokenType = TokenType.EndOfString;
			if (pos >= length)
			{
				result = ReadOnlyMemory<char>.Empty;

				return tokenType;
			}

			int idx = pos, startPos = pos;

			while (idx < length)
			{
				if (queryString[idx] == '=')
				{
					tokenType = TokenType.Key;
					break;
				}

				if (queryString[idx] == '&')
				{
					tokenType = TokenType.Value;
					break;
				}

				if (queryString[idx] == '#')
				{
					tokenType = TokenType.EndOfString;
					break;
				}

				idx++;
			}

			result = queryString.AsMemory(startPos, idx - startPos);

			pos = idx + 1;

			return tokenType;
		}

		private enum TokenType
		{
			EndOfString,
			Key,
			Value
		}
	}
}