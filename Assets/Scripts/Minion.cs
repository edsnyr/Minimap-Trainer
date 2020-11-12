using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minion : MonoBehaviour
{

    public Side side;
    public Transform lanePoint; //first destination in lane after spawning
    public Collider2D laneBox; //area the minion will occupy after arriving to lane
    public Transform teamBase; //spawn area
    public SpriteRenderer minimapSprite;
    public float moveSpeed = 4f;
    public float aliveTimeBase = 10f; //time the minion will stay alive after arriving in lane

    [HideInInspector] public bool visible = false;


    /// <summary>
    /// Sets the appropriate spawn location and destination.
    /// </summary>
    /// <param name="basePos"></param>
    /// <param name="lp"></param>
    /// <param name="box"></param>
    public void Initialize(Transform basePos, Transform lp, Collider2D box) {
        transform.position = basePos.position;
        lanePoint = lp;
        laneBox = box;
        UpdateVisibility();
        StartCoroutine(GoToLane());
    }

    /// <summary>
    /// Moves the minion to the primary lane point, then to the combat area,
    /// and dies after a specified time.
    /// </summary>
    /// <returns></returns>
    private IEnumerator GoToLane() {
        yield return StartCoroutine(MoveToPoint(lanePoint.transform.position));
        yield return StartCoroutine(MoveToPoint(GetNewTarget()));
        yield return new WaitForSeconds(aliveTimeBase);
        Destroy(gameObject);
    }

    /// <summary>
    /// Moves the minion to the specified point.
    /// </summary>
    /// <param name="target"></param>
    /// <returns></returns>
    private IEnumerator MoveToPoint(Vector3 target) {
        while(transform.position != target) {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }

    /// <summary>
    /// Gets a point within the laneBox collider
    /// </summary>
    /// <returns></returns>
    private Vector3 GetNewTarget() {
        float x = Random.Range(laneBox.bounds.min.x, laneBox.bounds.max.x);
        float y = Random.Range(laneBox.bounds.min.y, laneBox.bounds.max.y);

        return new Vector3(x, y, 0);
    }

    /// <summary>
    /// If the minion is an enemy, checks if the player should be able to see it on the map
    /// and updates accordingly
    /// </summary>
    private void UpdateVisibility() {
        if(side == Side.Enemy) {
            if(visible) {
                minimapSprite.gameObject.SetActive(true);
            } else {
                minimapSprite.gameObject.SetActive(false);
            }
        } else {
            minimapSprite.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(side == Side.Enemy && collision.CompareTag("Vision")) { //if the enemy minion enters ally vision range
            visible = true;
            UpdateVisibility();
        }
        //since the minion will never leave vision before dying, not necessary to check if it leaves
    }
}
