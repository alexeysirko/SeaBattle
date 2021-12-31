using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using Cars.Utils;
using OpenQA.Selenium;

namespace SeaBattleTest.Forms
{
    public class SeaBattleForm : Form
    {
        private static ILink randomRivalLink =
            ElementFactory.GetLink(By.XPath("//div[contains(@class,'battlefield-start-choose_rival')]//a[contains(@class,'choose_rival') and not(contains(@class,'connect'))]"), "randomRivalLink");
        private static IButton playButton =
            ElementFactory.GetButton(By.XPath("//div[contains(@class,'battlefield-start-button')]"), "playButton");
        private static IButton randomiseShipsButton =
            ElementFactory.GetButton(By.XPath("//*[contains(@class,'placeships-variant placeships-variant__randomly')]//*[contains(@class,'placeships-variant-link')]"), "randomiseShipsButton");
        private static ILabel rivalLoadingNotification =
            ElementFactory.GetLabel(By.XPath("//div[contains(@class,'notification__move-on') and not(contains(@class,'none'))]"), "rivalLoadingNotification");

        
        public SeaBattleForm() : base(randomRivalLink.Locator, "StartPageForm")
        {
        }

        public void ChooseRadnomRival()
        {
            randomRivalLink.Click();
        }

        public void ClickRandomiseShipsButtonRandomTimes(int fromTimes, int toTimes)
        {
            randomiseShipsButton.ClickRadnomTimes(fromTimes, toTimes);
        }

        public void ClickPlayLink()
        {
            playButton.Click();
        }

        public void WaitRivalToLoad()
        {
            rivalLoadingNotification.State.WaitForExist();
        }
    }
}
