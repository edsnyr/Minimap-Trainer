using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour {

    public Button playButton;
    public Button howToPlayButton;
    public Button settingsButton;
    public Button creditsButton;
    public Button backButton;

    public static UnityEvent escapeEvent;

    public GameObject mainMenu;
    public GameObject howToPlayMenu;
    public GameObject settingsMenu;
    public GameObject creditsMenu;

    private void Awake() {
        if(escapeEvent == null) { escapeEvent = new UnityEvent(); }
        playButton.onClick.AddListener(() => SceneManager.LoadScene("Game"));
        escapeEvent.AddListener(() => SetMenu(mainMenu));
        backButton.onClick.AddListener(() => escapeEvent.Invoke());
        howToPlayButton.onClick.AddListener(() => SetMenu(howToPlayMenu));
        settingsButton.onClick.AddListener(() => SetMenu(settingsMenu));
        creditsButton.onClick.AddListener(() => SetMenu(creditsMenu));
        SetMenu(mainMenu);
    }

    private void Update() {
        GetInput();
    }

    /// <summary>
    /// Checks if the escape button has been pressed.
    /// </summary>
    private void GetInput() {
        if(Input.GetButtonDown("Cancel")) {
            escapeEvent.Invoke();
        }
    }

    /// <summary>
    /// Displays the appropriate menu while hiding the others/
    /// </summary>
    /// <param name="menu"></param>
    private void SetMenu(GameObject menu) {
        mainMenu.SetActive(menu == mainMenu);
        howToPlayMenu.SetActive(menu == howToPlayMenu);
        settingsMenu.SetActive(menu == settingsMenu);
        creditsMenu.SetActive(menu == creditsMenu);
        backButton.gameObject.SetActive(menu != mainMenu);
    }
}
