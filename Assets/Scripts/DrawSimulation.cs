using UnityEngine;
using System.Collections;

public class DrawSimulation : MonoBehaviour {
    VillageLifeSimulator vls;

	// Use this for initialization
	void Start () {
        vls = new VillageLifeSimulator();
        vls.Start();
	}
	
	// Update is called once per frame
	void Update () {
        if(vls.getEntities().Count > 0)
            vls.Update();
	}
}
