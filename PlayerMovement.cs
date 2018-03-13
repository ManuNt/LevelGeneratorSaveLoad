using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Animator m_Anim;
    private float m_Speed;
    public static int m_PosX, m_PosY;
    public InventoryUI m_Inventory;

    private bool m_IsMoving;

    public static ObjectPool m_Pool;

    public GameObject m_PauseMenePrefab;
    public static bool m_IsGamePaused;

	
	private void Start ()
    {
        m_Anim = GetComponent<Animator>();
        m_Speed = 5f;

        if (LevelGenerator.Instance.m_CanPlayerInitPos)
        {
            m_PosX = 1;
            m_PosY = 1;

            if (SceneManager.GetActiveScene().name.Contains("1"))
            {
                LevelGenerator.Instance.LoadLevel(1);
            }
            else if (SceneManager.GetActiveScene().name.Contains("2"))
            {
                LevelGenerator.Instance.LoadLevel(2);
            }
            else if (SceneManager.GetActiveScene().name.Contains("3"))
            {
                LevelGenerator.Instance.LoadLevel(3);
            }

        }
        else
        {
            LevelGenerator.Instance.LoadSavedLevel();
            m_PosX = LevelGenerator.Instance.GetPos()[0];
            m_PosY = LevelGenerator.Instance.GetPos()[1];

        }

        transform.position = LevelGenerator.Instance.m_WheresIt[m_PosX, m_PosY];


        m_IsMoving = false;

        m_Pool = new ObjectPool();

        m_IsGamePaused = false;
	}
	
	private void Update ()
    {
        if (!m_IsGamePaused)
        {
            if (m_IsMoving)
            {
                transform.position = Vector3.MoveTowards(transform.position, LevelGenerator.Instance.m_WheresIt[m_PosX, m_PosY], m_Speed * Time.deltaTime);
                if (Vector3.Distance(transform.position, (Vector3)LevelGenerator.Instance.m_WheresIt[m_PosX, m_PosY]) < 0.01f)
                {
                    m_IsMoving = false;
                }
            }
            else
            {
                CheckMovementInput();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                PauseGame();
            }
        }
        
        

    }


    private void CheckMovementInput()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            m_Anim.SetBool("IsWalkingLeft", true);
            if (LevelGenerator.Instance.m_WhatsThere[m_PosX, m_PosY - 1] == ETileType.Floor ||
                LevelGenerator.Instance.m_WhatsThere[m_PosX, m_PosY - 1] == ETileType.CollectableFloor)
            {
                m_PosY -= 1;
                m_IsMoving = true;
            }
        }
        else
        {
            m_Anim.SetBool("IsWalkingLeft", false);
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            m_Anim.SetBool("IsWalkingUp", true);
            if (LevelGenerator.Instance.m_WhatsThere[m_PosX - 1, m_PosY] == ETileType.Floor ||
                LevelGenerator.Instance.m_WhatsThere[m_PosX - 1, m_PosY] == ETileType.CollectableFloor)
            {
                m_PosX -= 1;
                m_IsMoving = true;
            }
        }
        else
        {
            m_Anim.SetBool("IsWalkingUp", false);
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            m_Anim.SetBool("IsWalkingRight", true);
            if (LevelGenerator.Instance.m_WhatsThere[m_PosX, m_PosY + 1] == ETileType.Floor ||
                LevelGenerator.Instance.m_WhatsThere[m_PosX, m_PosY + 1] == ETileType.CollectableFloor)
            {
                m_PosY += 1;
                m_IsMoving = true;
            }
        }
        else
        {
            m_Anim.SetBool("IsWalkingRight", false);
        }

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            m_Anim.SetBool("IsWalkingDown", true);
            if (LevelGenerator.Instance.m_WhatsThere[m_PosX + 1, m_PosY] == ETileType.Floor ||
                LevelGenerator.Instance.m_WhatsThere[m_PosX + 1, m_PosY] == ETileType.CollectableFloor)
            {
                m_PosX += 1;
                m_IsMoving = true;
            }
        }
        else
        {
            m_Anim.SetBool("IsWalkingDown", false);
        }

        if (LevelGenerator.Instance.m_WhatsThere[m_PosX, m_PosY] == ETileType.CollectableFloor &&
            Input.GetKeyDown(KeyCode.Space))
        {
            PickUpItem();
        }
    }

    private void PickUpItem()
    {
        char item = ' ';
        if (LevelGenerator.Instance.m_WhatItem[m_PosX, m_PosY] == Item.ItemType.Pokeball)
        {
            m_Pool.AddPokeballToInventory();
            item = 'p';
        }
        else if (LevelGenerator.Instance.m_WhatItem[m_PosX, m_PosY] == Item.ItemType.Starbucksball)
        {
            m_Pool.AddAStarbucksballToInventory();
            item = 's';
        }
        else if (LevelGenerator.Instance.m_WhatItem[m_PosX, m_PosY] == Item.ItemType.Grenade)
        {
            m_Pool.AddAGrenadeToInventory();
            item = 'g';
        }

           StartCoroutine(m_Inventory.AknowledgePickup(item));

        LevelGenerator.Instance.ChangeTileToFloor(m_PosX, m_PosY);
        

    }

    private void PauseGame()
    {
        m_IsGamePaused = true;
        GameObject pauseMenu = Instantiate(m_PauseMenePrefab);
    }
}
