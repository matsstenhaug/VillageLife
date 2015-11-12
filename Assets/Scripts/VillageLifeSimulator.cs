using UnityEngine;
using System.Collections;


public class VillageLifeSimulator : MonoBehaviour {
	ArrayList ents;
	ArrayList children;
	ArrayList diseases;
	int iteration = 0;
	int dead = 0;
	int newBorns = 0;
	int kills = 0;
	int ageDeaths = 0;
	int CanHaveKidsThresh = 42;
	int MAX_SUPPORT = 500;

    int itLastDisease = 1;

    // Use this for initialization
    void Start ()
	{
		ents = new ArrayList ();
		children = new ArrayList ();
		diseases = new ArrayList ();
		// creates 20 Entities.
		for (int i = 0; i < 50; i ++) {
			Entity e = new Entity(50,Random.Range(1,101),new ArrayList(),Random.Range(1,101),100,Random.Range(12,18),Random.Range(1,1001),new ArrayList(),new ArrayList(),Random.Range(1,101), Random.Range(1,5));
			ents.Add(e);
		}
		Debug.Log ("Created " + ents.Count + " Entities.");
	}

    #region Interactions Updates
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
				Entity newEnt = new Entity(50,Random.Range(1,101),new ArrayList(),Random.Range(1,101),100,Random.Range(13,18),Random.Range(1,1001),new ArrayList(),new ArrayList(),Random.Range(1,101), Random.Range(1,20));
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
		if(e2.partner == null || e2.partner == e){ // e can cheat :D
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
							if(e.age < CanHaveKidsThresh && e2.age < CanHaveKidsThresh){
								makeLoveChild (e, e2);
							}else{
								makeLove(e,e2);
							}
						}
						break;
					case Entity.sexPref.hetero:
						if (e2.sp == Entity.sexPref.bi || e2.sp == Entity.sexPref.hetero) {
							if(e.age < CanHaveKidsThresh && e2.age <CanHaveKidsThresh){
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
    #endregion

    #region Diseases Updates
    //Decrease Diseases by resistanceDropRate

    void UpdateDiseases() {
        //Create new Disease, based on chance.
        CreateNewDisease();
        InfectPeople();
        foreach(Disease d in diseases)
            d.lifespan++;
        CleanupDiseases();
    }

    void CleanupDiseases() {
        ArrayList current = new ArrayList();
        foreach(Disease d in diseases) {
            foreach (Entity e in ents)
                if (e.infections.Contains(d)) {
                    current.Add(d);
                    break;
                }
            if(d.lifespan > d.lifetime)
                current.Remove(d);
        }
        diseases = current;
    }

    void CreateNewDisease(){
		//Random - if higher than X -> create new Disease
		int a = Random.Range (0,100);
        float chance = 100; 
        if(diseases.Count > 0) {
            //print("chance = (Poulation " + ents.Count + " / VillageSupport " + MAX_SUPPORT + ") * time " + itLastDisease + " / currentDiseases " + Diseases.Count);
            chance = ((((float)ents.Count / (float)MAX_SUPPORT) * itLastDisease) / diseases.Count) * 100;
        }
        if (a <= chance) { // X % chance
            //print("A new Disease has emerged! " + chance);
            Disease d = new Disease(Random.Range(0, 10), Random.Range(0, 100), Random.Range(0, 10), Random.Range(0, 10), null);
            diseases.Add(d);
            itLastDisease = 1;
        }

	}

    //TODO: Spread Diseases (with chance of mutation) 
    // 

    void InfectPeople() {
		if(diseases.Count > 0){
			foreach (Disease d in diseases) {
				foreach (Entity e in ents) {
					if(!e.infections.Contains(d) && !e.immunities.Contains(d) ) { // Not infected and not immune
                        int a = Random.Range(0, 100);
                        if(a <= (int)d.infectionRate) //infect
                            e.infections.Add(d);
						//d.host = e;
						//d.personalRes = (e.strength+e.hp) / 2;
					}
				}
			}
		}
	}
    #endregion

    #region People Updates
    void UpdatePeople() {
		int deaths = 0;
		int killed = 0;
		int aged = 0;
        ArrayList deadPeeps = new ArrayList();
		foreach (Entity e in ents) {
            //go through people and take damage from infections
            DamagePeople(e);
            //if entity.hp <= 0 -> Destroy and remove from list.
            if (e.hp <= 0){
				deaths++;
				killed++;
                deadPeeps.Add(e);
			}else if(e.age > CanHaveKidsThresh){ // Basically kills the people who are Unable to reproduce.
				deaths++;
				aged++;
                deadPeeps.Add(e);
            }
		}
        foreach(Entity e in deadPeeps)
            ents.Remove(e);

        dead += deaths;
		kills += killed;
		ageDeaths += aged;
	}

    void DamagePeople(Entity e) {
        foreach (Disease d in e.infections) {
            //int ra = Random.Range(0,100);
            e.hp -= (d.lethality / d.lifespan);// (d.lethality/((e.strength+e.hp) / 2)));
        }
    }
    #endregion

    void Update () {
		newBorns = 0;
		kills = 0;
		ageDeaths = 0;
		// Update interactions, update Diseases, update people.
		UpdateInteractions ();
		ArrayList newList = new ArrayList ();
		foreach (Entity e in ents) {
			if(newList.Count == MAX_SUPPORT) 
				break;
			newList.Add(e);
		}
		foreach (Entity e in children) {
			if(newList.Count == MAX_SUPPORT)
				break;
			newList.Add(e);
		}
		children = new ArrayList ();
		//ents = new ArrayList ();
		ents = (ArrayList)newList.Clone ();
		/////END OF INTERACTIONS//////

		UpdateDiseases ();
		UpdatePeople (); // take damage and stuff;

		iteration++;
        itLastDisease++;
        int infected = 0;
        float averageDiseases = 0;
        foreach (Entity e in ents) {
            if (e.infections.Count > 0) {
                infected++;
                averageDiseases += (float)e.infections.Count;
            }
        }
        if(infected > 0)
            averageDiseases /= (float)infected;
        Debug.Log ("Iteration: " + iteration + ". Entities: " + ents.Count + ". Dead: "+dead +", Babies: "+newBorns+", kills: "+kills+", oldies: "+ageDeaths + 
            ", infected: " + infected + ", average: " + (float)averageDiseases + ", Diseases: " + diseases.Count);
		//findPartner (); // create in main class, so that it goes through all the entities.
		if (ents.Count == 0) {
			Destroy(this);
		}

		
	}
}

