using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dictionary : MonoBehaviour {

    public Rigidbody Collectable;
   // public List<Transform> collectableSpawnPoints = new List<Transform>();
    Dictionary<string, Transform> collectableSpawnPoints = new Dictionary<string,Transform>();
	// Use this for initialization
	void Start () {
       Transform[] spawnPoints =Transform.FindObjectsOfType(typeof(Transform)) as Transform[];
       foreach (Transform t in spawnPoints)
       {
           if (t.tag == "CollectablePoint")
           {
               collectableSpawnPoints.Add(t.name, t);
           }
       }
        foreach(KeyValuePair<string, Transform> t in collectableSpawnPoints)
            Collectable = Instantiate(Collectable, t.Value.position, t.Value.rotation) as Rigidbody;
    }
	
	// Update is called once per frame
	void Update () {
	
	}


}
