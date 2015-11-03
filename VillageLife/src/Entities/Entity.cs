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

	Random r = new Random ();

	sex s;
	sexPref sp = null;
	int age;
	int genderThresh;

	float strength;
	float stamina;
	float hp;
	float intelligence;
	float potency;

	float resourceConsumpt;

	int sexPrefThresh;
	ArrayList<Decease> immunities;
	ArrayList<Decease> weaknesses;
	ArrayList<Decease> handicaps;



	enum sex {
		male, female
	}

	enum sexPref {
		hetero, bi, homo, a
	}

	public Entity(int genderThreshold, float strength, ArrayList<Decease> immunities, 
	              float stamina, float hp, int sexPrefThresh, float intelligence,
	              ArrayList<Decease> weaknesses, ArrayList<Decease> handicaps, float potency){
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
		int gend = r.Next (100) + 1;
		if (gend >= genderThresh) {
			s = sex.male;
		}else{
			s = sex.female;
		}

		

	}
	
	// Update is called once per frame
	void Update ()
	{
		if(age >= sexPrefThresh && sexPref == null)
		{
			// Might need a variable for each preferance, that can be tweeked with Evo-Alg
			int a = r.Next(4);
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
				sp = sexPref.bi;
				break;
			}
		}
		// set the sexual preference at a certain age.

		// update the age at end?
		age ++;
	}
}

