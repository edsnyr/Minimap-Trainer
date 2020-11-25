using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class HealthBar : MonoBehaviour
{

    public Slider slider;
    public TextMeshProUGUI health;
    public Transform restartUI;
    public int maxHealth = 10;

    public void Awake() {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void Start() {
        AudioPlayer.missEvent.AddListener(TakeDamage);
    }

    public void TakeDamage() {
        slider.value--;
        health.text = slider.value + " / " + maxHealth;
        if(slider.value == 0) {
            Die();
        }
    }

    public void Die() {
        restartUI.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void Restart() {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }
}
