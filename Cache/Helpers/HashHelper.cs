namespace ClassCache.Cache.Helpers
{
    using System.Text;
    using System.Text.Json;
    using XSystem.Security.Cryptography;

    public static class HashHelper
    {
        public static string GetHash(IDictionary<string, object> cacheRequest)
        {
            var hash = new List<string>();

            foreach (var kvp in cacheRequest)
            {
                var obj = kvp.Value as IClassCachHash;

                if (obj != null)
                {
                    hash.Add($"{kvp.Key}_{obj.GetHash()}");
                }
                else
                {
                    var jsonStr = JsonSerializer.Serialize(kvp.Value);
                    var tmpSource = Encoding.ASCII.GetBytes(jsonStr);
                    byte[] tmpNewHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
                    hash.Add($"{kvp.Key}_{ByteArrayToString(tmpNewHash)}");
                }

            }

            var hashJsonStr = JsonSerializer.Serialize(hash);
            return hashJsonStr;
        }

        public static string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
        }
    }
}
