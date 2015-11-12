using UnityEngine;
using System.Collections;

public class Disease {
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

	public float lifespan;
    public float lifetime;

	public float lethality;
	public float infectionRate;
	public float resDropRate;

    public Disease parent = null;


	public Disease(float leth, float inf, float life, float resDR, Disease par) {
        lifespan = 0;
        this.lethality = leth;
		this.infectionRate = inf;
		this.lifetime = life;
		this.resDropRate = resDR;
        this.parent = par;
	}
}

