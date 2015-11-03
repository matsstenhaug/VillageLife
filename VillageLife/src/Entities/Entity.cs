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

	sex s;
	int age;
	float sexThresh;
	float strength;
	ArrayList<Decease> immunities;


	enum sex {
		male, female
	}

	public Entity(int sexThreshold, float strength, ArrayList<Decease> immunities, 
	              float stamina, float hp, int sexPrefThresh, float intelligence,
	              ArrayList<Decease> weaknesses, ArrayList<Decease> handicap, float potency){

	}

	// Use this for initialization
	void Start ()
	{
		Random r = new Random ();
		int a = r.Next (2);

		if (a == 1) {
			s = sex.male;
		}else{
			s = sex.female;
		}


	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

