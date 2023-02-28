namespace enovaApi.Proxy
{
    public static class Extensions
    {
        public static async Task<string> GetString(this Stream stream)
        {
            return await new StreamReader(stream).ReadToEndAsync();
        }
    }
}
