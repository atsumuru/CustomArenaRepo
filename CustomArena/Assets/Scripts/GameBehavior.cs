using UnityEngine;
using UnityEngine.UI;
using TMPro; 
using UnityEngine.SceneManagement;

using CustomExtensions;

public class GameBehavior : MonoBehaviour, IManager
{
    private int _itemsCollected = 0; // Items Collected
    public int _playerHP = 10; // Player Health
    public int MaxItems = 3; // Maximum Items to Win

    public float _playerSpeed = 5f;// Player Speed

    public TMP_Text HealthText; // Health Display
    public TMP_Text ItemText; // Items Display
    public TMP_Text ProgressText; // Progress Display

    public Button WinButton; // Win Button
    public Button LossButton;

    public TMP_Text SpeedText; // Speed Pickup


    private string _state;
    public string State
    {
        get { return _state; }
        set { _state = value; }
    }


    void Start()
    {
        ItemText.text += _itemsCollected; // Initialize Items Display, adds starting value
        HealthText.text += _playerHP; // Initialize Health Display, adds starting value
        SpeedText.text += _playerSpeed; // Initialize Speed Display, adds starting value
    
        Initialize();
    }

    public void Initialize()
    {
        _state = "Game Manager initialized..";

        _state.FancyDebug();
    }

    public void UpdateScene(string updatedText)
    {
        ProgressText.text = updatedText;
        Time.timeScale = 0f;
    }

    public int Items // Property for Items Collected
    {
        get { return _itemsCollected; }
        set
        {
            _itemsCollected = value;
            ItemText.text = "Items: " + Items;
            if (_itemsCollected >= MaxItems)
            {
                WinButton.gameObject.SetActive(true);
                UpdateScene("You've found all the items!");
            }
            else
            {
                ProgressText.text = "Item found, only " + (MaxItems - _itemsCollected) + " more to go!";
            }

// edits the progress text and checks for win condition

            if (_itemsCollected >= MaxItems) // Reaches max items
            {
                ProgressText.text = "You've found all the items!"; // Win text
                WinButton.gameObject.SetActive(true); // activate win button
                Time.timeScale = 0f; // Pause game
            }
            else
            {
                ProgressText.text = "Item found, only " + (MaxItems - _itemsCollected) + " more to go!"; // progress text
            }
        }
    }

    public int HP // Property for Player Health
    {
        get { return _playerHP; } // Get player health
        set
        {
            _playerHP = value;
                HealthText.text = "Health: " + HP; // Update health display
                Debug.LogFormat("Lives: {0}", _playerHP); // Log current health
                
            if (_playerHP <= 0)
            {
                ProgressText.text = "You want another life with that?";
                LossButton.gameObject.SetActive(true);
                Time.timeScale = 0;
            }
            else
            {
                ProgressText.text = "Ouch... that's gotta hurt.";
            }
        }
    }
    
    public float Speed // Property for Player Speed
    {
        get { return _playerSpeed; } // Get player speed
        set
        {
            Debug.Log(_playerSpeed);
            _playerSpeed = value;
            Debug.Log(_playerSpeed);
            SpeedText.text = "Speed: " + _playerSpeed; // Update speed display
            // Debug.LogFormat("Speed: {0}", _playerSpeed); // Log current speed
        }
    }

    public void RestartScene() // Restart Scene Method
    {
        Utilities.RestartLevel(0);
    }
}