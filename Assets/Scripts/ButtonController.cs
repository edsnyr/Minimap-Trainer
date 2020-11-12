using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{

    public List<StatusButton> buttons;

    public Color visibleColor;
    public Color visibleColorPressed;
    public Color missingColor;
    public Color missingColorPressed;


    private void Start() {
        SetColors();
    }

    private void Update() {
        GetInput();
    }

    /// <summary>
    /// Calls a button event if the corresponding key is pressed.
    /// </summary>
    private void GetInput() {
        if(Input.GetButtonDown("TopClick")) {
            buttons[0].buttonEvent.Invoke();
        } else if(Input.GetButtonDown("BotClick")) {
            buttons[1].buttonEvent.Invoke();
        }
    }

    /// <summary>
    /// Gives the buttons appropriate colors for visible and missing statuses.
    /// </summary>
    private void SetColors() {
        foreach(StatusButton button in buttons) {
            button.visibleColor = visibleColor;
            button.visibleColorPressed = visibleColorPressed;
            button.missingColor = missingColor;
            button.missingColorPressed = missingColorPressed;
            button.UpdateButton();
        }
    }




}
