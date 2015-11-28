using UnityEngine;
using System.Collections;

public class DrawSimulation : MonoBehaviour {
    VillagePeopleSimulator vls;
    public UnityEngine.UI.Text textbox;
    public UnityEngine.UI.Scrollbar scrollBar;
    bool done;

    // Use this for initialization
    void Start () {
        newVillage();
    }
	
	// Update is called once per frame
	void Update () {
        if(vls.getEntities().Count > 0) {
            vls.NextStep();
            textbox.text += "\n" + vls.GetState();
            scrollBar.value = 0; // keep at buttom
        }
        else if(!done && vls.getEntities().Count == 0) {
            done = true;
            scrollBar.value = 1; // go to top
        }
	}

    public void newVillage() {
        vls = new VillagePeopleSimulator();
        vls.Start();
        textbox.text = "New Village Created!";
        done = false;
    }
}
