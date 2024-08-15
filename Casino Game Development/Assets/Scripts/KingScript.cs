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

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class KingScript : MonoBehaviour
{
    // Needed to update the game only if the king is moved and also moving the king only after and if the king figure is chosen and green square displayed
    public bool kingMoved;
    private bool kingMovedOnObstacle;

    // Flags to check if the queen is being dragged and if it is on square finally
    private bool isDragging = false;
    public bool detectionCan = false;

    public CoinScript coinScript;
    public DiceScript diceScript;
    public LogicManagerScript logicManager;
    public GreenSquareScript squareScript;

    // Green square prefab 
    public GameObject GreenSquare;

    // Initial position of the queen
    private Vector3 initialPosition;
    private Vector3 startKingPosition;

    private int count;

    // Rigidbody2D component of the queen
    private Rigidbody2D rb2D;

    // Start is called before the first frame update
    void Start()
    {
        squareScript = GreenSquare.GetComponent<GreenSquareScript>();

        rb2D = GetComponent<Rigidbody2D>();

        // Disable physics simulation initially
        rb2D.isKinematic = true;

        startKingPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        kingMoved = false;
        kingMovedOnObstacle = false;

        // Detecting where is the king and allowin the king to be moved only after the green square is displayed and king is dropped
        if (detectionCan == true && coinScript.isKing == true)
        { 
            kingMouseDetection();
        }

        // If bankrupt, figure destroyed
        if (logicManager.balanceNum != 0 && squareScript.finishScreen == true)
        {
            Destroy(gameObject);
        }

        // Reseting the initial position of the green square
        if (coinScript.isKing == true)
        {
            count += 1;
            if (count == 1)
            {
                diceScript.greenSquarePosition = startKingPosition;
            }
        }

        // If the dice has not yet been rolled, the figure remains frozen
        if (diceScript.greenSquareSpawned != true)
        {
            // Freezing the queen
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }

        else
        {
            // Unfreezing the king
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        }
    }

    // Function that handles mouse input for the square and king
    private void kingMouseDetection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;

            if (GetComponent<Collider2D>().OverlapPoint(mousePosition))
            {
                // Getting the current position of the king so that the king can be returned to its current position
                initialPosition = transform.position;

                isDragging = true;

                // Enable physics simulation for dragging
                rb2D.isKinematic = false;
            }
        }

        if (isDragging && Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            rb2D.MovePosition(mousePosition);
        }

        if (isDragging && Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            rb2D.velocity = Vector2.zero;
            rb2D.isKinematic = true;

            CheckOverlap("GreenSquare");
            CheckOverlap("Hole");
            CheckOverlap("ChessBoard");
            CheckOverlap("4");
            CheckOverlap("5");
            CheckOverlap("11");

            // If the green square is not encountered, return to initial position
            if (kingMoved == false)
            {
                rb2D.MovePosition(initialPosition);
                logicManager.moveToSquareWarning();
                StartCoroutine(EraseWarningTextAfterDelay());
            }

            // If the object is encountered, the king is returned to its initial position and stats are reset
            if (kingMovedOnObstacle == true)
            {
                rb2D.isKinematic = false;
                rb2D.MovePosition(startKingPosition);
                coinScript.hasCoinClicked = false;
                coinScript.count = 0;
                coinScript.countFlip = 0;
                count = 0;
                diceScript.countMoves = 0;
                diceScript.countSpawn = 0;
                coinScript.isKing = false;
                StartCoroutine(PromptNextActionAfterDelay());

                // Paying the fee
                logicManager.balanceFee();

                // Player won $0 at the beginning and charged $2 fee
                logicManager.feeCasino();

                // Keeping track of the casino's profit per each dollar player spent (once at the beginning of the game)
                logicManager.casinoProfit();
            }
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
                            if (tag == "GreenSquare")
                            { 
                                // Reseting values so that everything works correctly
                                kingMoved = true;
                                diceScript.countMoves = 0;
                                detectionCan = false;
                            }

                            // Additional checking so that the king is destroyed if the obstacle is encountered
                            if ((tag == "Hole" || tag == "ChessBoard") && detectionCan == false)
                            {
                                // Reseting values so that everything works correctly
                                kingMoved = true;
                                diceScript.countMoves = 0;
                                detectionCan = false;
                                kingMovedOnObstacle = true;
                            }

                            if ((tag == "4" || tag == "5" || tag == "11") && detectionCan == false)
                            {
                                kingMovedOnObstacle = true;
                            }
                        }
                    }
                }
            }
        }
    }

    // Funtion to erase the text
    IEnumerator EraseWarningTextAfterDelay()
    {
        yield return new WaitForSeconds(7f);

        logicManager.eraseText();
    }

    // Function to prompt the user for the next action after a delay
    IEnumerator PromptNextActionAfterDelay()
    {
        yield return new WaitForSeconds(8f);

        logicManager.eraseText();

        // Prompting the user for the next deeds
        logicManager.letsPlayAgain();

        yield return new WaitForSeconds(20f);
    }
}



