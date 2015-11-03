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
			e.hasLived = true;
			meetPeople(e);
		}
		foreach(Entity e in ents) {
			// give birth to new children.
			// Make the child dependant on the parents??
			if(e.isPregnant){
				Entity newEnt = new Entity(50,Random.Range(1,101),null,Random.Range(1,101),100,Random.Range(12,18),Random.Range(1,1001),null,null,Random.Range(1,101));
				e.isPregnant = false;
				e.children.Add(newEnt);
				e.partner.children.Add(newEnt);
			}
			e.hasLived = false; // resets and makes ready for next iteration
		}
	}

	// add STDs?
	void makeLoveChild(Entity e, Entity e2){
		if(e2.partner == null || e2.partner == e){
			if(Random.Range(1,101)>=e.potency){
				if(Random.Range(1,101)>=e2.potency){
					// Potency Success -> Pregnancy inc!
					if(e.s == Entity.sex.female){
						e.isPregnant = true;
						e.partner = e2;
						e2.partner = e;
					}else{
						e2.isPregnant = true;
						e.partner = e2;
						e2.partner = e;
					}
				}
			}
		}
	}
	// add STDs?
	void makeLove(Entity e, Entity e2){
	
	}

	void meetPeople(Entity e){
		if (e.hasSexPref) {
			foreach (Entity e2 in ents) {
				//a sexual = break
				if (e.sp == Entity.sexPref.a) {
					break;
				}

				if (e2.sp == Entity.sexPref.a) {
					break;
				}

				// MAKE SURE YOU DONT HAVE INCEST!! :P
				// Different sex
				if (e2.s != e.s) {
					//Bi or Hetero
					switch (e.sp) {
					case Entity.sexPref.bi:
						if (e2.sp == Entity.sexPref.bi || e2.sp == Entity.sexPref.hetero) {
							makeLoveChild (e, e2);
						}
						break;
					case Entity.sexPref.hetero:
						if (e2.sp == Entity.sexPref.bi || e2.sp == Entity.sexPref.hetero) {
							makeLoveChild (e, e2);
						}
						break;
					}
				}
			//Same Sex
			else {
					//Bi or Homo
					switch (e.sp) {
					case Entity.sexPref.bi:
						if (e2.sp == Entity.sexPref.bi || e2.sp == Entity.sexPref.hetero) {
							makeLove (e, e2);
						}
						break;
					case Entity.sexPref.homo:
						if (e2.sp == Entity.sexPref.bi || e2.sp == Entity.sexPref.homo) {
							makeLove (e, e2);
						}
						break;
					}
				}
			}
		}
	}

	void Update ()
	{
		// Update interactions, update deceases, update people.
		UpdateInteractions ();

		//findPartner (); // create in main class, so that it goes through all the entities.

	}
}

