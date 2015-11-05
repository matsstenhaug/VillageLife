using UnityEngine;
using System.Collections;


public class VillageLifeSimulator : MonoBehaviour
{
	
	ArrayList ents;
	ArrayList children;
	ArrayList deceases;
	int iteration = 0;
	int dead = 0;
	int newBorns = 0;
	int kills = 0;
	int ageDeaths = 0;
	
	// Use this for initialization
	void Start ()
	{
		ents = new ArrayList ();
		children = new ArrayList ();
		deceases = new ArrayList ();
		// creates 20 Entities.
		for (int i = 0; i < 20; i ++) {
			Entity e = new Entity(50,Random.Range(1,101),new ArrayList(),Random.Range(1,101),100,Random.Range(12,18),Random.Range(1,1001),new ArrayList(),new ArrayList(),Random.Range(1,101), Random.Range(1,5));
			ents.Add(e);
		}
		Debug.Log ("Created " + ents.Count + " Entities.");
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
				//Debug.Log("Child Created");
				Entity newEnt = new Entity(50,Random.Range(1,101),new ArrayList(),Random.Range(1,101),100,Random.Range(13,18),Random.Range(1,1001),new ArrayList(),new ArrayList(),Random.Range(1,101), Random.Range(1,5));
				e.isPregnant = false;
				e.children.Add(newEnt);
				e.partner.children.Add(newEnt);
				children.Add(newEnt);
				newBorns++;
			}
			e.hasLived = false; // resets and makes ready for next iteration
			e.IncreaseAge();

		}
	}
	
	// add STDs?
	void makeLoveChild(Entity e, Entity e2){
		if(e2.partner == null || e2.partner == e){
			if(Random.Range(1,101)>=e.potency){
				if(Random.Range(1,101)>=e2.potency){
					//Debug.Log("HAVING SEX");
					// Potency Success -> Pregnancy inc!
					if(!(e.children.Count >= e.maxChildren) && !(e2.children.Count >= e2.maxChildren)){
						if(!e2.isPregnant && !e.isPregnant){
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
					//break;
				}
				
				if (e2.sp == Entity.sexPref.a) {
					//break;
				}
				
				// MAKE SURE YOU DONT HAVE INCEST!! :P
				// Different sex
				if (e2.s != e.s) {
					//Bi or Hetero
					switch (e.sp) {
					case Entity.sexPref.bi:
						if (e2.sp == Entity.sexPref.bi || e2.sp == Entity.sexPref.hetero) {
							if(e.age < 42 && e2.age <42){
								makeLoveChild (e, e2);
							}else{
								makeLove(e,e2);
							}
						}
						break;
					case Entity.sexPref.hetero:
						if (e2.sp == Entity.sexPref.bi || e2.sp == Entity.sexPref.hetero) {
							if(e.age < 42 && e2.age <42){
								makeLoveChild (e, e2);
							}else{
								makeLove(e,e2);
							}
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

	void CreateNewDecease(){
		//Random - if higher than X -> create new decease
		int a = Random.Range (0,100);
		if (a < 10) { // X % chance
			int newID = deceases.Count;
			//Debug.Log("Size of Dec: " + newID);
			Decease d = new Decease(Random.Range(0,10),Random.Range(0,10), Random.Range(0,10), Random.Range(0,10), newID);
			deceases.Add(d);
		}

	}

	void InfectPeople(){
		if(deceases.Count > 0){
			foreach (Decease d in deceases) {
				foreach (Entity e in ents) {
					if(!e.infections.Contains(d) || !e.immunities.Contains(d) ){ // Not infected and not immune
						//infect
						e.infections.Add(d);
						//d.host = e;
						//d.personalRes = (e.strength+e.hp) / 2;
					}
				}
			}
		}
	}

	//Decrease deceases by resistanceDropRate

	void UpdateDeceases(){
		//Create new decease, based on chance.
		CreateNewDecease ();
		InfectPeople ();
	}

	void UpdatePeople() {
		foreach (Entity e in ents) {
			//go through people and take damage from infections
			foreach(Decease d in e.infections){
				//int ra = Random.Range(0,100);
				e.hp -=( d.lethality * (d.lethality/((e.strength+e.hp) / 2)));
			}
			//if entity.hp <= 0 -> Destroy and remove from list.
			if(e.hp <= 0){
				ents.Remove(e);
				Destroy(e);
				dead++;
				kills++;
				break;
			}else if(e.age >80){
				ents.Remove(e);
				Destroy(e);
				dead++;
				ageDeaths++;
				break;
			}
		}
	}

	void Update ()
	{
		newBorns = 0;
		kills = 0;
		ageDeaths = 0;
		// Update interactions, update deceases, update people.
		UpdateInteractions ();
		ArrayList newList = new ArrayList ();
		foreach (Entity e in ents) {
			newList.Add(e);
		}
		foreach (Entity e in children) {
			newList.Add(e);
		}
		children = new ArrayList ();
		//ents = new ArrayList ();
		ents = (ArrayList)newList.Clone ();
		/////END OF INTERACTIONS//////

		UpdateDeceases ();
		UpdatePeople (); // take damage and stuff;

		iteration++;
		Debug.Log ("Iteration: "+iteration+". Entities: "+ents.Count+". Dead: "+dead +", Babies: "+newBorns+", kills: "+kills+", oldies: "+ageDeaths);
		//findPartner (); // create in main class, so that it goes through all the entities.
		if (ents.Count == 0) {
			Destroy(this);
		}

		
	}
}

