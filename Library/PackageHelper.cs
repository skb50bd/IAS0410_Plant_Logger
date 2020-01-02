using Package = 
    System.Collections.Generic.Dictionary<
        string, 
        System.Collections.Generic.Dictionary<string, string>>;

namespace IAS04110
{
    public static class PackageHelper
    {
        public static string Serialize(
            this Package package)
        {
            var output = string.Empty;
            foreach (var (key, value) in package)
            {
                output += $"{key}:\n";
                foreach (var (k, v) in value)
                    output += $"{k}: {v}\n";
            }
            return output;
        }
    }
}
