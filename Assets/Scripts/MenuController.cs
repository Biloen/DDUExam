using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using .UI, so we can get slider value and text component text
using UnityEngine.UI;
// Using scenemanager so we can reset the scene
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Gameobjects for each player, so we can enable/disable them
    public GameObject player1, player2, player3, player4;

    // Timer, so players appear when voice ends
    float timer;
    // bool, so we can have switch that enables player objects
    bool startgame;

    // 5 empties, whose children either have audiosources that play on awake (when we enable them), or to show/hide UI elements on canvas
    public GameObject mainMenu, gameMenu, mainSound,gameSound, endMenu;

    // UI elements, whose data we manipulate when deciding how many players in each game
    public GameObject playerCounttext, playerSlider;
    void Start()
    {
        // Main menu is typically disabled in editor, because the grey overlay looks annoying when editing scene
        mainMenu.SetActive(true);
    }
    void Update()
    {
        // Constant update, to display to users how many players will be playing in the round
        playerCounttext.GetComponent<Text>().text = "Players: " + playerSlider.GetComponent<Slider>().value.ToString();

        // If game started, start counter so we know to enable players after...
        if (startgame)
            timer = timer + Time.deltaTime;

        // ... 9 seconds has passed, as specified in this if statement
        if (timer > 9)
        {
            // Switch with 3 cases, either 2,e or 4 players can play the game. To check how many we use a UI element called a slider, that has a int value on slider.value
            switch (playerSlider.GetComponent<Slider>().value)
            {
                case 2:
                    // Enable player 1 (thief and 1 catcher)
                    player1.gameObject.SetActive(true);
                    player2.gameObject.SetActive(true);
                    // set game UI to active, so players can see points etc
                    gameMenu.SetActive(true);
                    break;
                // Repeat but 1 thief and 2 catchers this time
                case 3:
                    player1.gameObject.SetActive(true);
                    player2.gameObject.SetActive(true);
                    player3.gameObject.SetActive(true);
                    gameMenu.SetActive(true);
                    break;
                case 4:
                    player1.gameObject.SetActive(true);
                    player2.gameObject.SetActive(true);
                    player3.gameObject.SetActive(true);
                    player4.gameObject.SetActive(true);
                    gameMenu.SetActive(true);
                    break;
                // Failsafe for debugging
                default:
                    print("How tf you end up here?");
                    break;
            }
        }
    }
    // Function to be used when pressing start game button in menu
    public void StartGame()
    {
        // Disable main menu music, and main menu UI
        mainSound.SetActive(false);
        mainMenu.SetActive(false);

        // Enable game music, and voice (which has a 6.06 seconds delay, to fit with first loop of music)
        gameSound.SetActive(true);

        // Start timer from before
        startgame = true;
    }
    public void EndGame()
    {
        gameMenu.SetActive(false);


        endMenu.SetActive(true);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
