using UnityEngine;
using System.Collections;

public class Entity
{
	/*
	 * Sex
	 * Age
	 * Strengths
	 * Immunities/mutations
	 * Stamina
	 * Vitality/health
	 * SexualPreference
	 * Intelligence
	 * Weaknesses
	 * Handicaps/Chronological things
	 * Sexual Potency
	 * 
	 * Resource Consumption
	 */
	
	/*
		Have a variable+threshold for how often it is a male and female?
	 */
	
	public Entity partner = null;
	
	public ArrayList children = new ArrayList ();

	public int maxChildren;
	public bool isPregnant = false;
	public bool hasSexPref = false;
	public bool hasLived = false;
	
	public sex s;
	public sexPref sp;
	public int age;
	public int genderThresh;
	
	public float looks;
	public float strength;
	public float stamina;
	public float hp;
	public float intelligence;
	public float potency;
    public float vitality;
    public float maxHealth;
	
	public float resourceConsumpt;
	
	public int sexPrefThresh; // also used for when he/she is of age (able to have a child)
	
	public ArrayList immunities; // add %values for immunities. Indexes correspond to the infections list
	public ArrayList infections;
	public ArrayList weaknesses;
	public ArrayList handicaps;
	
	
	
	public enum sex {
		male, female
	}
	
	public enum sexPref {
		hetero, bi, homo, a
	}
	
	public Entity(int age, int genderThreshold, float strength, 
	              float stamina, float hp, int sexPrefThresh, float intelligence,
	              ArrayList weaknesses, ArrayList handicaps, float potency, int maxC, float vit) {
		this.age = age;
		this.genderThresh = genderThreshold;
		this.strength = strength;
		this.stamina = stamina;
		this.hp = hp;
		this.sexPrefThresh = sexPrefThresh;
		this.intelligence = intelligence;
		this.weaknesses = weaknesses;
		this.handicaps = handicaps;
		this.potency = potency;
		this.maxChildren = maxC;
		int gend = Random.Range (1,101);
		if (gend >= genderThresh) {
			s = sex.male;
		}else{
			s = sex.female;
		}
        maxHealth = vit * 100;
        this.hp = maxHealth;
		infections = new ArrayList ();
        immunities = new ArrayList();
        this.vitality = vit;

        //Debug.Log ("Gender: " + s);
    }
	
	
	public void setSexPref(){
		// Might need a variable for each preferance, that can be tweeked with Evo-Alg
		
		int a = Random.Range(0,4);
		switch(a){
		case 0:
			sp = sexPref.hetero;
			break;
		case 1:
			sp = sexPref.homo;
			break;
		case 2:
			sp = sexPref.a;
			break;
		case 3:
			sp = sexPref.bi; // 50/50 for wanting opposite sex
			break;
		}
		
		//Debug.Log ("Sex Pref = " + sp);
	}
	
	public void IncreaseAge(){
		age++;
		if(age >= sexPrefThresh && !hasSexPref)
		{
			setSexPref();
			hasSexPref = true;
		}
	}

    public void infect(Disease d) {
        infections.Add(d);
        immunities.Add(0f);
    }

    public Entity Copy() {
        Entity newEntity = new Entity(age, genderThresh, strength, stamina, hp, sexPrefThresh, intelligence, weaknesses, handicaps, potency, maxChildren, vitality);
        newEntity.s = s;
        newEntity.sp = sp;
        foreach (Disease d in infections) {
            newEntity.infections.Add(d);
        }
        foreach (float f in immunities) {
            newEntity.immunities.Add(f);
        }
        return newEntity;
    }
}

