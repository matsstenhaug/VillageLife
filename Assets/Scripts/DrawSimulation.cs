using UnityEngine;
using System.Collections;
using System.IO;

public class DrawSimulation : MonoBehaviour {
    VillagePeopleSimulator vls;
    public UnityEngine.UI.Text textbox;
    public UnityEngine.UI.Scrollbar scrollBar;

    public int initialPopulation = 50;
    public int maximumPopulation = 500;
    public int recoveringPeriod = 10;
    public bool bonusInfo = false;

    bool done;

    // Use this for initialization
    void Start () {
        newVillage();
    }
	
	// Update is called once per frame
	void Update () {
        if(vls.getEntities().Count > 0) {
            vls.NextStep();
            textbox.text += "\r\n" + vls.GetState(bonusInfo);
            scrollBar.value = 0; // keep at buttom
        }
        else if(!done && vls.getEntities().Count == 0) {
            done = true;
            textbox.text += "\r\n" + vls.GetStatistics();
            scrollBar.value = 1; // go to top
        }
	}

    public void newVillage() {
        vls = new VillagePeopleSimulator();
        vls.Start(initialPopulation, maximumPopulation, recoveringPeriod);
        textbox.text = "New Village Created!";
        done = false;
    }

    public static string SaveTextName() {
        string filePath = string.Format("{0}/text", System.IO.Directory.GetParent(Application.dataPath));
        System.IO.Directory.CreateDirectory(filePath);
        return string.Format("{0}/log_{1}.txt", filePath,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    public void saveTxt() {
        StreamWriter writer = new StreamWriter(SaveTextName());
        writer.Write(textbox.text.ToString());
        writer.Close();
    }
}
