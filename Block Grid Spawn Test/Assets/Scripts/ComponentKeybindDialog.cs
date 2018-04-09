using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ComponentKeybindDialog : MonoBehaviour 
{
    KeybindableComponent keybindableComponent;
    
    public void OpenDialog(KeybindableComponent keybindableComponent)
    {
        this.keybindableComponent = keybindableComponent;
        gameObject.SetActive(true);

        transform.Find("Keybind").GetComponent<Text>().text = keybindableComponent.keyCode.ToString();

    }
    
	void Update () 
	{
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameObject.SetActive(false);
        }

        foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
        {
            if (keyCode != KeyCode.Mouse0 && keyCode != KeyCode.Mouse1 && keyCode != KeyCode.Mouse2 && Input.GetKeyUp(keyCode))
            {
                keybindableComponent.keyCode = keyCode;
                gameObject.SetActive(false);
                return;
            }
        }
        
	}
}
