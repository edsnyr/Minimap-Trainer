using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jungler : MonoBehaviour
{
    //currently not used, needs work
    /*
     * Create points for Jungler to move between
     * Points hold list of viable next points
     * Jungler waits at point for a set time then chooses another
     * If junglers meet, they are revealed and one kills the other randomly after a time
     * Add gray masking sprite over map and sprite mask to simulate fog of war
     * Junglers have sprite masks to reveal fog of war
     * Jungler is different color to more easily see when they appear in a lane
     */

    public Side side;
    public JunglePoint currentPoint;

    public float moveSpeed = 5f;
    public float minMoveTime = 0.5f;


    void Start()
    {
        transform.position = currentPoint.transform.position;
        StartCoroutine(Wander());
    }

    private IEnumerator Wander() {
        while(true) {
            currentPoint = currentPoint.NextPoint(side);
            yield return StartCoroutine(MoveToPoint(currentPoint.transform.position));
            yield return new WaitForSeconds(Random.Range(minMoveTime, currentPoint.maxWaitTime));
        }
    }

    private IEnumerator MoveToPoint(Vector3 target) {
        while(transform.position != target) {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }


}
