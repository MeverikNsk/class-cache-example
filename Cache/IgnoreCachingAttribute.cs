namespace ClassCache.Cache
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class IgnoreCachingAttribute : Attribute
    {        
    }
}
