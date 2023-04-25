namespace ClassCache.Cache.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class IgnoreCachingAttribute : Attribute
    {
    }
}
