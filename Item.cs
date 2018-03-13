using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public string m_Name;
    public bool m_IsInInventory;

    public enum ItemType
    {
        Pokeball,
        Starbucksball,
        Grenade,
    }

    public ItemType m_Type;
	
}
