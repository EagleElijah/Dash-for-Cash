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
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip clip;
    private AudioListener listener;
    private string text;
    public InputField inputField;

    private int countPause = 0;

    // Start is called before the first frame update
    void Start()
    {
        // So that I'm not dragging the prefab and do not have to instantiate in that prefab new scripts that I don't have in my second scene
        audioSource = GetComponent<AudioSource>();
        listener = GetComponent<AudioListener>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Pausing function
    public void Pause()
    {
        StartCoroutine(WaitForClipAndPause());
    }

    // Coroutine to wait for clip to finish and then pause
    private IEnumerator WaitForClipAndPause()
    {
        countPause += 1;

        if (countPause < 2)
        {
            while (audioSource.clip != null)
            {
                yield return new WaitForSeconds(0.1f);
            }

            // Load the "Pause Scene" additively
            SceneManager.LoadScene("Pause Scene", LoadSceneMode.Additive);

            DisableEventSystemAndAudioListenersOfScene("Casino Game Main Scene");

            // Pause background music
            PauseBackgroundMusic();

            countPause = 0;
        }
    }

    // Continuing function
    public void Continue()
    {
        StartCoroutine(WaitForClipAndContinue());
    }

    // Coroutine to wait for clip to finish and then continue
    private IEnumerator WaitForClipAndContinue()
    {
        while (audioSource.clip != null)
        {
            yield return new WaitForSeconds(0.1f);
        }

        // Load the main scene additively
        DisableEventSystemAndAudioListenersOfScene("Pause Scene");
        SceneManager.UnloadSceneAsync("Pause Scene");

        EnableEventSystemAndAudioListenersOfScene("Casino Game Main Scene");

        // Resume background music
        ResumeBackgroundMusic();
    }

    // Restarting function
    public void Restart()
    {
        StartCoroutine(WaitForClipAndRestart());
    }

    // Coroutine to wait for clip to finish and then restart
    private IEnumerator WaitForClipAndRestart()
    {
        while (audioSource.clip != null)
        {
            yield return new WaitForSeconds(0.1f);
        }

        // Loading the main scene
        SceneManager.LoadScene("Casino Game Main Scene");
    }

    // Quitting to menu function
    public void QuitToMenu()
    {
        StartCoroutine(WaitForClipAndMenu());
    }

    // Coroutine to wait for clip to finish and then menu
    private IEnumerator WaitForClipAndMenu()
    {
        while (audioSource.clip != null)
        {
            yield return new WaitForSeconds(0.1f);
        }

        // Loading the main scene
        SceneManager.LoadScene("Menu Scene");
    }

    // Starting the game
    public void StartingGame()
    {
        StartCoroutine(StartGame());
    }

    // Coroutine to wait for clip to finish and then menu
    private IEnumerator StartGame()
    {
        while (audioSource.clip != null)
        {
            yield return new WaitForSeconds(0.1f);
        }

        // Loading the main scene
        SceneManager.LoadScene("Check Scene");
    }

    // Opening the main scene
    // Starting the game
    public void StartingCasino()
    {
        // Loading the main scene
        SceneManager.LoadScene("Casino Game Main Scene");
    }

    public void QuitApp()
    {
        StartCoroutine(QuitGame());
    }

    // Coroutine to wait for clip to finish and then menu
    private IEnumerator QuitGame()
    {
        while (audioSource.clip != null)
        {
            yield return new WaitForSeconds(0.1f);
        }

        // Quiting the application through Unity built-in features
        Application.Quit();
    }

    // Coroutine to wait for clip to finish and then menu
    public void QuityGame()
    {
        // Loading the main scene
        SceneManager.LoadScene("Menu Scene");
    }

    // Function to play a certain audio clip
    public void PlayClip()
    {
        audioSource.clip = clip;
        audioSource.Play();
        StartCoroutine(WaitForClipToEnd());
    }

    IEnumerator WaitForClipToEnd()
    {
        // Wait until the clip finishes playing
        yield return new WaitForSeconds(0.5f);

        // Assign null to the clip after it's finished playing
        audioSource.clip = null;
    }

    public void onSubmit()
    {
        if (inputField.text.ToUpper().Trim() == "I CONSENT" || inputField.text.ToUpper().Trim() == "I  CONSENT" || inputField.text.ToUpper().Trim() == "I   CONSENT")
        {
            StartingCasino();
        }

        else 
        {
            QuityGame();
        }
    }

    // Quitting to menu function
    public void QuitToOver()
    {
        StartCoroutine(gameOVer());
    }

    // Coroutine to wait for clip to finish and then menu
    private IEnumerator gameOVer()
    {
        yield return new WaitForSeconds(2f);

        // Loading the main scene
        SceneManager.LoadScene("Game Over Scene");
    }

    // Disable the EventSystem and AudioListeners of the specified scene
    private void DisableEventSystemAndAudioListenersOfScene(string sceneName)
    {
        // Find the scene by name
        Scene scene = SceneManager.GetSceneByName(sceneName);

        // If the scene exists and is loaded
        if (scene.IsValid() && scene.isLoaded)
        {
            // Find all EventSystem components in the scene
            EventSystem[] eventSystems = scene.GetRootGameObjects()
                .SelectMany(go => go.GetComponentsInChildren<EventSystem>())
                .ToArray();

            // Disable each EventSystem found
            foreach (EventSystem es in eventSystems)
            {
                es.enabled = false;
            }

            // Find all AudioListener components in the scene
            AudioListener[] audioListeners = scene.GetRootGameObjects()
                .SelectMany(go => go.GetComponentsInChildren<AudioListener>())
                .ToArray();

            // Disable each AudioListener found
            foreach (AudioListener listener in audioListeners)
            {
                listener.enabled = false;
            }
        }
    }


    // Enable the EventSystem and AudioListeners of the specified scene
    private void EnableEventSystemAndAudioListenersOfScene(string sceneName)
    {
        // Find the scene by name
        Scene scene = SceneManager.GetSceneByName(sceneName);

        // If the scene exists and is loaded
        if (scene.IsValid() && scene.isLoaded)
        {
            // Find all EventSystem components in the scene
            EventSystem[] eventSystems = scene.GetRootGameObjects()
                .SelectMany(go => go.GetComponentsInChildren<EventSystem>())
                .ToArray();

            // Enable each EventSystem found
            foreach (EventSystem es in eventSystems)
            {
                es.enabled = true;
            }

            // Find all AudioListener components in the scene
            AudioListener[] audioListeners = scene.GetRootGameObjects()
                .SelectMany(go => go.GetComponentsInChildren<AudioListener>())
                .ToArray();

            // Enable each AudioListener found
            foreach (AudioListener listener in audioListeners)
            {
                listener.enabled = true;
            }
        }
    }

    // Function to pause background music
    private void PauseBackgroundMusic()
    {
        // Find all AudioSources playing music in the main scene
        AudioSource[] audioSources = SceneManager.GetActiveScene().GetRootGameObjects()
            .SelectMany(go => go.GetComponentsInChildren<AudioSource>())
            .Where(source => source.clip != null)
            .ToArray();

        // Pause each AudioSource found
        foreach (AudioSource source in audioSources)
        {
            source.Pause();
        }
    }

    // Function to resume background music
    private void ResumeBackgroundMusic()
    {
        // Find all AudioSources playing music in the main scene
        AudioSource[] audioSources = SceneManager.GetActiveScene().GetRootGameObjects()
            .SelectMany(go => go.GetComponentsInChildren<AudioSource>())
            .Where(source => source.clip != null)
            .ToArray();

        // Resume each AudioSource found
        foreach (AudioSource source in audioSources)
        {
            source.UnPause();
        }
    }
}
