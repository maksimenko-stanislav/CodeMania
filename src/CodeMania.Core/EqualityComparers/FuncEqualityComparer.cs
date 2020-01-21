using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace CodeMania.Core.EqualityComparers
{
	public sealed class FuncEqualityComparer<T> : EqualityComparer<T>
	{
		private readonly Func<T, T, bool> equalsFunc;
		private readonly Func<T, int> getHashCodeFunc;

		public FuncEqualityComparer([NotNull] Func<T, T, bool> equalsFunc, [NotNull] Func<T, int> getHashCodeFunc)
		{
			this.equalsFunc = equalsFunc ?? throw new ArgumentNullException(nameof(equalsFunc));
			this.getHashCodeFunc = getHashCodeFunc ?? throw new ArgumentNullException(nameof(getHashCodeFunc));
		}

		public override bool Equals(T x, T y) => equalsFunc(x, y);

		public override int GetHashCode(T obj) => getHashCodeFunc(obj);

		public static FuncEqualityComparer<T> Create([NotNull] Func<T, T, bool> equalsFunc, [NotNull] Func<T, int> getHashCodeFunc) =>
			new FuncEqualityComparer<T>(
				equalsFunc ?? throw new ArgumentNullException(nameof(equalsFunc)),
				getHashCodeFunc ?? throw new ArgumentNullException(nameof(getHashCodeFunc)));
	}
}