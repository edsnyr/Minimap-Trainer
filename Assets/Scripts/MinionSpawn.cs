using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawn : MonoBehaviour
{

    public float timeBetweenWaves;
    public float timeBetweenMinions;
    public int minionsPerWave;

    public Minion allyMinion;
    public Minion enemyMinion;

    public Transform topAllyLanePoint;
    public Transform botAllyLanePoint;
    public Transform topEnemyLanePoint;
    public Transform botEnemyLanePoint;
    public Transform allyBase;
    public Transform enemyBase;

    public Collider2D topLaneBox;
    public Collider2D botLaneBox;

    private void Start() {
        StartCoroutine(SpawnMinions());
    }


    /// <summary>
    /// Spawns waves of minions at a consistent rate.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnMinions() {
        while(true) {
            for(int i = 0; i < minionsPerWave; i++) {
                Minion minion = Instantiate(allyMinion);
                minion.Initialize(allyBase, topAllyLanePoint, topLaneBox);
                minion = Instantiate(allyMinion);
                minion.Initialize(allyBase, botAllyLanePoint, botLaneBox);
                minion = Instantiate(enemyMinion);
                minion.Initialize(enemyBase, topEnemyLanePoint, topLaneBox);
                minion = Instantiate(enemyMinion);
                minion.Initialize(enemyBase, botEnemyLanePoint, botLaneBox);
                yield return new WaitForSeconds(timeBetweenMinions);
            }
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }



}
