using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;

public class DrawSimulation : MonoBehaviour {
    VillagePeopleSimulator vls;
    public UnityEngine.UI.Text textbox;
    public UnityEngine.UI.Scrollbar scrollBar;

    public int initialPopulation = 50;
    public int maximumPopulation = 500;
    public int recoveringPeriod = 10;
    public bool bonusInfo = false;

    bool done;
    private float nextActionTime = 0.0f;
    public float period = 0.5f;
    StringBuilder output = new StringBuilder("");
    int textboxBufferSize = 2000;

    // Use this for initialization
    void Start () {
        newVillage();
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time > nextActionTime) {
            nextActionTime += period;
            if (vls.getEntities().Count > 0) {
                vls.NextStep();
                output.Append("\r\n" + vls.GetState(bonusInfo));
            } else if (!done && vls.getEntities().Count == 0) {
                done = true;
                output.Append("\r\n" + vls.GetStatistics());
                //scrollBar.value = 1.1f; // go to top
            }
            textbox.text = textboxBufferSize > output.Length ? output.ToString(0, output.Length) : output.ToString(output.Length - textboxBufferSize, textboxBufferSize);
            scrollBar.value = -0.1f; // keep at buttom
        }
	}

    public void newVillage() {
        vls = new VillagePeopleSimulator();
        vls.Start(initialPopulation, maximumPopulation, recoveringPeriod);
        output = new StringBuilder("New Village Created!");
        textbox.text = output.ToString();
        done = false;
    }

    public static string SaveTextName() {
        string filePath = string.Format("{0}/text", Directory.GetParent(Application.dataPath));
        Directory.CreateDirectory(filePath);
        return string.Format("{0}/log_{1}.txt", filePath,
                             System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    public void saveTxt() {
        StreamWriter writer = new StreamWriter(SaveTextName());
        writer.Write(output.ToString());
        writer.Close();
    }
}
