using System;

namespace GalleyFramework.Extensions.Flow
{
    [Flags]
    public enum AbsFlags
    {
		None = 0,
		X = 1,
		Y = 2,
		Width = 4,
		Height = 8,
		Pos = 3,
		Size = 12,
		All = -1
    }
}
