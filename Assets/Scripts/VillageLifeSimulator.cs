using UnityEngine;
using System.Collections;


public class VillageLifeSimulator : MonoBehaviour
{

	ArrayList ents;

	// Use this for initialization
	void Start ()
	{
		ents = new ArrayList ();
		// creates 20 Entities.
		for (int i = 0; i < 20; i ++) {
			Entity e = new Entity(50,Random.Range(1,101),null,Random.Range(1,101),100,Random.Range(12,18),Random.Range(1,1001),null,null,Random.Range(1,101));
			ents.Add(e);
		}
	}

	void UpdateInteractions(){
		foreach(Entity e in ents) {


		}
		foreach(Entity e in ents) {
			
		}
	}

	void Update ()
	{
		// Update interactions, update deceases, update people.
		UpdateInteractions ();

		//findPartner (); // create in main class, so that it goes through all the entities.

	}
}

