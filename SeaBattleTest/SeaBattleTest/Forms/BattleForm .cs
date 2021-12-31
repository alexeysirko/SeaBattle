using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using Cars.Utils;
using OpenQA.Selenium;

namespace SeaBattleTest.Forms
{
    public class BattleForm : Form
    {
        private static ILabel rivalLoadingNotification =
            ElementFactory.GetLabel(By.XPath("//div[contains(@class,'notification__move-on') and not(contains(@class,'none'))]"), "rivalLoadingNotification");

        //TODO: table locator
        //table[contains(@class,'battlefield-table')]

        public BattleForm() : base(rivalLoadingNotification.Locator, "BattleForm")
        {
        }

        public void WaitRivalToLoad()
        {
            rivalLoadingNotification.State.WaitForExist();
        }
    }
}
