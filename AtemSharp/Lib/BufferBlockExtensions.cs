using System.Diagnostics;
using System.Threading.Tasks.Dataflow;

namespace AtemSharp.Lib;

public static class BufferBlockExtensions
{
    public static async Task<BufferBlock<T>> Recreate<T>(this BufferBlock<T> self)
    {
        try
        {
            self.Complete();
            await self.Completion;
        } catch (Exception ex) {
            Debug.Print($"Error while recreating BufferBlock<{typeof(T).Name}: {ex}");
        }

        return new BufferBlock<T>();
    }
}
