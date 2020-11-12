using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    public float moveSpeed = 5;

    private void Update() {
        Move();
    }

    private void Move() {
            transform.localPosition += transform.up * moveSpeed * Time.deltaTime;
    }

    public void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            AudioPlayer.missEvent.Invoke();
            Debug.Log("Hit");
        }
    }

    public void OnTriggerExit2D(Collider2D collision) {
        if(collision.CompareTag("Boundary")) {
            Destroy(gameObject); //if the projectile is outside the play area, destroy it
        }
    }
}
