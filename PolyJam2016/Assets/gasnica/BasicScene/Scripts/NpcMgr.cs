using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/*
Todo: 
 + set target positon for boid
   - improvements:
     - stronger stopping when reaching target
 - always escape chasing players
   - optional: have some max escape speed
 - avoid obstacles

*/

public class NpcMgr : MonoBehaviour {

  public GameObject NpcTemplate;
  
  public Transform NpcContainer;
  
  public Transform SpawnMarker;
  
  public float SpawnRadius = 10;
  
  public int NumNpcs = 10;
         
  [System.Serializable]  
  public class Attractor
  {
    public Transform Transform;
    public float Strength = 1; // Can be negative
    public float Radius = 10; // Radius beyond which attraction works, and below which repulsion workss
  }
  
  [System.Serializable]
  public class NpcInputStruct
  {
    public float MovementSpeed = 10;
    public float NeighbourDist = 100;    
    public float DesiredSeparation = 5;
    public float StartSpeed = 1;
    public List<Attractor> Attractors;
  }
  
  public NpcInputStruct BoidInput = new NpcInputStruct();
    
  
  List<Transform> Npcs = new List<Transform>();

	// Use this for initialization
	void Start () {
    // Initialize structs
    
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
      Npc boid = npc.GetComponent<Npc>();
      if (boid) { boid.Init(BoidInput); }
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
    List<Npc> boids = new List<Npc>(); 
    foreach(Transform npc in Npcs)
    {
      Npc boid = npc.GetComponent<Npc>();
      boids.Add(boid);
      
    }
        
    foreach(Npc boid in boids) { boid.run(boids, BoidInput); }
  }
}
