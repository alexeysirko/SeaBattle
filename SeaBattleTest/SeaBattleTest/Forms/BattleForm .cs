using Aquality.Selenium.Browsers;
using Aquality.Selenium.Core.Elements;
using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using OpenQA.Selenium;
using SeaBattleTest.TestData;
using SeaBattleTest.Utils;

namespace SeaBattleTest.Forms
{
    public class BattleForm : Form
    {
        private const string rivalFieldXPath = "//div[contains(@class,'battlefield__rival')]";

        private static IList<ILabel> BattlefieldTableRows
            = ElementFactory.FindElements<ILabel>(By.XPath($"{rivalFieldXPath}//table[contains(@class,'battlefield-table')]//*[contains(@class,'battlefield-row')]"), "BattlefieldTableRows");

        private static IButton battleFieldCell(int x, int y)
             => ElementFactory.GetButton(By.XPath($"{rivalFieldXPath}//div[contains(@class,'battlefield-cell-content') and @data-y='{y}' and @data-x='{x}']"), $"battleFieldCell({x},{y})", ElementState.ExistsInAnyState);

        private ILabel rivalFieldInWaitingCondition
            => ElementFactory.GetLabel(By.XPath("//div[contains(@class,'battlefield__rival') and contains(@class,'battlefield__wait')]"), "rivalFieldInWaitingCondition");

        private ILabel hitCell(int x, int y)
            => ElementFactory.GetLabel(By.XPath($"{rivalFieldXPath}//div[contains(@class,'battlefield-cell-content') and @data-y='{y}' and @data-x='{x}']//ancestor::*[contains(@class,'hit')]"), $"hitCell({x},{y})");

        private ILabel doneShip
            => ElementFactory.GetLabel(By.XPath($"{rivalFieldXPath}//*[contains(@class,'battlefield-cell') and contains(@class,'last') and contains(@class,'done')]"), "doneShip");

        private ILabel emptyCell(int x, int y)
            => ElementFactory.GetLabel(By.XPath($"{rivalFieldXPath}//div[contains(@class,'battlefield-cell-content') and @data-y='{y}' and @data-x='{x}']//ancestor::*[contains(@class,'empty')]"), $"emptyCell({x},{y})");

        private ILabel rivalLeave
            => ElementFactory.GetLabel(By.XPath("//div[contains(@class,'notification__rival-leave') and not(contains(@class,'none'))]"), "rivalLeave");

        private ILabel gameOverWin
            => ElementFactory.GetLabel(By.XPath("//div[contains(@class,'notification__game-over-win') and not(contains(@class,'none'))]"), "gameOverWin");

        private ILabel gameOverLose
            => ElementFactory.GetLabel(By.XPath("//div[contains(@class,'notification__game-over-lose') and not(contains(@class,'none'))]"), "gameOverLose");

        private ILabel gameContinue
            => ElementFactory.GetLabel(By.XPath("//div[contains(@class,'notification__move') and not(contains(@class,'none'))]"), "gameContinue");

        private readonly int fieldSize = BattlefieldTableRows.Count;

        public BattleForm() : base(battleFieldCell(0, 0).Locator, "BattleForm")
        {
        }


        private bool isRivalLeave()
        {
            return rivalLeave.State.IsExist;
        }

        private bool isGameOverWin()
        {
            return gameOverWin.State.IsExist;
        }

        private bool isGameOverLose()
        {
            return gameOverLose.State.IsExist;
        }

        private bool isGameContinue()
        {
            return gameContinue.State.IsExist;
        }

        private bool isCellEmpty(int x, int y)
        {
            return emptyCell(x, y).State.IsExist;
        }

        private bool isCellHit(int x, int y)
        {
            return hitCell(x,y).State.WaitForExist(TestSettings.htmlChangingWait);
        }

        private bool isShipDone()
        {
             return doneShip.State.WaitForExist(TestSettings.htmlChangingWait);
        }

