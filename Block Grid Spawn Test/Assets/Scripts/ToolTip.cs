using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ToolTip : MonoBehaviour 
{
	Item item;
	string data;
	GameObject toolTip;

	public void Start()
	{
		toolTip = GameObject.Find ("Tool Tip Window");

		toolTip.SetActive (false);
	}

	void Update()
	{
		if(toolTip.activeSelf)
		{
			toolTip.transform.position = Input.mousePosition;
		}
	
	}

	public void Activate(Item item)
	{
		this.item = item;
		ConstructDataString ();
		toolTip.SetActive (true);

	}

	public void Deactivate()
	{
		toolTip.SetActive (false);
	}

	public void ConstructDataString()
	{
		data = "<color=#000000><b>" + item.Title + "</b></color>\n\n" + item.Description + "\nPower : " + item.Power;
		toolTip.transform.GetChild (0).GetComponent<Text> ().text = data;
	}

}
