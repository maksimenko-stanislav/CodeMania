using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace CodeMania.FastLinq.UnitTests
{
    public class FastLinqWhereTests
    {
        [Test]
        public void Parametrized_NonChained_ReturnsExpectedSequence()
        {
            var numbers = new List<long> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var modDivisor = 2L;
            var comparand = 0L;

            var expected = numbers.Where(x => x % modDivisor == comparand);
            var actual = numbers.Where((modDivisor, comparand), (x, arg) => x % arg.modDivisor == arg.comparand);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void Parametrized_WhereToWhereChain_ReturnsExpectedSequence()
        {
            var numbers = new List<long> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var modDivisor = 2L;
            var comparand = 0L;

            var expected = numbers.Where(x => x % modDivisor == comparand).Where(x => x > 2);
            var actual = numbers.Where((modDivisor, comparand), (x, arg) => x % arg.modDivisor == arg.comparand).Where(x => x > 2);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void Parametrized_WhereToSelectChain_ReturnsExpectedSequence()
        {
            var numbers = new List<long> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var modDivisor = 2L;
            var comparand = 0L;

            var expected = numbers.Where(x => x % modDivisor == comparand).Select(x => x + 1);
            var actual = numbers.Where((modDivisor, comparand), (x, arg) => x % arg.modDivisor == arg.comparand).Select(x => x + 1);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void Parametrized_WhereToSelectToWhereChain_ReturnsExpectedSequence()
        {
            var numbers = new List<long> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            var modDivisor = 2L;
            var comparand = 0L;

            var expected = numbers.Where(x => x % modDivisor == comparand).Select(x => x + 1).Where(x => x < 7);
            var actual = numbers.Where((modDivisor, comparand), (x, arg) => x % arg.modDivisor == arg.comparand).Select(x => x + 1).Where(x => x < 7);

            CollectionAssert.AreEqual(expected, actual);
        }

        // TODO: add other tests
    }
}