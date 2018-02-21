using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Console.Tests
{
    public static class TestingExtensions
    {
        public static IEnumerable<T> AsEnumerable<T>(this T target)
        {
            yield return target;
        }
    }
}
