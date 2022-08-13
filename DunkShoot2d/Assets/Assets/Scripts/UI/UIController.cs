using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _title;
    

    public void UIMainMenuOff()
    {
        if (UIDatabase.MainMenuCanvasActivity)
        { 
            _title.SetActive(false);

            UIDatabase.MainMenuCanvasActivity = false;
        }
    }
}
