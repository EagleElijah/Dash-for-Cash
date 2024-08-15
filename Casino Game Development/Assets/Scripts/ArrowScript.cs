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

// Script solely for destroying the gameObject

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    public KingScript kingScript;
    public QueenScript queenScript;

    // Start is called before the first frame update
    void Start()
    {
        queenScript = GameObject.FindGameObjectWithTag("Queen").GetComponent<QueenScript>();
        kingScript = GameObject.FindGameObjectWithTag("King").GetComponent<KingScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (queenScript.queenMoved == true || kingScript.kingMoved == true)
        {
            Destroy(gameObject);
        }
    }
}
