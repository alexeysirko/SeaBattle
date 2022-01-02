using Aquality.Selenium.Browsers;
using NUnit.Framework;
using SeaBattleTest.Forms;
using SeaBattleTest.TestData;
using TechTalk.SpecFlow;

namespace SeaBattleTest.StepDefinitions
{
    [Binding]
    public class SeaBattleStepDefinition
    {
        [Given(@"I move to start page")]
        public void GivenIMoveToMainPage()
        {
            AqualityServices.Browser.GoTo(TestSettings.mainPageLink);
        }

        [Given(@"I choose random rival")]
        public void GivenIChooseRandomEnemy()
        {
            new StartForm().ChooseRadnomRival();
        }

        [Given(@"I randomly set ships")]
        public void GivenIRandomlySetShips()
        {
            new StartForm().ClickRandomiseShipsButtonRandomTimes(TestSettings.shuffleShipsFrom, TestSettings.shuffleShipsTo);
        }

        [Given(@"I click play button")]
        public void GivenIClickPlayButton()
        {
            new StartForm().ClickPlayLink();
        }

        [Given(@"I wait untill enemy is loaded")]
        public void GivenIWaitUntillEnemyIsLoaded()
        {
            new BattleForm().WaitRivalToLoad();
        }

        [Given(@"I play seaWars")]
        public void GivenIPlaySeaWars()
        {
            var battleForm = new BattleForm();
            battleForm.ClickCells(0,0);

            Assert.Fail();
        }
    }
}
