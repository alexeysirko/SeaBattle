using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattleTest.TestData
{
    public static class TestSettings
    {
        public static string mainPageLink = "http://ru.battleship-game.org";
        public static int shuffleShipsFrom = 1;
        public static int shuffleShipsTo = 15;
        public static TimeSpan rivalMaxWait = TimeSpan.FromSeconds(20);
        public static TimeSpan htmlChangingWait = TimeSpan.FromSeconds(0.7);
    }
}
