using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{

    public float moveSpeed = 5;

    Vector3 target; //where the player wants to go

    void Start()
    {
        target = transform.position;
        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {
        GetClicked();
    }

    /// <summary>
    /// If the right mouse button is down, get a new target.
    /// </summary>
    private void GetClicked() {
        if(Input.GetButton("Fire2")) {
            ChangeTarget();
        }
    }

    /// <summary>
    /// Sets the mouse position as the new target.
    /// </summary>
    private void ChangeTarget() {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target = new Vector3(pos.x, pos.y, 0);
    }

    /// <summary>
    /// The player is always moving toward the target position.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Move() {
        while(true) {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    
}
