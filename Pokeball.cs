using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokeball : Item
{
    public Pokeball()
    {
        m_Name = "Pokeball";
        m_IsInInventory = false;
        m_Type = ItemType.Pokeball;
    }
}