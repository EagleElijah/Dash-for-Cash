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

// This script is designed for the dice
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiceScript : MonoBehaviour
{
    // Reference to the CoinScript and other
    public CoinScript coinScript;
    public LogicManagerScript managerScript;
    public KingScript kingScript;
    public QueenScript queenScript;

    // Needed for balancing the game
    public int countMoves = 0;
    public int countSpawn = 0;

    // Needed for implementing only once before the game starts (renderer of the dice)
    private SpriteRenderer spriteRenderer;

    // Needed to keep in touch with the user
    public Text warningText;
    public bool greenSquareSpawned = false;

    // Sprites for heads and tails
    public Sprite numOneDice;
    public Sprite numTwoDice;
    public Sprite numThreeDice;
    public Sprite numFourDice;
    public Sprite numFiveDice;
    public Sprite numSixDice;

    // Needed for interacting with the green square 
    public GameObject GreenSquare;
    public Transform KingFigure;
    public Transform QueenFigure;
    public GameObject arrow;

    public Vector3 greenSquarePosition;

    // Start is called before the first frame update
    void Start()
    {
        // Get the SpriteRenderer component attached to the GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();

        managerScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicManagerScript>();

        // Set the initial sprite (number one) 
        spriteRenderer.sprite = numOneDice;
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Handling the game when the dice's sprite is pressed on the screen
    void OnMouseDown()
    {
        // Making sure the dice can be rolled only after the coin is flipped 
        if (coinScript.hasCoinClicked == true)
        {
            countMoves += 1;

            // Error handling so that the person can roll the dice again only after the figure is moved (after the initial start)
            if (countMoves > 1)
            {
                managerScript.moveFigureFirstWarning();

                StartCoroutine(EraseWarningTextAfterDelay());
            }

            else
            {
                // The green square has not been spawned yet
                greenSquareSpawned = false;

                Debug.Log("Dice rolled!");

                // Communication process
                managerScript.diceRolledWarning();

                StartCoroutine(Wait());

                // So that when the king is moved and the dice is not rolled, move to next square is not displayed.
                if (countMoves != 0)
                {
                    // Communication
                    StartCoroutine(NextPrompt());
                }

                // Rolling the dice (animation)
                StartCoroutine(RollDice());
            }
        }

        else
        {
            Debug.Log("Flip the coin first!");

            // Communication
            managerScript.flipCoinFirstWarning();

            // Erasing the text after delay 
            StartCoroutine(EraseWarningTextAfterDelay());
        }
    }

    // Roll the dice by changing sprites back and forth (animation once clicked)
    IEnumerator RollDice()
    {
        // Destroy any existing green square (Destroy(GreenSquare) is not permitted for some reason I'm not interested in) 
        Destroy(GameObject.FindGameObjectWithTag("GreenSquare"));

        // Random number of rolls between 5 and 7 (inclusive), just like in real life
        int randomRolls = UnityEngine.Random.Range(5, 8);

        // Can adjust the number of rolls as needed, now it is set to 7 rolls
        for (int i = 0; i < randomRolls + 1; i++)
        {
            // New sprite creation
            spriteRenderer.sprite = numTwoDice;

            // Instantiating the square based on the object's position
            GreenSquareRightAnimation();

            // Waiting for a certain period of time
            yield return new WaitForSeconds(0.07f);

            // Destroy any existing green square (Destroy(GreenSquare) is not permitted for some reason I'm not interested in) 
            Destroy(GameObject.FindGameObjectWithTag("GreenSquare"));

            // New sprite creation
            spriteRenderer.sprite = numThreeDice;

            // Instantiating the square based on the object's position
            GreenSquareLeftAnimation();

            // Waiting for a certain period of time
            yield return new WaitForSeconds(0.07f);

            // Destroy any existing green square (Destroy(GreenSquare) is not permitted for some reason I'm not interested in) 
            Destroy(GameObject.FindGameObjectWithTag("GreenSquare"));

            // New sprite creation
            spriteRenderer.sprite = numFourDice;

            // Instantiating the square based on the object's position
            GreenSquareRightAnimation();

            // Waiting for a certain period of time
            yield return new WaitForSeconds(0.07f);

            // Destroy any existing green square (Destroy(GreenSquare) is not permitted for some reason I'm not interested in) 
            Destroy(GameObject.FindGameObjectWithTag("GreenSquare"));

            // New sprite creation
            spriteRenderer.sprite = numFiveDice;

            // Instantiating the square based on the object's position
            GreenSquareLeftAnimation();

            // Waiting for a certain period of time
            yield return new WaitForSeconds(0.07f);

            // Destroy any existing green square (Destroy(GreenSquare) is not permitted for some reason I'm not interested in) 
            Destroy(GameObject.FindGameObjectWithTag("GreenSquare"));

            // New sprite creation
            spriteRenderer.sprite = numSixDice;

            // Instantiating the square based on the object's position
            GreenSquareRightAnimation();

            // Waiting for a certain period of time
            yield return new WaitForSeconds(0.07f);

            // Destroy any existing green square (Destroy(GreenSquare) is not permitted for some reason I'm not interested in) 
            Destroy(GameObject.FindGameObjectWithTag("GreenSquare"));

            // New sprite creation
            spriteRenderer.sprite = numOneDice;

            // Instantiating the square based on the object's position
            GreenSquareLeftAnimation();

            // Waiting for a certain period of time
            yield return new WaitForSeconds(0.07f);

            // Destroy any existing green square (Destroy(GreenSquare) is not permitted for some reason I'm not interested in) 
            Destroy(GameObject.FindGameObjectWithTag("GreenSquare"));
        }

        // Displaying the result of the rolling of the dice
        DisplayResultDiceRoll();
    }

    // Funtion to erase the text
    IEnumerator EraseWarningTextAfterDelay()
    {
        yield return new WaitForSeconds(4f);

        managerScript.eraseText();
    }

    // Waiting fumction
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(5f);
    }

    // Communicating 
    IEnumerator NextPrompt()
    {
        yield return new WaitForSeconds(7f);

        managerScript.moveToSquareWarning();
    }

    // Function for displaying the result of the dice roll
    void DisplayResultDiceRoll()
    {
        countSpawn += 1;

        // Random number of rolls between 1 and 6 (inclusive), just like in real life
        int randomDiceSide = UnityEngine.Random.Range(1, 7);

        // Destroy any existing green square (Destroy(GreenSquare) is not permitted for some reason I'm not interested in) 
        Destroy(GameObject.FindGameObjectWithTag("GreenSquare"));

        // Setting a random picture to be displayed every time the dice is rolled 
        switch (randomDiceSide)
        {
            case 1:
                spriteRenderer.sprite = numOneDice;

                // Instantiating the square and arrow based on the object's position
                GreenSquareLeft();
                arrowLeft();

                greenSquareSpawned = true;

                break;

            case 2:
                spriteRenderer.sprite = numTwoDice;

                // Instantiating the squareand arrow based on the object's position
                GreenSquareRight();
                arrowRight();

                greenSquareSpawned = true;

                break;

            case 3:
                spriteRenderer.sprite = numThreeDice;

                // Instantiating the square and arrow based on the object's position
                GreenSquareLeft();
                arrowLeft();

                greenSquareSpawned = true;

                break;

            case 4:
                spriteRenderer.sprite = numFourDice;

                // Instantiating the square and arrow based on the object's position
                GreenSquareRight();
                arrowRight();

                greenSquareSpawned = true;

                break;

            case 5:
                spriteRenderer.sprite = numFiveDice;

                // Instantiating the square and arrow based on the object's position
                GreenSquareLeft();
                arrowLeft();

                greenSquareSpawned = true;

                break;

            case 6:
                spriteRenderer.sprite = numSixDice;

                // Instantiating the square and arrow based on the object's position
                GreenSquareRight();
                arrowRight();

                greenSquareSpawned = true;

                break;

            default:
                Debug.Log("Invalid dice side!");
                break;
        }

        // The movement of the king can be detected only after the green square is spawned
        kingScript.detectionCan = true;

        // The movement of the king can be detected only after the green square is spawned
        queenScript.detectionCan = true;
    }

    // Instantiating the green square to the right based on the object's position
    void GreenSquareRightAnimation()
    {
        if (coinScript.isKing == true)
        {
            // Instantiate greensquare relative to king's position
            Instantiate(GreenSquare, KingFigure.position + new Vector3(1.2f, 1.03f, 0f), Quaternion.identity);
        }

        else if (coinScript.isKing == false)
        {
            // Instantiate greensquare relative to queen's position
            Instantiate(GreenSquare, QueenFigure.position + new Vector3(1.2f, 1.03f, 0f), Quaternion.identity);
        }
    }

    // Instantiating the green square to the right based on the object's position
    void GreenSquareRight()
    {
        if (countSpawn < 2)
        {
            if (coinScript.isKing == true)
            {
                // Instantiate greensquare relative to king's position
                Instantiate(GreenSquare, KingFigure.position + new Vector3(1.2f, 1.03f, 0f), Quaternion.identity);
                greenSquarePosition = KingFigure.position + new Vector3(1.2f, 1.03f, 0f);
            }

            else if (coinScript.isKing == false)
            {
                // Instantiate greensquare relative to queen's position
                Instantiate(GreenSquare, QueenFigure.position + new Vector3(1.2f, 1.03f, 0f), Quaternion.identity);
                greenSquarePosition = QueenFigure.position + new Vector3(1.2f, 1.03f, 0f);
            }
        }

        else
        {
            // Instantiate greensquare and arrow relative to green square's position
            Instantiate(GreenSquare, greenSquarePosition + new Vector3(1.2f, 1.03f, 0f), Quaternion.identity);

            greenSquarePosition += new Vector3(1.2f, 1.03f, 0f);
        }
    }

    // Instantiating the green square to the left based on the object's position
    void GreenSquareLeftAnimation()
    {
        if (coinScript.isKing == true)
        {
            // Instantiate greensquare relative to king's position
            Instantiate(GreenSquare, KingFigure.position + new Vector3(-1.2f, 1.03f, 0f), Quaternion.identity);
        }

        else if (coinScript.isKing == false)
        {
            // Instantiate greensquare relative to queen's position
            Instantiate(GreenSquare, QueenFigure.position + new Vector3(-1.2f, 1.03f, 0f), Quaternion.identity);
        }  
    }

    // Instantiating the green square to the left based on the object's position
    void GreenSquareLeft()
    {
        if (countSpawn < 2)
        {
            if (coinScript.isKing == true)
            {
                // Instantiate greensquare relative to king's position
                Instantiate(GreenSquare, KingFigure.position + new Vector3(-1.2f, 1.03f, 0f), Quaternion.identity);
                greenSquarePosition = KingFigure.position + new Vector3(-1.2f, 1.03f, 0f);
            }

            else if (coinScript.isKing == false)
            {
                // Instantiate greensquare relative to queen's position
                Instantiate(GreenSquare, QueenFigure.position + new Vector3(-1.2f, 1.03f, 0f), Quaternion.identity);
                greenSquarePosition = QueenFigure.position + new Vector3(-1.2f, 1.03f, 0f);
            }
        }

        else
        {
            // Instantiate greensquare and arrow relative to green square's position
            Instantiate(GreenSquare, greenSquarePosition + new Vector3(-1.195f, 1.03f, 0f), Quaternion.identity);

            greenSquarePosition += new Vector3(-1.195f, 1.03f, 0f);
        }
    }

    // Instantiating the arrow to the right of the object
    void arrowRight()
    {
        if (coinScript.isKing == true)
        {
            // Instantiate greensquare relative to king's position
            Instantiate(arrow, KingFigure.position + new Vector3(0.6f, 0.5f, 0f), Quaternion.Euler(0f, 0f, 45f));
        }

        else if (coinScript.isKing == false)
        {
            // Instantiate greensquare relative to queen's position
            Instantiate(arrow, QueenFigure.position + new Vector3(0.6f, 0.5f, 0f), Quaternion.Euler(0f, 0f, 45f));
        }
    }

    // Instantiating the arrow to the left of the object
    void arrowLeft()
    {
        if (coinScript.isKing == true)
        {
            // Instantiate greensquare relative to king's position
            Instantiate(arrow, KingFigure.position + new Vector3(-0.6f, 0.5f, 0f), Quaternion.Euler(0f, 0f, -225f));
        }

        else if (coinScript.isKing == false)
        {
            // Instantiate greensquare relative to queen's position
            Instantiate(arrow, QueenFigure.position + new Vector3(-0.6f, 0.5f, 0f), Quaternion.Euler(0f, 0f, -225f));
        }
    }
}

    