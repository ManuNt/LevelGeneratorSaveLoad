using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory<Type> where Type : Item
{
    public static Dictionary<string, int> m_Pocket;

    public Inventory()
    {
        m_Pocket = new Dictionary<string, int>();
    }


    public void AddItem(Type aItem)
    {
        
        if (m_Pocket.ContainsKey(aItem.m_Name))
        {
            m_Pocket[aItem.m_Name] += 1;
        }
        else
        {
            m_Pocket.Add(aItem.m_Name, 1);
        }
    }




    public Dictionary<string, int> ShowItemms()
    {
        return m_Pocket;
    }

    public void SetPocket(Dictionary<string, int> aDic)
    {
        m_Pocket = aDic;
    }

    public static Dictionary<string, int> GetPocket() { return m_Pocket; }
}
