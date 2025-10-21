namespace AtemSharp.Helpers;

[AttributeUsage(AttributeTargets.Class)]
public class BufferSizeAttribute : Attribute
{
    public BufferSizeAttribute(int size)
    {
        Size = size;
    }

    public int Size { get; }
}
