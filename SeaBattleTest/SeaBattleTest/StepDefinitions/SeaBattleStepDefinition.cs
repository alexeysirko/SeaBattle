using Aquality.Selenium.Browsers;
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
            new SeaBattleForm().ChooseRadnomRival();
        }

        [Given(@"I randomly set ships")]
        public void GivenIRandomlySetShips()
        {
            new SeaBattleForm().ClickRandomiseShipsButtonRandomTimes(TestSettings.shuffleShipsFrom, TestSettings.shuffleShipsTo);
        }

        [Given(@"I click play button")]
        public void GivenIClickPlayButton()
        {
            new SeaBattleForm().ClickPlayLink();
        }

        [Given(@"I wait untill enemy is loaded")]
        public void GivenIWaitUntillEnemyIsLoaded()
        {
            new SeaBattleForm().WaitRivalToLoad();
        }

        [Given(@"I play seaWars")]
        public void GivenIPlaySeaWars()
        {
            throw new PendingStepException();
        }
    }
}
