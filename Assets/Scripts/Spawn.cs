using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Spawn : MonoBehaviour {
    public Rigidbody[] PickUps = new Rigidbody[3];
    public Transform[] projectileSpawnPoints = new Transform[5];
    //int[] usedSpawnPoint = new int[3];
    int RandomPoint;
	// Use this for initialization
	void Start () {
        //will be used for powerup generation
        for (int i = 0; i < projectileSpawnPoints.Length; i++){
            RandomPoint = Random.Range(0, PickUps.Length);
            PickUps[RandomPoint] = Instantiate(PickUps[RandomPoint], projectileSpawnPoints[i].position, projectileSpawnPoints[i].rotation) as Rigidbody;
        }

        /* Spawn one of each object in one of the 5 spawn points (FINAL GAME FUNCTIONALITY: will be used for bullet generation)
        for (int j = 0; j < usedSpawnPoint.Length; j++)
            usedSpawnPoint[j] = 10;
            for (int i = 0; i < PickUps.Length; i++){
                while (usedSpawnPoint.Contains(RandomPoint)){
                    RandomPoint = Random.Range(0, projectileSpawnPoints.Length);
                }
                usedSpawnPoint[i] = RandomPoint;
                Debug.Log(RandomPoint);
                PickUps[i] = Instantiate(PickUps[i], projectileSpawnPoints[RandomPoint].position, projectileSpawnPoints[RandomPoint].rotation) as Rigidbody;
            }
         */
	}
	
	// Update is called once per frame
	void Update () {
	   
	}
}
