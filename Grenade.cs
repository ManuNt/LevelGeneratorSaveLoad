using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : Item
{
    public Grenade()
    {
        m_Name = "Grenade";
        m_IsInInventory = false;
        m_Type = ItemType.Grenade;
    }
}
