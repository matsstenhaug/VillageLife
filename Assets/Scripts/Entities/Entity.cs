using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour
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

	public float resourceConsumpt;

	public int sexPrefThresh; // also used for when he/she is of age (able to have a child)

	public ArrayList immunities;
	public ArrayList weaknesses;
	public ArrayList handicaps;



	public enum sex {
		male, female
	}

	public enum sexPref {
		hetero, bi, homo, a
	}

	public Entity(int genderThreshold, float strength, ArrayList immunities, 
	              float stamina, float hp, int sexPrefThresh, float intelligence,
	              ArrayList weaknesses, ArrayList handicaps, float potency){
		this.age = 0;
		this.genderThresh = genderThreshold;
		this.strength = strength;
		this.immunities = immunities;
		this.stamina = stamina;
		this.hp = hp;
		this.sexPrefThresh = sexPrefThresh;
		this.intelligence = intelligence;
		this.weaknesses = weaknesses;
		this.handicaps = handicaps;
		this.potency = potency;

	}

	// Use this for initialization
	void Start ()
	{
		// setting the gender of the entity
		int gend = Random.Range (1,101);
		if (gend >= genderThresh) {
			s = sex.male;
		}else{
			s = sex.female;
		}

		

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
	}

	public void IncreaseAge(){
		age++;
		if(age >= sexPrefThresh && !hasSexPref)
		{
			setSexPref();
		}
	}

	public void findPartner(){
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		findPartner (); // create in main class, so that it goes through all the entities.

	}
}
