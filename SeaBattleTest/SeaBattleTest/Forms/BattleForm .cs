using Aquality.Selenium.Elements.Interfaces;
using Aquality.Selenium.Forms;
using OpenQA.Selenium;

namespace SeaBattleTest.Forms
{
    public class BattleForm : Form
    {
        private static IButton battleFieldCell(int x, int y)
             => ElementFactory.GetButton(By.XPath($"//div[contains(@class,'battlefield__rival')]//div[contains(@class,'battlefield-cell-content') and @data-y='{y}' and @data-x='{x}']"), "battleFieldCell");
        
        private ILabel rivalIsLoadingNotification 
            = ElementFactory.GetLabel(By.XPath("//div[contains(@class,'notification__waiting-for-rival') and not(contains(@class,'none'))]"), "rivalLoadingNotification");

        private readonly int fieldSize
            = ElementFactory.FindElements<ILabel>(By.XPath("//div[contains(@class,'battlefield__rival')]//table[contains(@class,'battlefield-table')]//*[contains(@class,'battlefield-row')]"), "fieldSize")
            .Count;

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
       

        public BattleForm() : base(battleFieldCell(0,0).Locator, "BattleForm")
        {
        }

        public void WaitRivalToLoad()
        {
            rivalIsLoadingNotification.State.WaitForNotExist();
        }

        public void PlaySeaBattle()
        {
            for (int x = 0,y = 0; y < fieldSize; x++,y++)
            {
                ClickCell(x, y);
                if (isLastHit)
                {
                    FinishOffShip(x, y);
                }
            }                   
        }

        private void ClickCell(int x, int y)
        {
            waitForRivalFieldToBeAvailable();
            if(isCellEmpty(x,y))
            {
                battleFieldCell(x, y).Click();
            }            
        }

        private void FinishOffShip(int cellX, int cellY)
        {
            while(!isShipDone)
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

                //Left
                if (isCellEmpty(cellX - 1, cellY))
                {
                    ClickCell(cellX - 1, cellY);
                    if (isLastHit)
                    {
                        FinishOffShip(cellX - 1, cellY);
                    }
                }

                //Down
                if (isCellEmpty(cellX, cellY + 1))
                {
                    ClickCell(cellX, cellY + 1);
                    if (isLastHit)
                    {
                        FinishOffShip(cellX, cellY + 1);
                    }
                }

                //Up
                if (isCellEmpty(cellX, cellY - 1))
                {
                    ClickCell(cellX, cellY - 1);
                    if (isLastHit)
                    {
                        FinishOffShip(cellX, cellY - 1);
                    }
                }
            }
        }
    }
}
