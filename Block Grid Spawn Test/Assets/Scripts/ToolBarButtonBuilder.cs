using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToolBarButtonBuilder : MonoBehaviour 
{
    public GameObject buildButtonPrefab;
    public GameObject[] shipPartPrefabs;

	void Start () 
	{
        MouseManager mouseManager = GameObject.FindObjectOfType<MouseManager>();

        for (int i = 0; i < shipPartPrefabs.Length; i++)
        {
            GameObject shipPart = shipPartPrefabs[i];
            
            GameObject buttonGameObject = Instantiate(buildButtonPrefab);
            buttonGameObject.transform.parent = this.transform;
            Text buttonLabel = buttonGameObject.GetComponentInChildren<Text>();
            buttonLabel.text = shipPart.name;

            Button theButton = buttonGameObject.GetComponent<Button>();           

            theButton.onClick.AddListener(() => { mouseManager.prefabToSpawn = shipPart; });
        }
	}

	void Update () 
	{
	
	}
}
