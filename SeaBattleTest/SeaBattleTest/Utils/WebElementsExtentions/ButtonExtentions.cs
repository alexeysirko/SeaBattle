using Aquality.Selenium.Elements.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cars.Utils
{
    public static class ButtonExtentions
    {
        public static void ClickRadnomTimes(this IButton button, int fromTimes, int toTimes)
        {
            int randomTimes = new Random().Next(fromTimes, toTimes);
            for (int i = 0; i < randomTimes; i++)
            {
                button.Click();
            }
        }
    }
}
