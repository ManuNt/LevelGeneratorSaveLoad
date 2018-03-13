using System.Collections;
using System.Collections.Generic;

public class ObjectPool
{
    private List<Pokeball> m_Pball;
    private List<Starbucksball> m_Sball;
    private List<Grenade> m_Grenade;
    public Inventory<Item> m_Inventory;

    public ObjectPool()
    {
        m_Pball = new List<Pokeball>();
        m_Sball = new List<Starbucksball>();

        if (LevelGenerator.Instance.m_CanPlayerInitPos)
        {
            m_Inventory = new Inventory<Item>();
        }
        
        m_Grenade = new List<Grenade>();


        for (int i = 0; i < 10; i++)
        {
            m_Pball.Add(new Pokeball());
            m_Sball.Add(new Starbucksball());
            m_Grenade.Add(new Grenade());
        }
    }

    public void AddPokeballToInventory()
    {
        foreach (Pokeball ball in m_Pball)
        {
            if (!ball.m_IsInInventory)
            {
                ball.m_IsInInventory = true;
                m_Inventory.AddItem(ball);
                return;
            }
        }
        Pokeball p = new Pokeball();
        p.m_IsInInventory = true;
        m_Pball.Add(p);
        m_Inventory.AddItem(p);
    }

    public void AddAStarbucksballToInventory()
    {
        foreach (Starbucksball ball in m_Sball)
        {
            if (!ball.m_IsInInventory)
            {
                ball.m_IsInInventory = true;
                m_Inventory.AddItem(ball);
                return;
            }
        }
        Starbucksball s = new Starbucksball();
        s.m_IsInInventory = true;
        m_Sball.Add(s);
        m_Inventory.AddItem(s);
    }

    public void AddAGrenadeToInventory()
    {
        foreach (Grenade ball in m_Grenade)
        {
            if (!ball.m_IsInInventory)
            {
                ball.m_IsInInventory = true;
                m_Inventory.AddItem(ball);
                return;
            }
        }
        Grenade g = new Grenade();
        g.m_IsInInventory = true;
        m_Grenade.Add(g);
        m_Inventory.AddItem(g);
    }

}
