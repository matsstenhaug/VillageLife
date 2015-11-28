using UnityEngine;
using System.Collections;

public class DrawSimulation : MonoBehaviour {
    VillagePeopleSimulator vls;

	// Use this for initialization
	void Start () {
        vls = new VillagePeopleSimulator();
        vls.Start();
	}
	
	// Update is called once per frame
	void Update () {
        if(vls.getEntities().Count > 0)
            vls.NextStep();
	}

    public void newVillage() {
        vls = new VillagePeopleSimulator();
        vls.Start();
    }
}
