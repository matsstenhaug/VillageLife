using UnityEngine;
using System.Collections;


public class VillageLifeSimulator : MonoBehaviour
{

	ArrayList<Entity> entities;

	// Use this for initialization
	void Start ()
	{
		for (int i = 0; i < 20; i ++) {
			Entity e = new Entity();
		}
	}


	void Update ()
	{
		findPartner (); // create in main class, so that it goes through all the entities.

	}
}

