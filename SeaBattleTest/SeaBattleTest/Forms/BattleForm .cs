using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using OpenQA.Selenium;

namespace SeaBattleTest.Forms
{
    public class BattleForm : Form
    {
        private static ILabel rivalLoadingNotification 
            = ElementFactory.GetLabel(By.XPath("//div[contains(@class,'notification__move-on') and not(contains(@class,'none'))]"), "rivalLoadingNotification");
        private IButton battleFieldCell(int x, int y)
            => ElementFactory.GetButton(By.XPath($"//div[contains(@class,'battlefield__rival')]//div[contains(@class,'battlefield-cell-content') and @data-y='{y}' and @data-x='{x}']"), "battleFieldCell");
        private void waitForRivalFieldToBeAvailable()
            => ElementFactory.GetLabel(By.XPath("//div[contains(@class,'battlefield__rival') and contains(@class,'battlefield__wait')]"), "waitForRivalFieldToBeAvailable")
            .State.WaitForNotExist();
        private bool isLastHit 
            => ElementFactory.GetLabel(By.XPath("//div[contains(@class,'battlefield__rival')]//*[contains(@class,'battlefield-cell') and contains(@class,'last') and contains(@class,'hit')]"), "isLastHit")
            .State.IsExist;
        private bool isShipDone
            => ElementFactory.GetLabel(By.XPath("//div[contains(@class,'battlefield__rival')]//*[contains(@class,'battlefield-cell') and contains(@class,'last') and contains(@class,'done')]"), "isShipDone")
            .State.IsExist;
        private bool isCellEmpty(int x, int y)
            => ElementFactory.GetLabel(By.XPath($"//div[contains(@class,'battlefield__rival')]//div[contains(@class,'battlefield-cell-content') and @data-y='{y}' and @data-x='{x}']//ancestor::*[contains(@class,'empty')]"), "isCellEmpty")
            .State.IsExist;

        

        public BattleForm() : base(rivalLoadingNotification.Locator, "BattleForm")
        {
        }

        public void WaitRivalToLoad()
        {
            rivalLoadingNotification.State.WaitForExist();
        }

        public void ClickCells(int x, int y)
        {
            while (true)
            {
                for (int i = 0; i < 10; i++)
                {
                    
                    if (isLastHit)
                    {
                        FinishOffShip(x, y);
                        return;
                    }
                }            
            }
        }

        private void ClickCell(int x, int y)
        {
            waitForRivalFieldToBeAvailable();
            battleFieldCell(x, y).ClickAndWait();
        }

        private void FinishOffShip(int cellX, int cellY)
        {
            while(isShipDone)
            {
                //Right
                if (isCellEmpty(cellX+1, cellY))
                {
                    ClickCell(cellX + 1, cellY);
                    if(isLastHit)
                    {
                        FinishOffShip(cellX + 1, cellY);
                    }
                    
                }
            }
        }
    }
}
