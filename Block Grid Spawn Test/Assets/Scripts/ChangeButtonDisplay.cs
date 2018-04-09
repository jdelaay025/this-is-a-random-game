using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChangeButtonDisplay : MonoBehaviour
{
    #region Global Variable Declaration

    [SerializeField]
    Sprite sprite1;
    [SerializeField]
    Sprite sprite2;
    [SerializeField]
    string message1;
    [SerializeField]
    string message2;

    [SerializeField]
    TextMeshProUGUI text;
    [SerializeField]
    Image image;

    bool first = true;

    #endregion

    void Awake()
    {
        first = true;
    }

    void Start()
    {

    }

    public void UpdateDisplay()
    {
        if (first)
        {
            if (image != null)
            {
                image.sprite = sprite2;
            }

            if (text != null)
            {
                text.text = message2;
            }

            first = false;
        }
        else
        {
            if (image != null)
            {
                image.sprite = sprite1;
            }

            if (text != null)
            {
                text.text = message1;
            }

            first = true;
        }
    }

    public void SetDisplayToSettings()
    {
        if (image != null)
        {
            image.sprite = sprite1;
        }

        if (text != null)
        {
            text.text = message1;
        }

        first = true;
    }

    public void SetDisplayToGamePlay()
    {
        if (image != null)
        {
            image.sprite = sprite2;
        }

        if (text != null)
        {
            text.text = message2;
        }

        first = false;
    }

}
