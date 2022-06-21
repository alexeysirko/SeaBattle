using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattleTest.Utils
{
    internal static class DataGenerator
    {
        public static List<int> GenerateUnicRandomNumbers(int maxValue)
        {
            var rand = new Random();
            var ints = Enumerable.Range(0, maxValue)
                                         .Select(i => new Tuple<int, int>(rand.Next(maxValue), i))
                                         .OrderBy(i => i.Item1)
                                         .Select(i => i.Item2);
            return ints.ToList();
        }
    }
}
