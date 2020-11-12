using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunglePoint : MonoBehaviour
{

    /*
    This script is not currently implemented.
     */

    public List<Side> availability; //a jungler can only visit the point if its side is listed here
    public List<JunglePoint> adjacents; //all points a neutral jungler could go to from this point
    public float maxWaitTime; //maximum time spent at this point

    public JunglePoint NextPoint(Side side) {
        while(true) {
            int rand = Random.Range(0, adjacents.Count);
            if(adjacents[rand].availability.Contains(side)) {
                return adjacents[rand];
            }
        }
    }
}
