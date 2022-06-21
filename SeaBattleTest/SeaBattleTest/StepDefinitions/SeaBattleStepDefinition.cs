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
        private ScenarioContext scenarioContext;

        public SeaBattleStepDefinition(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }

        [Given(@"I start a game with random rival with randomly setted ships")]
        public void GivenIStartAGameWithRandomRivalWithRandomlySettedShips()
        {
            AqualityServices.Browser.GoTo(TestSettings.mainPageLink);
            var startForm = new StartForm();
            startForm.ChooseRadnomRival();
            startForm.ClickRandomiseShipsButtonRandomTimes(TestSettings.shuffleShipsFrom, TestSettings.shuffleShipsTo);
            startForm.ClickPlayLink();
            new BattleForm().WaitRivalToLoad();
        }

        [When(@"I play seaWars and save game results as '([^']*)' and '([^']*)'")]
        public void WhenIPlaySeaWarsAndSaveGameResultsAsAnd(string gameResultMessage, string isWin)
        {
            var battleForm = new BattleForm();
            bool isGameOverWin = false;
            string gameResult = battleForm.PlaySeaBattle(ref isGameOverWin);
            scenarioContext[isWin] = isGameOverWin;
            scenarioContext[gameResultMessage] = gameResult;
        }

        [Then(@"I assert results of the game based on '([^']*)' and '([^']*)'")]
        public void ThenISeeResultsOfTheGameBasedOnAnd(string gameResultMessage, string isWin)
        {
            Assert.IsTrue((bool)scenarioContext[isWin], (string)scenarioContext[gameResultMessage]);
        }
    }
}
