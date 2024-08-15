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

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
public class GreenSquareScript : MonoBehaviour
{
    // Connection to figures and keeping up with the user
    public KingScript kingScript;
    public QueenScript queenScript;
    public DiceScript diceScript;
    public LogicManagerScript managerScript;
    public CoinScript coinScript;

    // Needed to change the color of the sprite of the GameObject this script is attached to
    // Notice here that, unlike scripts, it can be dragged because it belongs to the same GameObject
    public SpriteRenderer greenSquareRenderer;
    public Sprite kingy;
    public Sprite queeny;

    private AudioSource audioSource;
    public AudioSource coinSource;

    public AudioClip taDa;
    public AudioClip quapQuap;
    public AudioClip vacuUm;

    private int countUpdate = 0;
    private int count1Update = 0;

    public bool overlapObstacleDetected = false;
    public bool finishScreen = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        coinSource = GameObject.FindGameObjectWithTag("Coin").GetComponent<AudioSource>();

        // Simply dragging the objects to the prefab does not work because the prefab is not spawned but saved to memory, so this is the way I suffered to come up with
        kingScript = GameObject.FindGameObjectWithTag("King").GetComponent<KingScript>();
        queenScript = GameObject.FindGameObjectWithTag("Queen").GetComponent<QueenScript>();
        diceScript = GameObject.FindGameObjectWithTag("Dice").GetComponent<DiceScript>();
        managerScript = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicManagerScript>();
        coinScript = GameObject.FindGameObjectWithTag("Coin").GetComponent<CoinScript>();

        audioSource.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // First, we are making sure that the green square is not collided with the chessboard or holes
        // Check for overlap with specific tags
        // So that I freeze the game when kingMoved or queenMoved = true and do not proceed further
        CheckOverlap("ChessBoard");
        CheckOverlap("Hole");
        CheckOverlap("4");
        CheckOverlap("5");
        CheckOverlap("11");

        if (audioSource.isPlaying == false && finishScreen == true)
        {
            if (managerScript.balanceNum != 0)
            {
                Destroy(gameObject);
            }
        }

