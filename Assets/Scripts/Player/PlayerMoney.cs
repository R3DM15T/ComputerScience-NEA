using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMoney : MonoBehaviour
{
    public static PlayerMoney instance;
    [SerializeField] public int currentMoney;
    public TextMeshProUGUI moneyText;


    private void Awake()
    {
        instance = this;
        currentMoney = PlayerPrefs.GetInt("prefMoney", 0);
    }

    private void Start()
    {
        UpdateMoneyText();
    }


    public void AddMoney(int moneyAmountToAdd)
    {
        currentMoney += moneyAmountToAdd;
        SaveMoney();
        UpdateMoneyText();
    }

    public bool TryRemoveMoney(int moneyToRemove)
    {
        if (currentMoney >= moneyToRemove)
        {
            currentMoney -= moneyToRemove;
            SaveMoney();
            UpdateMoneyText();
            return true;
        }

        return false;
    }

    private void SaveMoney()
    {
        PlayerPrefs.SetInt("prefMoney", currentMoney);
        PlayerPrefs.Save();
    }

    private void UpdateMoneyText()
    {
        if (moneyText != null)
        {
            moneyText.text = currentMoney.ToString();
        }
    }

    public void ChangeMoneyTextReference(TextMeshProUGUI newTextReference)
    {
        moneyText = newTextReference;
        UpdateMoneyText();
    }

}

