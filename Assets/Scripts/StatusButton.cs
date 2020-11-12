using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StatusButton : MonoBehaviour
{

    public UnityEvent buttonEvent;

    [HideInInspector] public Laner enemyLaner; //the laner this button tracks
    [HideInInspector] public Button button; //the actual button object in the scene
    TextMeshProUGUI textBox; //the text box displayed on the button
    [HideInInspector] public bool visible = false; //the current tracked status

    [HideInInspector] public Color visibleColor;
    [HideInInspector] public Color visibleColorPressed;
    [HideInInspector] public Color missingColor;
    [HideInInspector] public Color missingColorPressed;


    private void Awake() {
        if(buttonEvent == null) { buttonEvent = new UnityEvent(); }
        buttonEvent.AddListener(Flip);
        button = GetComponent<Button>();
        textBox = button.GetComponentInChildren<TextMeshProUGUI>();
        button.onClick.AddListener(() => buttonEvent.Invoke());
    }

    /// <summary>
    /// When clicked, flip visibility status and update display
    /// </summary>
    public void Flip() {
        visible = !visible;
        UpdateButton();
    }

    public void UpdateButton() {
        SetColors();
        SetText();
    }

    /// <summary>
    /// Change colors of the button to reflect the current visibility status
    /// </summary>
    private void SetColors() {
        ColorBlock colorBlock = button.colors;
        colorBlock.normalColor = visible ? visibleColor : missingColor;
        colorBlock.highlightedColor = visible ? visibleColor : missingColor;
        colorBlock.selectedColor = visible ? visibleColor : missingColor;
        colorBlock.pressedColor = visible ? visibleColorPressed : missingColorPressed;
        button.colors = colorBlock;
    }

    /// <summary>
    /// Changes the text of the button to reflect the current visibility status
    /// </summary>
    private void SetText() {
        textBox.text = button.name + ":\n" + (visible ? "Visible" : "Missing");
    }
}
