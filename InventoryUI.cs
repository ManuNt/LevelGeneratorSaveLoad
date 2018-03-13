using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    private Dictionary<string, int> m_Items;

    public GameObject m_InventoryPanel, m_PopupPanel;
    public Image m_FirstItem, m_SecondItem, m_ThirdItem;
    public Text m_FirstTxt, m_SecondTxt, m_ThirdTxt, m_PopupTxt;
    public Sprite m_Pokeball, m_Starbucksball, m_Grenade;

    private float m_PopupTime;
    

    private void Start ()
    {
        m_Items = new Dictionary<string, int>();
        m_InventoryPanel.SetActive(false);
        m_PopupPanel.SetActive(false);
        m_PopupTime = 4f;

    }
	
	private void Update ()
    {
        CheckInput();

    }

    private void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.I) && !m_InventoryPanel.activeInHierarchy)
        {
            ShowItems();
        }
    }

    private void ShowItems()
    {
        m_Items = PlayerMovement.m_Pool.m_Inventory.ShowItemms();

        m_InventoryPanel.SetActive(true);

        int counter = 0;

        foreach (KeyValuePair<string, int> entry in m_Items)
        {

            // do something with entry.Value or entry.Key
            // If there is a Pokeball
            if (entry.Key == "Pokeball" && counter == 0)
            {
                m_FirstItem.sprite = m_Pokeball;
                m_FirstTxt.text = entry.Value.ToString() + "x\t" + entry.Key;
                var color = m_FirstItem.color;
                color.a = 255;
                m_FirstItem.color = color;
                counter++;
            }
            else if (entry.Key == "Pokeball" && counter == 1)
            {
                m_SecondItem.sprite = m_Pokeball;
                m_SecondTxt.text = entry.Value.ToString() + "x\t" + entry.Key;
                var color = m_SecondItem.color;
                color.a = 255;
                m_SecondItem.color = color;
                counter++;
            }
            else if (entry.Key == "Pokeball" && counter == 2)
            {
                m_ThirdItem.sprite = m_Pokeball;
                m_ThirdTxt.text = entry.Value.ToString() + "x\t" + entry.Key;
                var color = m_ThirdItem.color;
                color.a = 255;
                m_ThirdItem.color = color;
                counter++;
            }

            // If there is a Starbucksball
            if (entry.Key == "Starbucksball" && counter == 0)
            {
                m_FirstItem.sprite = m_Starbucksball;
                m_FirstTxt.text = entry.Value.ToString() + "x\t" + entry.Key;
                var color = m_FirstItem.color;
                color.a = 255;
                m_FirstItem.color = color;
                counter++;
            }
            else if (entry.Key == "Starbucksball" && counter == 1)
            {
                m_SecondItem.sprite = m_Starbucksball;
                m_SecondTxt.text = entry.Value.ToString() + "x\t" + entry.Key;
                var color = m_SecondItem.color;
                color.a = 255;
                m_SecondItem.color = color;
                counter++;
            }
            else if (entry.Key == "Starbucksball" && counter == 2)
            {
                m_ThirdItem.sprite = m_Starbucksball;
                m_ThirdTxt.text = entry.Value.ToString() + "x\t" + entry.Key;
                var color = m_ThirdItem.color;
                color.a = 255;
                m_ThirdItem.color = color;
                counter++;
            }

            // If there is a Grenade
            if (entry.Key == "Grenade" && counter == 0)
            {
                m_FirstItem.sprite = m_Grenade;
                m_FirstTxt.text = entry.Value.ToString() + "x\t" + entry.Key;
                var color = m_FirstItem.color;
                color.a = 255;
                m_FirstItem.color = color;
                counter++;
            }
            else if (entry.Key == "Grenade" && counter == 1)
            {
                m_SecondItem.sprite = m_Grenade;
                m_SecondTxt.text = entry.Value.ToString() + "x\t" + entry.Key;
                var color = m_SecondItem.color;
                color.a = 255;
                m_SecondItem.color = color;
                counter++;
            }
            else if (entry.Key == "Grenade" && counter == 2)
            {
                m_ThirdItem.sprite = m_Grenade;
                m_ThirdTxt.text = entry.Value.ToString() + "x\t" + entry.Key;
                var color = m_ThirdItem.color;
                color.a = 255;
                m_ThirdItem.color = color;
                counter++;
            }

        }

        if (counter == 0)
        {
            var color = m_FirstItem.color;
            color.a = 0;
            m_FirstItem.color = color;
            color = m_SecondItem.color;
            color.a = 0;
            m_SecondItem.color = color;
            color = m_ThirdItem.color;
            color.a = 0;
            m_ThirdItem.color = color;
        }
        else if (counter == 1)
        {
            var color = m_SecondItem.color;
            color.a = 0;
            m_SecondItem.color = color;
            color = m_ThirdItem.color;
            color.a = 0;
            m_ThirdItem.color = color;
        }
        else if (counter == 2)
        {
            var color = m_ThirdItem.color;
            color.a = 0;
            m_ThirdItem.color = color;
        }
    }

    public void CloseInventory()
    {
        m_InventoryPanel.SetActive(false);
    }

    public IEnumerator AknowledgePickup(char aItem)
    {
        string item = "";
        m_PopupPanel.SetActive(true);
        
        if (aItem == 'p')
        {
            item = "Pokeball";
        }
        else if (aItem == 's')
        {
            item = "Starbucksball";
        }
        else if (aItem == 'g')
        {
            item = "Grenade";
        }

        m_PopupTxt.text = "A new " + item + " has been added to your inventory!";

        yield return new WaitForSeconds(m_PopupTime);
        m_PopupPanel.SetActive(false);
    }
}
