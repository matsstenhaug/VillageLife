using UnityEngine;
using System.Collections;

public class VillagePeopleSimulator : MonoBehaviour {
	ArrayList ents;
	ArrayList children;
	ArrayList diseases;
    public int initalPop = 50;
    int iteration = 0;
	int dead = 0;
	int newBorns = 0;
	public int kills = 0;
	int ageDeaths = 0;
	int CanHaveKidsThresh = 42;
	int MAX_SUPPORT = 500;
    public float damageDealt = 0;
    string state;
    int ageTally = 0;

    int createdDiseases = 0;
    int diseaseFreeIteration = 0;

    int itLastDisease = 1;

    string bonusString;

    public bool isSimulation = false;
    public int recoveringPeriod = 10;

    public ArrayList getEntities() { return ents; }
    public ArrayList getDiseases() { return diseases; }

    public int getIterations() { return iteration; }

    #region Methods for stating Simulator
    public void InitSimulation(ArrayList ents, Disease d) {
        this.ents = ents;
        children = new ArrayList();
        diseases = new ArrayList();
        ((Entity)ents[Random.Range(0, ents.Count)]).infect(d);
        diseases.Add(d);
        isSimulation = true;
        iteration = 0;
    }

    public VillagePeopleSimulator Clone() {
        VillagePeopleSimulator vps = new VillagePeopleSimulator();
        ArrayList upEnt = new ArrayList();
        foreach (Entity e in ents) {
            upEnt.Add(e.Copy());
        }
        vps.ents = upEnt;
        vps.children = new ArrayList();
        vps.diseases = (ArrayList)diseases.Clone();
        return vps;
    }

    public void AddDisease(Disease d) {
        int roll = Random.Range(0, ents.Count);
        ((Entity)ents[roll]).infect(d);
        //print("entity #" + roll + " was infected!");
        diseases.Add(d);
    }
    
    // Use this for initialization
    public void Start () {
		ents = new ArrayList ();
		children = new ArrayList ();
		diseases = new ArrayList ();
		// creates 20 Entities.
		for (int i = 0; i < initalPop; i ++) {
			Entity e = new Entity(Random.Range(15, 25), 50, Random.Range(1,101), Random.Range(1,101), 100, 
                Random.Range(12,18), Random.Range(1,1001), new ArrayList(), new ArrayList(), Random.Range(1,101), Random.Range(1,5), Random.Range(1,3));
			ents.Add(e);
		}
		Debug.Log ("Created " + ents.Count + " Entities.");
	}

    public void Start(int initPop, int maxPop, int recov = 10) {
        initalPop = initPop;
        MAX_SUPPORT = maxPop;
        recoveringPeriod = recov;
        Start();
        bonusString += "<<Sanctuary period: " + recoveringPeriod + " years>>" + "\n\r";
    }
    #endregion

    #region Interactions Updates
    ArrayList UpdateInteractions(ArrayList enties) {
        ArrayList entities = new ArrayList(enties);
        foreach (Entity e in entities) {
            ageTally++;
            e.hasLived = true;
			meetPeople(e);
		}
        foreach (Entity e in entities) {
            // give birth to new children.
            // Make the child dependant on the parents??
            if (e.isPregnant) {
                //Debug.Log("Child Created");
                Entity newEnt = new Entity(0, 50, Random.Range(1, 101), Random.Range(1, 101), 100, Random.Range(13, 18), Random.Range(1, 1001), new ArrayList(), new ArrayList(), Random.Range(1, 101), Random.Range(1, 20), Random.Range(1,3));
                e.isPregnant = false;
                e.children.Add(newEnt);
                e.partner.children.Add(newEnt);
                children.Add(newEnt);
                newBorns++;
            }
            e.hasLived = false; // resets and makes ready for next iteration
            e.IncreaseAge();

        }
        return entities;
    }
	
