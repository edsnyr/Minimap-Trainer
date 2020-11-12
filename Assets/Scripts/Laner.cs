using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laner : MonoBehaviour
{
    public Side side;
    public Position position;
    public Scoreboard scoreboard;
    public StatusButton statusButton; //the button that the player uses to track if the laner is missing
    public BoxCollider2D laneCollider; //bounds a laner can roam around inside
    public Transform missingPoint; //where a laner will go to leave vision
    public Transform teamBase; //spawn point
    public SpriteRenderer minimapSprite; //the attached sprite that appears on the minimap
    public float moveSpeed = 5f;
    public float minMoveTime = 0.5f; //min time a laner will stay still in lane
    public float maxMoveTime = 2f; //max time a laner will stay still in lane
    public int missingWeight = 8;
    public int recallWeight = 3;
    public float minMissingTime = 3f; // minimum time a laner will leave vision
    public float maxMissingTime = 15f; //maximum time a laner will leave vision
    public float recallTime = 4f; //time it takes to teleport back to base
    public float startWaitTime = 2f; //time the laner will wait before moving at the start of the game

    [HideInInspector] public float timeSinceChangedVisibility = 0; //tracked for scoring
    [HideInInspector] public string lastTime = "None";
    [HideInInspector] public bool visible = false;
    [HideInInspector] public bool waitingForRecognition = false; //true if visibility has changed and button not pressed yet

    int visionCount = 0;


    void Start()
    {
        transform.position = teamBase.position;
        UpdateVisibility();
        if(side == Side.Enemy) { 
            statusButton.buttonEvent.AddListener(ClickedButton);
            statusButton.enemyLaner = this;
            waitingForRecognition = false;
            StartCoroutine(Timer());
        }
        StartCoroutine(Wander());
    }

    /// <summary>
    /// The laner roams in the lane box to random positions until it decides to go missing
    /// </summary>
    /// <returns></returns>
    private IEnumerator Wander() {
        yield return new WaitForSeconds(Random.Range(startWaitTime, startWaitTime + 2));
        while(true) {
            yield return StartCoroutine(MoveToPoint(GetNewTarget())); //find a new point
            yield return new WaitForSeconds(Random.Range(minMoveTime, maxMoveTime)); //wait a random amount of time
            if(Random.Range(0,missingWeight) == 0) { //if met, leave vision
                if(Random.Range(0,recallWeight) == 0) { //if met, recall to base
                    yield return StartCoroutine(GoRecall());
                } else { //if not, stay at the missing point
                    yield return StartCoroutine(GoMissing());
                }
            }
        }
    }

    /// <summary>
    /// Waits at the missing point for a random amount of time, then returns to wandering
    /// </summary>
    /// <returns></returns>
    private IEnumerator GoMissing() {
        Debug.Log("Go Missing");
        yield return StartCoroutine(MoveToPoint(missingPoint.position));
        yield return new WaitForSeconds(Random.Range(minMissingTime, maxMissingTime));
    }

    /// <summary>
    /// Waits at the missing point to recall, teleports to base, then walks back and
    /// returns to wandering
    /// </summary>
    /// <returns></returns>
    private IEnumerator GoRecall() {
        Debug.Log("Go Recall");
        yield return StartCoroutine(MoveToPoint(missingPoint.position));
        yield return new WaitForSeconds(recallTime);
        transform.position = teamBase.position;
    }

    /// <summary>
    /// Gets a random point inside the lane box
    /// </summary>
    /// <returns></returns>
    private Vector3 GetNewTarget() {
        float x = Random.Range(laneCollider.bounds.min.x, laneCollider.bounds.max.x);
        float y = Random.Range(laneCollider.bounds.min.y, laneCollider.bounds.max.y);

        return new Vector3(x,y,0);
    }

    /// <summary>
    /// Moves the laner to the specified point
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private IEnumerator MoveToPoint(Vector3 target) {
        while(transform.position != target) {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Vision")) {
            visionCount++;
            if(!visible) {
                visible = true;
                if(side == Side.Enemy)
                    ResetTimer();
                UpdateVisibility();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(collision.CompareTag("Vision")) {
            visionCount--;
            if(visionCount == 0) { //no longer in range of any ally vision
                visible = false;
                if(side == Side.Enemy)
                    ResetTimer();
                UpdateVisibility();
            }
        }
    }

    /// <summary>
    /// Hides enemy laners on the minimap when they are not in range of ally vision
    /// </summary>
    private void UpdateVisibility() {
        if(side == Side.Enemy) {
            if(visible) {
                minimapSprite.gameObject.SetActive(true);
            } else {
                minimapSprite.gameObject.SetActive(false);
            }
            timeSinceChangedVisibility = 0;
            waitingForRecognition = true;
        } else {
            minimapSprite.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Sets the timer back to zero. If the laner was not recognized for entering/leaving
    /// vision, the player is penalized.
    /// </summary>
    private void ResetTimer() {
        if(visible == statusButton.visible && waitingForRecognition) {
            AudioPlayer.missEvent.Invoke();
            SetLastTime();
        }
        timeSinceChangedVisibility = 0;
    }

    /// <summary>
    /// Constantly running timer.
    /// </summary>
    /// <returns></returns>
    private IEnumerator Timer() {
        while(true) {
            timeSinceChangedVisibility += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// If the laner is waiting for recognition when called, the state is flipped and recognized.
    /// If the laner is not waiting for recognition, the player is penalized.
    /// </summary>
    private void ClickedButton() {
        //the button's status is flipped before this function is called
        if(visible == statusButton.visible) { //if the status of the button and laner match, the player is scored for time
            waitingForRecognition = false;
            AudioPlayer.hitEvent.Invoke();
            SetLastTime();
        } else { //the player was wrong
            AudioPlayer.missEvent.Invoke();
            Debug.Log("Still " + (visible ? "visible." : "missing."));
            statusButton.Flip(); //return the button to original state
        }
    }

    /// <summary>
    /// Gets the timer's status and reports it to the scoreboard.
    /// </summary>
    /// <returns></returns>
    private float SetLastTime() {
        float time = (int)(timeSinceChangedVisibility * 1000) / 1000f;
        scoreboard.Score(time, position);
        return time;
    }
}

public enum Side { Ally, Enemy };
public enum Position { Top, Bot };
