using System;

namespace CodeMania.TestData.TestDataTypes
{
    [Flags]
    public enum Flags
    {
        None = 0,
        All = -1,
        One = 1,
        Two = 2,
        Four = 4,
        Eight = 8
    }
}