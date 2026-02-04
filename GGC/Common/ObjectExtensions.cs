namespace GGC.Common
{
    public static class ObjectExtensions
    {
        public static int ToInt(this object obj, int defaultValue = 0)
        {
            if (obj == null)
                return defaultValue;

            if (obj.GetType() == typeof(int))
                return (int)obj;

            if (obj.GetType() == typeof(string))
            {
                if (int.TryParse((string)obj, out int parsedValue))
                    return parsedValue;
            }

            return defaultValue;
        }
    }
}