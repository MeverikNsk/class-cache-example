namespace ClassCache.Cache
{
    using System.Reflection;

    public interface ICacheDurationProvider
    {
        TimeSpan GetDuration(MethodInfo methodInfo);
    }
}
