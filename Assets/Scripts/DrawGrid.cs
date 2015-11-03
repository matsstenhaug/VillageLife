using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DrawGrid : MonoBehaviour {

    #region Grid variables
    public int rows = 4;
    public int columns = 4;
    public Button prefab;
    private Button button;
    #endregion

    // Use this for initialization
    void Start () {
        #region Make grid
        RectTransform myRect = GetComponent<RectTransform>();
        GridLayoutGroup grid = this.GetComponent<GridLayoutGroup>();

        float objectHeight = (myRect.rect.height - (grid.padding.top - grid.padding.bottom)) / (float)rows;
        float objectWidth = (myRect.rect.width - (grid.padding.left - grid.padding.right)) / (float)columns;

        grid.cellSize = new Vector2(objectWidth, objectHeight);
        
        // Fill out the grid
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < columns; j++) {
                button = (Button)Instantiate(prefab);
                button.gameObject.SetActive(true);
                button.transform.SetParent(transform, false);
            }
        }
        #endregion
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