        private bool waitForRivalFieldToBeAvailable()
        {
            return rivalFieldInWaitingCondition.State.WaitForNotExist(TestSettings.rivalMaxWait);
        }

        private void Shoot(int x, int y)
        {
            CheckGameStatus();
            if (ClickCell(x, y))
            {
                FinishOffShip(x, y);
            }
        }

        /// <returns>True, if shot is hit</returns>
        private bool ClickCell(int x, int y)
        {
            if (waitForRivalFieldToBeAvailable() && isCellEmpty(x, y))
            {
                waitForRivalFieldToBeAvailable();
                battleFieldCell(x, y).WaitAndClick();
            }

            return isCellHit(x, y);
        }

        private void FinishOffShip(int cellX, int cellY)
        {
            hitRigthRecursive(cellX, cellY);
            hitLeftRecursive(cellX, cellY);
            hitDownRecursive(cellX, cellY);
            hitUpRecursive(cellX, cellY);
        }

        private void hitRigthRecursive(int cellX, int cellY)
        {
            if (cellX + 1 < fieldSize
                && isCellEmpty(cellX + 1, cellY)
                && !isShipDone())
            {
                if (ClickCell(cellX + 1, cellY))
                {
                    hitRigthRecursive(cellX + 1, cellY);
                }
            }
        }

        private void hitLeftRecursive(int cellX, int cellY)
        {
            if (cellX - 1 >= 0
                && isCellEmpty(cellX - 1, cellY)
                && !isShipDone())
            {
                if (ClickCell(cellX - 1, cellY))
                {
                    hitLeftRecursive(cellX - 1, cellY);
                }
            }
        }

        private void hitDownRecursive(int cellX, int cellY)
        {
            if (cellY + 1 < fieldSize
                && isCellEmpty(cellX, cellY + 1)
                && !isShipDone())
            {
                if (ClickCell(cellX, cellY + 1))
                {
                    hitDownRecursive(cellX, cellY + 1);
                }
            }
        }

        private void hitUpRecursive(int cellX, int cellY)
        {
            if (cellY - 1 >= 0
                && isCellEmpty(cellX, cellY - 1)
                && !isShipDone())
            {
                if (ClickCell(cellX, cellY - 1))
                {
                    hitUpRecursive(cellX, cellY - 1);
                }
            }
        }

        private void FillRandomRow(int rowIndex)
        {
            for (int x = 0, y = rowIndex; x < fieldSize; x++)
            {
                Shoot(x, y);
            }
        }

        private string GetGameStatus(ref bool isWin)
        {
            if (isRivalLeave())
            {
                return "Game Over. Rival left the game";
            }

            if (isGameOverWin())
            {
                isWin = true;
                return "Game Over. Win";
            }

            if (isGameOverLose())
            {
                return "Game Over. Lose";
            }
            return "Game Over. Unknown reason.";
        }

        private void CheckGameStatus()
        {
            if (!isGameContinue())
            {
                throw new Exception("GameOver");
            }
        }


        public void WaitRivalToLoad()
        {
            AqualityServices.ConditionalWait.WaitForTrue(() => isGameContinue(), TestSettings.rivalMaxWait);
        }

        public string PlaySeaBattle(ref bool isWin)
        {
            try
            {
                //From left up to right down
                for (int x = 0, y = 0; y < fieldSize; x++, y++)
                {
                    Shoot(x, y);
                }

                //From right up to left down
                for (int x = fieldSize - 1, y = 0; y < fieldSize; x--, ++y)
                {
                    Shoot(x, y);
                }

                //Fill Unic Radnom rows
                var randomRowsIndexes = DataGenerator.GenerateUnicRandomNumbers(fieldSize);
                foreach (var item in randomRowsIndexes)
                {
                    FillRandomRow(item);
                }
            }
            catch
            {
                return GetGameStatus(ref isWin);
            }
            return "Game Over. Unknown reason.";
        }
    }
}
