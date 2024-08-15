/*******************************************************************************************************************************
* Made by: Illia                                                                                                               *
* Made for: Ms. Cullum                                                                                                         *
* Course: ICS4U                                                                                                                *
* Date: April 2024                                                                                                             *
* ISU Assignment: this is an interactive game created to verify the profitability of my MDM4U Casino Summative.                * 
* It serves as a digital prototype with the specific objective of calculating the casino's profit per dollar spent by players, *
* which must fall between 10 and 25 cents (USD 0.1 - 0.25). Both the digital and physical versions of this game are original   *
* concepts of mine and cannot be found on the internet. The programming approach taken is unique, with no available tutorials, *
* providing me with an authentic learning experience. The basic gameplay instructions/principles are as follows:               *                                                          *
* 1. First, they flip a coin.                                                                                                  *
* 2. If heads is shown, they start in the king’s place (black center spot).                                                    *
* 3. If the tail is shown, they start at the position of the queen (white center spot).                                        *
* 4. They keep rolling the 6-sided dice until they make it to the end of the game.                                             *
* 5. If an odd number is rolled, they move diagonally left by 1 square.                                                        *
* 6. If an even number is rolled, they move diagonally right by 1 square.                                                      *
* 7. If they happen to cross the border or encounter the hole, the game ends, and they get nothing.                            *
* 8. Their goal is to avoid the obstacles and get a decent reward located on that side of the board.                           *
*******************************************************************************************************************************/

// This is a logic manager, which executes a whole bunch of commands and prompts from the user and is called by other scripts

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogicManagerScript : MonoBehaviour
{
    public Text warningText;
    public Text playerWonNum;
    public Text playerLostNum;
    public Text holdPercentageNum;
    public Text balanceText;

    public int wonNum;
    public int lostNum;
    public int balanceNum;
    public decimal profitCasino;

    public void coinFlippedWarning()
    {
        warningText.color = Color.green;
        warningText.fontSize = 65;
        warningText.text = "Coin flipped!";
    }

    public void coinAlreadyFlippedWarning()
    {
        warningText.color = Color.red;
        warningText.fontSize = 65;
        warningText.text = "Coin already flipped!";
    }

    public void bankruptWarning()
    {
        warningText.color = Color.red;
        warningText.fontSize = 65;
        warningText.text = "You went bankrupt!";
    }

    public void youAreTheKingWarning()
    {
        warningText.fontSize = 70;
        warningText.color = Color.green;
        warningText.text = "You are the king!";
    }

    public void youAreTheQueenWarning()
    {
        warningText.fontSize = 70;
        warningText.color = Color.green;
        warningText.text = "You are the queen!";
    }

    public void letsRollDiceWarning()
    {
        warningText.color = Color.green;
        warningText.fontSize = 58;
        warningText.text = "Now, let's roll the dice!";
    }

    public void letsFlipCoin()
    {
        // Being polite to the user
        warningText.color = Color.green;
        warningText.fontSize = 60;
        warningText.text = "Hi! Let's flip the coin!:)";
    }

    public void eraseText()
    {
        warningText.text = "";
    }

    public void diceRolledWarning() 
    {
        warningText.color = Color.green;
        warningText.fontSize = 65;
        warningText.text = "Dice rolled!";
    }

    public void flipCoinFirstWarning()
    {
        warningText.color = Color.red;
        warningText.fontSize = 65;
        warningText.text = "Flip the coin first!";
    }

    public void moveToSquareWarning()
    {
        warningText.color = Color.magenta;
        warningText.fontSize = 57;
        warningText.text = "Move to new square!";
    }

    public void letsPlayAgain()
    {
        // Being polite to the user
        warningText.color = Color.green;
        warningText.fontSize = 55;
        warningText.text = "Let's flip the coin again:)";
    }

    public void negativeGameOverWarning()
    {
        warningText.color = Color.red;
        warningText.fontSize = 70;
        warningText.text = "Game Over!";
    }

    public void positiveGameOverWarning()
    {
        warningText.color = Color.green;
        warningText.fontSize = 70;
        warningText.text = "Game Over!";
    }

    public void inTroubleWarning()
    {
        warningText.color = Color.red;
        warningText.fontSize = 70;
        warningText.text = "You are in trouble!";
    }

    public void moveFigureFirstWarning()
    {
        warningText.color = Color.red;
        warningText.fontSize = 65;
        warningText.text = "Move the figure first!";
    }

    public void addFourPlayerWon()
    {
        wonNum += 4;
        playerWonNum.text = ""; // Reset playerWonNum.text
        playerWonNum.text += wonNum.ToString();
    }

    public void addFivePlayerWon()
    {
        wonNum += 5;
        playerWonNum.text = ""; // Reset playerWonNum.text
        playerWonNum.text += wonNum.ToString();
    }

    public void addElevenPlayerWon()
    {
        wonNum += 11;
        playerWonNum.text = ""; // Reset playerWonNum.text
        playerWonNum.text += wonNum.ToString();
    }

    public void congratulateUser()
    {
        warningText.color = Color.green;
        warningText.fontSize = 55;
        warningText.text = "There is your reward!";
    }

    public void feeCasino()
    {
        lostNum += 2;
        playerLostNum.text = ""; // Reset playerLostNum.text
        playerLostNum.text += lostNum.ToString();
    }

    public void casinoProfit()
    {
        // The numerator is casino's profit, the whole division is how much casino makes per each dollar player spent
        profitCasino = 0; // Reset the casino's profit
        profitCasino += (decimal)(lostNum - wonNum) / lostNum;
        holdPercentageNum.text = ""; // Reset holdPercentageNum.text
        holdPercentageNum.text += profitCasino.ToString("F2");

        StaticData.valueToKeep = holdPercentageNum.text;
    }

    public void setBalance(int j)
    {
        balanceNum = j;
        balanceText.text = ""; // Reset text
        balanceText.text = balanceNum.ToString();
    }

    public void addFourBalance()
    {
        balanceNum += 4;
        balanceText.text = ""; // Reset text
        balanceText.text = balanceNum.ToString();
    }

    public void addFiveBalance()
    {
        balanceNum += 5;
        balanceText.text = ""; // Reset text
        balanceText.text = balanceNum.ToString();
    }

    public void addElevenBalance()
    {
        balanceNum += 11;
        balanceText.text = ""; // Reset text
        balanceText.text = balanceNum.ToString();
    }

    public void balanceFee()
    {
        balanceNum -= 2;
        balanceText.text = balanceNum.ToString();
    }
}

