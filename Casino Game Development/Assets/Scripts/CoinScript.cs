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

// This script is designed for the coin 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;

public class CoinScript : MonoBehaviour
{
    // Needed for implementing only once before the game starts
    public bool hasCoinClicked = false;
    public bool isKing = false;
    public int countFlip = 0;

    // Sprite renderer of the coin
    private SpriteRenderer spriteRenderer;

    // For updating text
    public LogicManagerScript managerScript;

    // Sprites for heads and tails
    public Sprite headsSprite;
    public Sprite tailsSprite;

    public int count = 0;

    // Needed to keep in touch with the user
    public Text warningText;

    // Needed to highlight the king or the queen based on the side of the coin shown up
    public GameObject queenFigure;
    public GameObject kingFigure;

    // Define the AudioSource variable
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // Paying the fee and also setting up the budget of the player
        managerScript.setBalance(50);
        managerScript.balanceFee();

        // Player won $0 at the beginning and charged $2 fee
        managerScript.feeCasino();

        // Keeping track of the casino's profit per each dollar player spent (once at the beginning of the game)
        managerScript.casinoProfit();

        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        // Disable the AudioSource component initially
        audioSource.enabled = false;

        // Get the SpriteRenderer component attached to the GameObject
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the initial sprite (heads) 
        spriteRenderer.sprite = headsSprite;

        // Being polite to the user
        managerScript.letsFlipCoin();
    }

    // Update is called once per frame
    void Update()
    { 
        // Playing the song after the coin is flipped
        if (hasCoinClicked == true)
        {
            if (count < 2)
            {
                // Enable the AudioSource component before playing the sound effect
                audioSource.enabled = true;

                count += 1;
            }
        }
    }

    // Handling the game when the coin's sprite is pressed on the screen
    void OnMouseDown()
    {
        countFlip += 1;

        // Making sure the coin is interacted only once per game
        if (!hasCoinClicked && countFlip < 2) 
        {
            Debug.Log("Coin flipped!");

            // Communication
            managerScript.coinFlippedWarning();

            // Erasing the text after delay
            StartCoroutine(EraseWarningTextAfterDelay());

            // Flipping the coin (animation)
            StartCoroutine(FlipCoin());
        }

        else
        {
            // Communication process 
            Debug.Log("Coin already flipped!");

            // Communication
            managerScript.coinAlreadyFlippedWarning();

            // Erasing the text after delay 
            StartCoroutine(EraseWarningTextAfterDelay());
        }
    }

    // Flip the coin by changing sprites back and forth (animation once clicked)
    IEnumerator FlipCoin()
    {
        // Random number of rolls between 10 and 20 (inclusive), just like in real life
        int randomFlips = UnityEngine.Random.Range(10, 21);

        // Can adjust the number of flips as needed, now it is set between 10 and 20 (inclusive)
        for (int i = 0; i < randomFlips + 1; i++) 
        {
            // New sprite creation
            spriteRenderer.sprite = tailsSprite;

            queenFigure.GetComponent<SpriteRenderer>().color = Color.blue;

            // Waiting for a certain time
            yield return new WaitForSeconds(0.05f);

            queenFigure.GetComponent<SpriteRenderer>().color = Color.white;

            // New sprite creation
            spriteRenderer.sprite = headsSprite;

            kingFigure.GetComponent<SpriteRenderer>().color = Color.blue;

            // Waiting for a certain time
            yield return new WaitForSeconds(0.05f);

            kingFigure.GetComponent<SpriteRenderer>().color = Color.white;
        }

        // Displaying the result of the coin flip (highlighting respective figure)
        DisplayResultCoinFlip();
    }

    // Displaying the result of the coin flip
    void DisplayResultCoinFlip()
    {
        // Random number of flips between 1 and 2 (inclusive), just like in real life
        int randomCoinSide = UnityEngine.Random.Range(1, 3);

        // Setting a random picture to be displayed every time the dice is rolled (ultimate point)
        switch (randomCoinSide)
        {
            case 1:
                spriteRenderer.sprite = headsSprite;

                // Highlighting the kingFigure GameObject
                kingFigure.GetComponent<SpriteRenderer>().color = Color.blue;

                // Informing the player
                managerScript.youAreTheKingWarning();
                isKing = true;
                StartCoroutine(EraseWarningTextAfterDelay());

                // Setting the flag to false to indicate that we have already clicked the coin
                hasCoinClicked = true;

                // So that the queen is not frozen
                kingFigure.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

                // Freezing the queen
                queenFigure.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

                break;

            case 2:
                spriteRenderer.sprite = tailsSprite;

                // Highlighting the queenFigure GameObject
                queenFigure.GetComponent<SpriteRenderer>().color = Color.blue;

                // Informing the player
                managerScript.youAreTheQueenWarning();
                isKing = false;
                StartCoroutine(EraseWarningTextAfterDelay());

                // Setting the flag to false to indicate that we have already clicked the coin
                hasCoinClicked = true;

                // So that the king is not frozen
                queenFigure.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;

                // So that the king is frozen
                kingFigure.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

                break;

            default:
                Debug.Log("Invalid dice side!");

                break;
        }

        // Delay before prompting the user for the next deeds
        StartCoroutine(PromptNextActionAfterDelay());
    }

    // Funtion to erase the text
    IEnumerator EraseWarningTextAfterDelay()
    {
        yield return new WaitForSeconds(4f);

        managerScript.eraseText();
    }

    // Function to prompt the user for the next action after a delay
    IEnumerator PromptNextActionAfterDelay()
    {
        yield return new WaitForSeconds(3f);

        // Prompting the user for the next deeds
        managerScript.letsRollDiceWarning();

        yield return new WaitForSeconds(20f);
    }
}
