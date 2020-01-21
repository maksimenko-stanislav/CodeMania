using System.Runtime.CompilerServices;

namespace CodeMania.Core.Internals
{
	// TODO: Use System.HashCode after moving to netstandard 2.1
	/// <summary>
	/// Class contains helper methods for hash code calculations.
	/// </summary>
	public static class HashHelper
	{
		public const int HashSeed = 5381; // (obtained from String.GetHashCode implementation)

		// From System.Web.Util.HashCodeCombiner

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int CombineHashCodes(int h1, int h2) => unchecked(((h1 << 5) + h1) ^ h2);
	}
}