        // If greensquare is located on the legitimate location, and the figure is moved, then the green square is destroyed and game continues
        if ((kingScript.kingMoved == true || queenScript.queenMoved == true) && diceScript.greenSquareSpawned == true && overlapObstacleDetected == false)
        {
            // Destroying the greensquare and arrow
            Destroy(gameObject);

            // Reseting values
            countUpdate = 0;

            StartCoroutine(EraseWarningTextAfterDelay());

            // Helping the user out
            managerScript.letsRollDiceWarning();

            StartCoroutine(EraseWarningTextAfterDelay());
        }
    }

    // This function checks if GreenSquare is overlaping with either 2 box colliders (borders/bounds) of the chessboard or those of holes (based on what "tag" is)
    // Based on the results, respective actions are executed
    private void CheckOverlap(string tag)
    {
        // Only after the final square is displayed
        if (diceScript.greenSquareSpawned == true)
        {
            // Get all colliders with the specified tag
            GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(tag);

            // Loop through each object
            foreach (GameObject obj in taggedObjects)
            {
                // Get all BoxCollider2D components
                BoxCollider2D[] colliders = obj.GetComponents<BoxCollider2D>();

                // Loop through each collider
                foreach (BoxCollider2D collider in colliders)
                {
                    // Check if there's a collider component
                    if (collider != null)
                    {
                        // Check if any overlap exists through IsTouching method
                        if (GetComponent<BoxCollider2D>().IsTouching(collider))
                        {
                            // If the boxcollider tag happens to be the number and the figure is moved
                            if (tag == "4")
                            {
                                // If figure is moved, game over happens (booleans kingMoved and queenMoved are handled seperately)
                                if (kingScript.kingMoved == true || queenScript.queenMoved == true)
                                {
                                    // So that the stats are updated only once even though we are checking for collider multiple times
                                    countUpdate += 1;

                                    coinSource.enabled = false;

                                    audioSource.clip = taDa;
                                    audioSource.enabled = true;

                                    if (countUpdate < 2)
                                    {
                                        // Loading the black screen
                                        managerScript.positiveGameOverWarning();

                                        // Fee casino + player won money + calculation is displayed
                                        managerScript.addFourPlayerWon();

                                        // Adding four to the balance
                                        managerScript.addFourBalance();

                                        // Keeping track of the casino's profit per each dollar player spent (updating only matters if hole or reward or out of bounds)
                                        managerScript.casinoProfit();
                                    }

                                    finishScreen = true;
                                }

                                // If figure is not moved, the warning pops up (booleans kingMoved and queenMoved are handled seperately)
                                else if ((kingScript.kingMoved == false || queenScript.queenMoved == false) && finishScreen == false)
                                {
                                    // So that the stats are updated only once even though we are checking for collider multiple times
                                    count1Update += 1;

                                    if (count1Update < 2)
                                    {
                                        managerScript.congratulateUser();
                                    }
                                }
                            }

                            else if (tag == "5")
                            {
                                // If figure is moved, game over happens (booleans kingMoved and queenMoved are handled seperately)
                                if (kingScript.kingMoved == true || queenScript.queenMoved == true)
                                {
                                    // So that the stats are updated only once even though we are checking for collider multiple times
                                    countUpdate += 1;

                                    coinSource.enabled = false;

                                    audioSource.clip = taDa;
                                    audioSource.enabled = true;

                                    if (countUpdate < 2)
                                    {
                                        // Loading the black screen
                                        managerScript.positiveGameOverWarning();

                                        // Fee casino + player won money + calculation is displayed
                                        managerScript.addFivePlayerWon();

                                        // Adding five to the balance
                                        managerScript.addFiveBalance();

                                        // Keeping track of the casino's profit per each dollar player spent (updating only matters if hole or reward or out of bounds)
                                        managerScript.casinoProfit();
                                    }

                                    finishScreen = true;
                                }

                                // If figure is not moved, the warning pops up (booleans kingMoved and queenMoved are handled seperately)
                                else if ((kingScript.kingMoved == false || queenScript.queenMoved == false) && finishScreen == false)
                                {
                                    // So that the stats are updated only once even though we are checking for collider multiple times
                                    count1Update += 1;

                                    if (count1Update < 2)
                                    {
                                        managerScript.congratulateUser();
                                    }
                                }
                            }

                            else if (tag == "11")
                            {
                                // If figure is moved, game over happens (booleans kingMoved and queenMoved are handled seperately)
                                if (kingScript.kingMoved == true || queenScript.queenMoved == true)
                                {
                                    // So that the stats are updated only once even though we are checking for collider multiple times
                                    countUpdate += 1;

                                    coinSource.enabled = false;

                                    audioSource.clip = taDa;
                                    audioSource.enabled = true;

                                    if (countUpdate < 2)
                                    {
                                        // Loading the black screen
                                        managerScript.positiveGameOverWarning();

                                        // Fee casino + player won money + calculation is displayed
                                        managerScript.addElevenPlayerWon();

                                        // Adding eleven to the balance
                                        managerScript.addElevenBalance();

                                        // Keeping track of the casino's profit per each dollar player spent (updating only matters if hole or reward or out of bounds)
                                        managerScript.casinoProfit();
                                    }

                                    finishScreen = true;
                                }

                                // If figure is not moved, the warning pops up (booleans kingMoved and queenMoved are handled seperately)
                                else if ((kingScript.kingMoved == false || queenScript.queenMoved == false) && finishScreen == false)
                                {
                                    // So that the stats are updated only once even though we are checking for collider multiple times
                                    count1Update += 1;

                                    if (count1Update < 2)
                                    {
                                        managerScript.congratulateUser();
                                    }
                                }
                            }

                            // Hole or out of bounds
                            else if (tag == "Hole" || tag == "ChessBoard")
                            {
                                // Painting the square to black (red + green = black)
                                greenSquareRenderer.color = Color.red;

                                // So that the stats are updated only once even though we are checking for collider multiple times
                                countUpdate += 1;

                                // Overlap detected
                                // If figure is moved, game over happens (booleans kingMoved and queenMoved are handled seperately)
                                if (kingScript.kingMoved == true || queenScript.queenMoved == true)
                                {
                                    coinSource.enabled = false;

                                    if (tag == "Hole")
                                    {
                                        audioSource.clip = vacuUm;
                                        audioSource.enabled = true;
                                    }

                                    else if (tag == "ChessBoard")
                                    {
                                        audioSource.clip = quapQuap;
                                        audioSource.enabled = true;
                                    }

                                    // Loading the black screen
                                    managerScript.negativeGameOverWarning();

                                    finishScreen = true;

                                    if (managerScript.balanceNum < 0)
                                    { 
                                        managerScript.eraseText();
                                        managerScript.bankruptWarning();
                                        managerScript.setBalance(0);
                                        managerScript.lostNum -= 4;
                                        managerScript.feeCasino();
                                        managerScript.casinoProfit();

                                        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManagerScript>().QuitToOver();
                                    }
                                }

                                // If figure is not moved, the warning pops up (booleans kingMoved and queenMoved are handled seperately)
                                else if ((kingScript.kingMoved == false || queenScript.queenMoved == false) && finishScreen == false)
                                {
                                    if (countUpdate < 2)
                                    {
                                        managerScript.inTroubleWarning();
                                    }
                                }
                            }

                            overlapObstacleDetected = true;
                        }
                    }
                }
            }
        }
    }
    IEnumerator waitGameOver()
    {
        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("Game Over Scene");
    }

    // Funtion to erase the text
    IEnumerator EraseWarningTextAfterDelay()
    {
        yield return new WaitForSeconds(10f);

        managerScript.eraseText();
    }
}








