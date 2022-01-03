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
        private static IButton battleFieldCell(int x, int y)
             => ElementFactory.GetButton(By.XPath($"//div[contains(@class,'battlefield__rival')]//div[contains(@class,'battlefield-cell-content') and @data-y='{y}' and @data-x='{x}']"), $"battleFieldCell({x},{y})", ElementState.ExistsInAnyState);

        private ILabel rivalIsLoadingNotification
            = ElementFactory.GetLabel(By.XPath("//div[contains(@class,'notification__waiting-for-rival') and not(contains(@class,'none'))]"), "rivalLoadingNotification");

        private readonly int fieldSize
            = ElementFactory.FindElements<ILabel>(By.XPath("//div[contains(@class,'battlefield__rival')]//table[contains(@class,'battlefield-table')]//*[contains(@class,'battlefield-row')]"), "fieldSize")
            .Count;

        private bool waitForRivalFieldToBeAvailable()
            => ElementFactory.GetLabel(By.XPath("//div[contains(@class,'battlefield__rival') and contains(@class,'battlefield__wait')]"), "waitForRivalFieldToBeAvailable")
            .State.WaitForNotExist(TestSettings.rivalMaxWait);

        private bool isLastHit
            => ElementFactory.GetLabel(By.XPath("//div[contains(@class,'battlefield__rival')]//*[contains(@class,'battlefield-cell') and contains(@class,'last') and contains(@class,'hit')]"), "isLastHit")
            .State.WaitForExist(TestSettings.htmlChangingWait);

        private bool isShipDone
            => ElementFactory.GetLabel(By.XPath("//div[contains(@class,'battlefield__rival')]//*[contains(@class,'battlefield-cell') and contains(@class,'last') and contains(@class,'done')]"), "isShipDone")
            .State.WaitForExist(TestSettings.htmlChangingWait);
       
        private bool isCellEmpty(int x, int y)
            => ElementFactory.GetLabel(By.XPath($"//div[contains(@class,'battlefield__rival')]//div[contains(@class,'battlefield-cell-content') and @data-y='{y}' and @data-x='{x}']//ancestor::*[contains(@class,'empty')]"), $"isCellEmpty({x},{y})")
            .State.WaitForExist(TestSettings.htmlChangingWait);
        
        private bool isRivalLeave
            => ElementFactory.GetLabel(By.XPath("//div[contains(@class,'notification__rival-leave') and not(contains(@class,'none'))]"), "isGameOver")
            .State.WaitForExist(TestSettings.htmlChangingWait);
       
        private bool isGameOverWin
            => ElementFactory.GetLabel(By.XPath("//div[contains(@class,'notification__game-over-win') and not(contains(@class,'none'))]"), "isGameOverWin")
            .State.WaitForExist(TestSettings.htmlChangingWait);
       
        private bool isGameOverLose
            => ElementFactory.GetLabel(By.XPath("//div[contains(@class,'notification__game-over-lose') and not(contains(@class,'none'))]"), "isGameOverLose")
            .State.WaitForExist(TestSettings.htmlChangingWait);

        private bool isGameContinue
            => ElementFactory.GetLabel(By.XPath("//div[contains(@class,'notification__move') and not(contains(@class,'none'))]"), "isGameContinue")
            .State.WaitForExist(TestSettings.htmlChangingWait);

        public BattleForm() : base(battleFieldCell(0, 0).Locator, "BattleForm")
        {
        }

        public void WaitRivalToLoad()
        {
            rivalIsLoadingNotification.State.WaitForNotExist();
        }

        public string PlaySeaBattle(ref bool isWin)
        {
            try
            {
                //From left up to right down
                for (int x = 0, y = 0; y < fieldSize; x++, y++)
                {
                    ClickCell(x, y);
                    if (isLastHit && !isShipDone)
                    {
                        FinishOffShip(x, y);
                    }
                }

                //From right up to left down
                for (int x = fieldSize - 1, y = 0; y < fieldSize; x--, ++y)
                {
                    ClickCell(x, y);
                    if (isLastHit && !isShipDone)
                    {
                        FinishOffShip(x, y);
                    }
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

        private void ClickCell(int x, int y)
        {         
            if (waitForRivalFieldToBeAvailable() && isCellEmpty(x, y))
            {
                battleFieldCell(x, y).State.WaitForClickable();
                waitForRivalFieldToBeAvailable();
                battleFieldCell(x, y).WaitAndClick();
            }
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
                && !isShipDone)
            {               
                ClickCell(cellX + 1, cellY);
                if(isLastHit)
                {
                    hitRigthRecursive(cellX + 1, cellY);
                }
            }
        }

        private void hitLeftRecursive(int cellX, int cellY)
        {
            if (cellX - 1 >= 0
                && isCellEmpty(cellX - 1, cellY)
                && !isShipDone)
            {
                ClickCell(cellX - 1, cellY);
                if (isLastHit)
                {
                    hitLeftRecursive(cellX - 1, cellY);
                }
            }
        }

        private void hitDownRecursive(int cellX, int cellY)
        {
            if (cellY + 1 < fieldSize                
                && isCellEmpty(cellX, cellY + 1)
                && !isShipDone)
            {
                ClickCell(cellX, cellY + 1);
                if (isLastHit)
                {
                    hitDownRecursive(cellX, cellY + 1);
                }             
            }
        }

        private void hitUpRecursive(int cellX, int cellY)
        {
            if (cellY - 1 >= 0
                && isCellEmpty(cellX, cellY - 1)
                && !isShipDone)
            {
                ClickCell(cellX, cellY - 1);
                if (isLastHit)
                {
                    hitUpRecursive(cellX, cellY - 1);
                }
            }
        }

        private void FillRandomRow(int rowIndex)
        {
            for (int x = 0, y = rowIndex; x < fieldSize - 1; x++)
            {
                ClickCell(x, y);
                if (isLastHit && !isShipDone)
                {
                    FinishOffShip(x, y);
                }
            }
        }

        private string GetGameStatus(ref bool isWin)
        {
            if (isRivalLeave)
            {
                return "Game Over. Rival left the game";
            }

            if (isGameOverWin)
            {
                isWin = true;
                return "Game Over. Win";
            }

            if (isGameOverLose)
            {
                return "Game Over. Lose";
            }
            return "Game Over. Unknown reason.";
        }

        private void CheckGameStatus()
        {
            if (!isGameContinue)
            {
                throw new Exception("GameOver");
            }
        }
    }
}
