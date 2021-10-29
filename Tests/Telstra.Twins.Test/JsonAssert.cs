using Xunit;

namespace Telstra.Twins.Test
{
    public static class JsonAssert
    {
        private static readonly JsonStringComparer JsonStringComparer = new JsonStringComparer();
        public static void Equal(string a, string b) => Assert.Equal(a, b, JsonStringComparer);
        public static void NotEqual(string a, string b) => Assert.NotEqual(a, b, JsonStringComparer);
    }
}
