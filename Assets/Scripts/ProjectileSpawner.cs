using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{

    public Projectile projectilePrefab;
    public CharacterController character; //player character
    public float spawnRadius = 5f; //how far from the origin to spawn from
    public float timeBetweenProjectiles = 1f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnProjectiles());
    }

    private IEnumerator SpawnProjectiles() {
        while(true) {
            Projectile projectile = Instantiate(projectilePrefab);
            Vector3 spawnVector = Quaternion.AngleAxis(Random.Range(0,360), Vector3.forward) * Vector3.right; //a random direction to spawn in
            projectile.transform.position = spawnVector.normalized * spawnRadius; //place the projectile a specified distance away in the new direction
            Vector3 angleVector = character.transform.position - projectile.transform.position; //vector3 between the projectile and player
            float angle = Vector3.Angle(new Vector3(angleVector.x, angleVector.y, 0), Vector3.up); //angle between the projectile and player
            if(angleVector.x > 0) //since the found angle is given between 0 and 180 in either direction, it may need to be flipped to be correct
                angle *= -1;
            projectile.transform.localEulerAngles = new Vector3(0, 0, angle); //look at the player
            yield return new WaitForSeconds(timeBetweenProjectiles);
        }
    }
}
