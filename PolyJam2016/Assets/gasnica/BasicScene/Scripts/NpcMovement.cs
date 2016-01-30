using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class NpcMovement : MonoBehaviour {

  public GameObject NpcTemplate;
  
  public Transform NpcContainer;
  
  public Transform SpawnMarker;
  
  public float SpawnRadius = 10;
  
  public int NumNpcs = 10;
  
  List<Transform> Npcs = new List<Transform>();

	// Use this for initialization
	void Start () {
	  Random.seed = 123;
    SpawnNpcs();
	}
	
	// Update is called once per frame
	void Update () {
	  UpdateNpcs();
	}
  
  void SpawnNpcs()
  {
    for (int i = 0; i < NumNpcs; i++)
    {
      // Randomize position
      float r = SpawnRadius;
      Vector3 offset = new Vector3(Random.Range(-r, r), 0, Random.Range(-r, r));
      GameObject npc = Instantiate(NpcTemplate, SpawnMarker.position + offset, Quaternion.identity) as GameObject;
      npc.transform.parent = NpcContainer;
      Npcs.Add(npc.transform);
    }
  }
  
  void UpdateNpcs()
  {
//     var tmp = from npc in Npcs where null != npc.GetComponent<Boid>() select npc.GetComponent<Boid>();
//     List<Boid> boids = tmp as List<Boid>;
// 
//     foreach(Boid boid in boids)
//     {
//       // figure new direction
//       boid.run(boids);
//     }
//    
    Debug.Log("num npcs" + Npcs.Count);
    List<Boid> boids = new List<Boid>(); 
    foreach(Transform npc in Npcs)
    {
      Boid boid = npc.GetComponent<Boid>();
      boids.Add(boid);
      
    }
    
    foreach(Boid boid in boids) { boid.run(boids); }
  }
}
