using System.Runtime.CompilerServices;

namespace AtemSharp.Tests;

public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Init()
    {
        UseProjectRelativeDirectory("VerifySnapshots");
    }
}
