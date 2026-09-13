using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
public class GunUI : MonoBehaviour
{
    public static GunUI instance;
    public Image weaponImage;
    public TextMeshProUGUI weaponNameText;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
