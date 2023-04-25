namespace ClassCache.Cache.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class CacheDurationAttribute : Attribute
    {
        public CacheDurationAttribute(int days, int hours, int minutes, int seconds, int millisecond)
        {
            Duration = new TimeSpan(days, hours, minutes, seconds, millisecond);
        }

        public CacheDurationAttribute(int days, int hours, int minutes, int seconds)
        {
            Duration = new TimeSpan(days, hours, minutes, seconds);
        }

        public CacheDurationAttribute(long ticks)
        {
            Duration = new TimeSpan(ticks);
        }

        public CacheDurationAttribute(int hours, int minutes, int seconds)
        {
            Duration = new TimeSpan(hours, minutes, seconds);
        }

        public TimeSpan Duration { get; private set; }
    }
}
