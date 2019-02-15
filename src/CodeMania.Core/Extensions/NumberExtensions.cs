namespace CodeMania.Core.Extensions
{
	public static class NumberExtensions
	{
		public static bool IsOnlyOneBitSet(this sbyte number) => number != 0 && (number & (number - 1)) == 0;

		public static bool IsOnlyOneBitSet(this byte number) => number != 0 && (number & (number - 1)) == 0;

		public static bool IsOnlyOneBitSet(this short number) => number != 0 && (number & (number - 1)) == 0;

		public static bool IsOnlyOneBitSet(this ushort number) => number != 0 && (number & (number - 1)) == 0;

		public static bool IsOnlyOneBitSet(this int number) => number != 0 && (number & (number - 1)) == 0;

		public static bool IsOnlyOneBitSet(this uint number) => number != 0 && (number & (number - 1)) == 0;

		public static bool IsOnlyOneBitSet(this long number) => number != 0 && (number & (number - 1)) == 0;

		public static bool IsOnlyOneBitSet(this ulong number) => number != 0 && (number & (number - 1)) == 0;
	}
}