	// add STDs?
	void makeLoveChild(Entity e, Entity e2) {
		if(e2.partner == null || e2.partner == e) { // e can cheat :D
			if(Random.Range(1,101)>=e.potency) {
				if(Random.Range(1,101)>=e2.potency) {
					// Potency Success -> Pregnancy inc!
					if(!(e.children.Count >= e.maxChildren) && !(e2.children.Count >= e2.maxChildren)) {
						if(!e2.isPregnant && !e.isPregnant) {
							if(e.s == Entity.sex.female) {
								e.isPregnant = true;
								e.partner = e2;
								e2.partner = e;
							} else {
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
	void makeLove(Entity e, Entity e2){ }
	
	void meetPeople(Entity e) {
		if (e.hasSexPref) {
			foreach (Entity e2 in ents) {
				// Different sex
				if (e2.s != e.s) {
					//Bi or Hetero
					switch (e.sp) {
					case Entity.sexPref.bi:
						if (e2.sp == Entity.sexPref.bi || e2.sp == Entity.sexPref.hetero) {
							if(e.age < CanHaveKidsThresh && e2.age < CanHaveKidsThresh) {
								makeLoveChild (e, e2);
							} else {
								makeLove(e, e2);
							}
						}
						break;
					case Entity.sexPref.hetero:
						if (e2.sp == Entity.sexPref.bi || e2.sp == Entity.sexPref.hetero) {
							if(e.age < CanHaveKidsThresh && e2.age <CanHaveKidsThresh) {
								makeLoveChild (e, e2);
							} else {
								makeLove(e, e2);
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
    
    ArrayList UpdateDiseases(ArrayList ents) {
        ArrayList entities = new ArrayList(ents);
        //Create new Disease, based on chance.
        entities = CreateNewDisease(entities);
        entities = InfectPeople(entities);
        CleanupDiseases();
        return entities;
    }

    void CleanupDiseases() {
        ArrayList current = new ArrayList();
        foreach(Entity e in ents) {
            for(int i = 0; i < e.infections.Count; i++) {
                Disease d = (Disease)e.infections[i];
                if (!current.Contains(d) && (float)e.immunities[i] < 100f) {
                    current.Add(d);
                }
            }
        }
        if (diseases.Count > 0 && current.Count == 0) {
            diseaseFreeIteration = iteration;
            bonusString += "<<Sanctuary period: " + recoveringPeriod + " years>>" + "\r\n";
        }
        diseases = current;
    }

    ArrayList CreateNewDisease(ArrayList ents) {
        ArrayList entities = new ArrayList(ents);
        //Random - if higher than X -> create new Disease
        int a = Random.Range (0,100);
        float chance = 100; 
        if(diseases.Count > 0) {
            chance = ((((float)ents.Count / (float)MAX_SUPPORT) * (itLastDisease - recoveringPeriod)) / diseases.Count) * 100;
        }
        if ((a <= chance && (itLastDisease > recoveringPeriod && (diseaseFreeIteration + recoveringPeriod) < iteration))) { // X % chance
            createdDiseases++;
            //////// EVOLUTIONIZE HERE :D /////////
            GeneticAlgorithm ga = new GeneticAlgorithm(5, this);
            Gene g = ga.StartAlgorithm();
            bonusString += "<<New Disease: " + g.getPhenotype() + ", Fitness: " + g.mFitness + ">>" + "\r\n";
            Disease d = new Disease(g.mChromosome[0], g.mChromosome[1], Random.Range(0, 10), g.mChromosome[2], null);
            diseases.Add(d);
            itLastDisease = 1;
            ((Entity)entities[Random.Range(0, ents.Count)]).infect(d);
        }
        return entities;
    }

    ArrayList InfectPeople(ArrayList ents) {
        ArrayList entities = new ArrayList(ents);
        if (diseases.Count > 0) { //Can spread diseases, even when immune
            foreach (Disease d in diseases) {
                int victimCounter = 0;
                foreach (Entity e in entities) {
                    if (e.infections.Contains(d))
                        victimCounter++;
                }
                foreach (Entity e in entities) {
                    if (!e.infections.Contains(d)) { // Not infected
                        goto infect;
                    }
                    if (e.infections.Count == 0)
                        goto infect;
                    continue;

                infect:
                    float rng = Random.Range(0f, entities.Count / 10f); // Peeps has 10% chance of encountering others
                    if (rng < victimCounter) {
                        int a = Random.Range(0, 100);
                        if (a <= (int)d.infectionRate) {
                            e.infect(d);
                        }
                    }
                }
            }
        }
        #region Previous attempts of spreading diseases
        // Spreads diseases in a terrible manner (the earlier in the arraylist the first victim is, the more gets infected
        /*bool immunitiesMatter = true;
        foreach (Entity e in entities) {
            //if (!isSimulation) Debug.Log("# of infections in host " + e.infections.Count);
            for (int i = 0; i < e.infections.Count; i++) {
                if ((float)e.immunities[i] < 100 || !immunitiesMatter) { // Not immune
                    Disease d = (Disease)e.infections[i];
                    foreach (Entity victim in entities) {
                        if (!e.Equals(victim) && !victim.infections.Contains(d)) { // It'd be silly to infect yourself
                            int rng = Random.Range(0, 10); // 10% chance of encountering others
                            if (rng == 0) {
                                int a = Random.Range(0, 100);
                                if (a <= (int)d.infectionRate) {//&& !victim.infections.Contains(d)) {
                                    victim.infect(d);
                                }
                            }
                        }
                    }
                }
            }
        }*/
        // Spreads uniformly across population
        /*if (diseases.Count > 0) {
            foreach (Disease d in diseases) {
                foreach (Entity e in entities) {
                    for (int i = 0; i < e.infections.Count; i++) {
                        if (e.infections[i] != d) { // Not infected
                            goto infect;
                        }
                    }
                    if (e.infections.Count == 0)
                        goto infect;
                    break;

                infect:
                    int a = Random.Range(0, 100);
                    if (a <= (int)d.infectionRate) {
                        e.infect(d);
                    }
                }
            }
        }*/
        #endregion

        return entities;
	}
    #endregion

    #region People Updates
    ArrayList UpdatePeople(ArrayList ents) {
        ArrayList entities = new ArrayList(ents);
        int deaths = 0;
		int killed = 0;
		int aged = 0;
        ArrayList deadPeeps = new ArrayList();
		foreach (Entity e in entities) {
            //go through people and take damage from infections
            DamagePeople(e);
            //if entity.hp <= 0 -> Destroy and remove from list.
            if (e.hp <= 0){
				deaths++;
				killed++;
                deadPeeps.Add(e);
			} else if(e.age > CanHaveKidsThresh){ // Basically kills the people who are Unable to reproduce.
				deaths++;
				aged++;
                deadPeeps.Add(e);
            }
            else
            {
                RegeneratePeople(e);
            }
		}
        foreach(Entity e in deadPeeps)
            entities.Remove(e);

        if (!isSimulation)
        {
            dead += deaths;
            //Debug.Log("Not Simulating");
        }
		kills += killed;
		ageDeaths += aged;
        return entities;
    }

    void DamagePeople(Entity e) {
        foreach (Disease d in e.infections) {
            int index = e.infections.IndexOf(d);
            float damage = ((d.lethality * (1 - ((float)e.immunities[index]) / 100))/100) * e.maxHealth;
            if (isSimulation) {
                damageDealt += damage;
			}
            e.hp -= damage;
            if ((float)e.immunities[index] >= 100 || (float)e.immunities[index] + d.resDropRate >= 100) {
                e.immunities[index] = 100f;
            }
            else {
                e.immunities[index] = (float) e.immunities[index] + d.resDropRate;
            }
        }
    }

    public void RegeneratePeople(Entity e) {
        float reg = (e.vitality * (e.hp / e.maxHealth)) * 10;
        e.hp += reg;

        if (e.hp > e.maxHealth)
            e.hp = e.maxHealth;
    }
    #endregion

    public void SimulateNextStep() {
        ents = UpdateInteractions(ents);
        ArrayList newList = new ArrayList();
        foreach (Entity e in ents) {
            if (newList.Count == MAX_SUPPORT)
                break;
            newList.Add(e);
        }
        foreach (Entity e in children) {
            if (newList.Count == MAX_SUPPORT)
                break;
            newList.Add(e);
        }
        children = new ArrayList();
        //ents = new ArrayList ();
        ents = (ArrayList)newList.Clone();
        ents = InfectPeople(ents);
        ents = UpdatePeople(ents);
		iteration++;
    }

    public void NextStep () {
        newBorns = 0;
		kills = 0;
		ageDeaths = 0;
		// Update interactions, update Diseases, update people.
		ArrayList entities = UpdateInteractions (ents);
		ArrayList newList = new ArrayList ();
		foreach (Entity e in entities) {
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
		ents = (ArrayList)newList.Clone ();
        /////END OF INTERACTIONS//////
        
        ents = UpdateDiseases(ents);
		ents = UpdatePeople (ents); // take damage and stuff;

        // String used for providing output
        state = string.Format("{0,8}, {1,14}, {2,12}.", "Year: " + iteration.ToString("D2"), "Villagers: " + ents.Count.ToString("D3"), "Body Count: " + (kills + ageDeaths).ToString("D2"));

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
		if (ents.Count == 0) {
            ////// ENDS THE SIMULATION IF NO PEOPLE ARE ALIVE //////
			Destroy(this);
		}
	}

    public string GetState(bool bonus = false) {
        string output = "";
        if (bonus) {
            output += bonusString;
            bonusString = "";
        }
        output += state;
        return output;
    }

    public string GetStatistics() {
        return "Total Body Count: " + dead + ", Total Diseases encountered: " + createdDiseases + " Average Lifespan: " + Mathf.Ceil(ageTally / dead) + " years.";
    }
}

