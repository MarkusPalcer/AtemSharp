using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace AtemSharp.Lib;

internal interface IActionLoopFactory
{
    public ActionLoop Start(Func<CancellationToken, Task> loopedAction, ILogger logger,
                            [CallerArgumentExpression(nameof(loopedAction))] string name = null!);
}
