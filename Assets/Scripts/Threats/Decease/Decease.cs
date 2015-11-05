using UnityEngine;
using System.Collections;

public class Decease : MonoBehaviour
{



	/*
	 * Types: Virus, Bacteria, Parasite
	 * 
	 * Lethality
	 * Infection Rate
	 * Resistance -> ResistanceDropRate
	 * 
	 * --Effect - Symptom(s) - Transmission--
	 */

	/*
		Have a resistance factor for each person attached??
	 */

	public float personalRes;
	public Entity host;

	int ID;

	public float lethality;
	public float infectionRate;
	public float resistance;
	public float resDropRate;


	public Decease(float leth, float inf, float res, float resDR, int id){
		this.lethality = leth;
		this.infectionRate = inf;
		this.resistance = res;
		this.resDropRate = resDR;
		this.ID = id;
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

