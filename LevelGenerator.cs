using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    public GameObject m_FloorPrefab, m_Wall1Prefab, m_Wall2Prefab, m_Wall3Prefab;
    public GameObject m_PokeballPrefab, m_StarbPrefab, m_GrenadePrefab;
    public ETileType[,] m_WhatsThere;
    public Vector2[,] m_WheresIt;
    public Item.ItemType[,] m_WhatItem;
    private GameObject[,] m_Items;

    private const int NOOF_ROWS_TILE = 10;
    private const int NOOF_COLUMNS_TILE = 15;
    private const float TILE_SIZE = 100;
    private const int PIXEL_PER_UNIT = 100;

    public bool m_CanPlayerInitPos;
    
    private Vector2 m_InitialPosition;

    private int ppX, ppY;

    private static LevelGenerator m_Instance;
    public static LevelGenerator Instance { get { return m_Instance; } }

    private void Awake()
    {

        if (m_Instance == null)
        {
            m_Instance = this;
        }

        DontDestroyOnLoad(gameObject);
        // Generating 3 random levels and storing them in a json file that will be read on level selected
        for (int i = 1; i <= 3; i++)
        {
            CreateJsonLevel(i);
        }

        m_CanPlayerInitPos = true;
    }


    public void ChangeTileToFloor(int x, int y)
    {
        GameObject floor = Instantiate(m_FloorPrefab);
        m_WhatsThere[x, y] = ETileType.Floor;
        floor.transform.position = m_WheresIt[x, y];
        Destroy(m_Items[x, y]);
    }

    private void GenerateLevel()
    {

        m_WhatsThere = new ETileType[NOOF_ROWS_TILE, NOOF_COLUMNS_TILE];
        m_WheresIt = new Vector2[NOOF_ROWS_TILE, NOOF_COLUMNS_TILE];
        m_WhatItem = new Item.ItemType[NOOF_ROWS_TILE, NOOF_COLUMNS_TILE];
        m_InitialPosition = new Vector2((-(Screen.width / 2) + (TILE_SIZE / 2)) / PIXEL_PER_UNIT,
                                        (((Screen.height / 2) - (TILE_SIZE / 2)) / PIXEL_PER_UNIT));

        for (int i = 0; i < NOOF_ROWS_TILE; i++)
        {
            for (int j = 0; j < NOOF_COLUMNS_TILE; j++)
            {
                Vector2 spawnPos = m_InitialPosition + new Vector2((TILE_SIZE * j) / PIXEL_PER_UNIT,
                                                                    -(TILE_SIZE * i) / PIXEL_PER_UNIT);
                if (i == 0 || j == 0 || i == NOOF_ROWS_TILE - 1 || j == NOOF_COLUMNS_TILE - 1)
                {
                    m_WhatsThere[i, j] = ETileType.Wall;
                    m_WheresIt[i, j] = spawnPos;
                }
                else if (i == 1 && j == 1 || i == 1 && j == 2 || i == 2 && j == 1 || i == 2 && j == 2)
                {
                    m_WhatsThere[i, j] = ETileType.Floor;
                    m_WheresIt[i, j] = spawnPos;
                }
                else
                {
                    int rand = Random.Range(0, 101);
                    if (rand <= 70)
                    {
                        int itemRand = Random.Range(0, 101);

                        if (itemRand > 10)
                        {
                            m_WhatsThere[i, j] = ETileType.Floor;
                            m_WheresIt[i, j] = spawnPos;
                        }
                        else
                        {
                            int whatRand = Random.Range(0, 101);

                            if (whatRand < 33)
                            {
                                m_WhatItem[i, j] = Item.ItemType.Pokeball;
                            }
                            else if (whatRand < 66)
                            {
                                m_WhatItem[i, j] = Item.ItemType.Starbucksball;
                            }
                            else
                            {
                                m_WhatItem[i, j] = Item.ItemType.Grenade;
                            }

                            m_WhatsThere[i, j] = ETileType.CollectableFloor;
                            m_WheresIt[i, j] = spawnPos;
                        }

                    }
                    else if (rand > 70 && rand <= 85)
                    {
                        m_WhatsThere[i, j] = ETileType.Wall;
                        m_WheresIt[i, j] = spawnPos;
                    }
                    else
                    {
                        m_WhatsThere[i, j] = ETileType.Wall;
                        m_WheresIt[i, j] = spawnPos;
                    }
                }
            }
        }


    }

    /*
        Wall                / Pokeball          ==> 0
        Floor               / Starbucksball     ==> 1
        CollectableFloor    / Grenade           ==> 2
    */

    private void CreateJsonLevel(int aLvl)
    {
        GenerateLevel();

        Json sauvegarde = new Json(aLvl);

        sauvegarde.initialPos.x = m_InitialPosition.x;
        sauvegarde.initialPos.y = m_InitialPosition.y;

        for (int i = 0; i < NOOF_ROWS_TILE; i++)
        {
            for (int j = 0; j < NOOF_COLUMNS_TILE; j++)
            {
                // Saving the type of tiles, there position

                if (m_WhatsThere[i, j] == ETileType.Wall)
                {
                    sauvegarde.whatsThere[i, j] = 0;

                }
                else if (m_WhatsThere[i,j] == ETileType.Floor)
                {
                    sauvegarde.whatsThere[i, j] = 1;
                }
                else if (m_WhatsThere[i, j] == ETileType.CollectableFloor) // if it's an item, saving the type of item.
                {
                    sauvegarde.whatsThere[i, j] = 2;

                    if (m_WhatItem[i,j] == Item.ItemType.Pokeball)
                    {
                        sauvegarde.whatItem[i, j] = 0;
                    }
                    else if (m_WhatItem[i, j] == Item.ItemType.Starbucksball)
                    {
                        sauvegarde.whatItem[i, j] = 1;
                    }
                    else if (m_WhatItem[i, j] == Item.ItemType.Grenade)
                    {
                        sauvegarde.whatItem[i, j] = 2;
                    }
                }

                sauvegarde.whereIsIt[i, j].x = m_WheresIt[i, j].x;
                sauvegarde.whereIsIt[i, j].y = m_WheresIt[i, j].y;
            }
        }


        // Serialize the instance of the class
        string saving = JsonConvert.SerializeObject(sauvegarde, Formatting.Indented);
        File.WriteAllText("level" + aLvl + ".json", saving);
    }


    public void LoadLevel(int aLvl)
    {
        string savedContent = File.ReadAllText("level" + aLvl + ".json");
        Json levelInfo = JsonConvert.DeserializeObject<Json>(savedContent);
        m_InitialPosition = new Vector2(levelInfo.initialPos.x, levelInfo.initialPos.y);
        m_Items = new GameObject[NOOF_ROWS_TILE, NOOF_COLUMNS_TILE];

        for (int i = 0; i < NOOF_ROWS_TILE; i++)
        {
            for (int j = 0; j < NOOF_COLUMNS_TILE; j++)
            {
                Vector2 spawnPos = m_InitialPosition + new Vector2((TILE_SIZE * j) / PIXEL_PER_UNIT,
                                                                    -(TILE_SIZE * i) / PIXEL_PER_UNIT);
                    
                if (levelInfo.whatsThere[i, j] == 0)                // Wall
                {
                    GameObject wall = Instantiate(m_Wall1Prefab);
                    wall.transform.position = spawnPos;
                    m_WhatsThere[i, j] = ETileType.Wall;
                    m_WheresIt[i, j] = spawnPos;
                }
                else if (levelInfo.whatsThere[i, j] == 1)           // Floor
                {
                    GameObject floor = Instantiate(m_FloorPrefab);
                    floor.transform.position = spawnPos;
                    m_WhatsThere[i, j] = ETileType.Floor;
                    m_WheresIt[i, j] = spawnPos;
                }
                else if (levelInfo.whatsThere[i, j] == 2)           // Item
                {
                    GameObject collectable;

                    if (levelInfo.whatItem[i,j] == 0)           // Pokeball
                    {
                        collectable = Instantiate(m_PokeballPrefab);
                        m_WhatItem[i, j] = Item.ItemType.Pokeball;
                    }
                    else if (levelInfo.whatItem[i, j] == 1)     // Starbucksball
                    {
                        collectable = Instantiate(m_StarbPrefab);
                        m_WhatItem[i, j] = Item.ItemType.Starbucksball;
                    }
                    else                                        // Grenade
                    {
                        collectable = Instantiate(m_GrenadePrefab);
                        m_WhatItem[i, j] = Item.ItemType.Grenade;
                    }

                    collectable.transform.position = spawnPos;
                    m_WhatsThere[i, j] = ETileType.CollectableFloor;
                    m_WheresIt[i, j] = spawnPos;
                    m_Items[i, j] = collectable;
                }
            }
        }
    }

    public void LoadSavedLevel()
    {
        m_CanPlayerInitPos = false;

        string savedContent = File.ReadAllText("save.json");
        Json levelInfo = JsonConvert.DeserializeObject<Json>(savedContent);
        GameObject player = GameObject.FindGameObjectWithTag("Player");



        m_InitialPosition = new Vector2(levelInfo.initialPos.x, levelInfo.initialPos.y);
        m_Items = new GameObject[NOOF_ROWS_TILE, NOOF_COLUMNS_TILE];
        ppX = levelInfo.ppX;
        ppY = levelInfo.ppY;


        for (int i = 0; i < NOOF_ROWS_TILE; i++)
        {
            for (int j = 0; j < NOOF_COLUMNS_TILE; j++)
            {
                Vector2 spawnPos = m_InitialPosition + new Vector2((TILE_SIZE * j) / PIXEL_PER_UNIT,
                                                                    -(TILE_SIZE * i) / PIXEL_PER_UNIT);

                if (levelInfo.whatsThere[i, j] == 0)                // Wall
                {
                    GameObject wall = Instantiate(m_Wall1Prefab);
                    wall.transform.position = spawnPos;
                    m_WhatsThere[i, j] = ETileType.Wall;
                    m_WheresIt[i, j] = spawnPos;
                }
                else if (levelInfo.whatsThere[i, j] == 1)           // Floor
                {
                    GameObject floor = Instantiate(m_FloorPrefab);
                    floor.transform.position = spawnPos;
                    m_WhatsThere[i, j] = ETileType.Floor;
                    m_WheresIt[i, j] = spawnPos;
                }
                else if (levelInfo.whatsThere[i, j] == 2)           // Item
                {
                    GameObject collectable;

                    if (levelInfo.whatItem[i, j] == 0)           // Pokeball
                    {
                        collectable = Instantiate(m_PokeballPrefab);
                        m_WhatItem[i, j] = Item.ItemType.Pokeball;
                    }
                    else if (levelInfo.whatItem[i, j] == 1)     // Starbucksball
                    {
                        collectable = Instantiate(m_StarbPrefab);
                        m_WhatItem[i, j] = Item.ItemType.Starbucksball;
                    }
                    else                                        // Grenade
                    {
                        collectable = Instantiate(m_GrenadePrefab);
                        m_WhatItem[i, j] = Item.ItemType.Grenade;
                    }

                    collectable.transform.position = spawnPos;
                    m_WhatsThere[i, j] = ETileType.CollectableFloor;
                    m_WheresIt[i, j] = spawnPos;
                    m_Items[i, j] = collectable;
                }
            }
        }

        Inventory<Item>.m_Pocket = levelInfo.pocket;


        m_CanPlayerInitPos = true; // Re-initializing in case

    }

    public void SaveGame()
    {
        // Initializing
        Json json = new Json();
        json.initialPos.x = 1;
        json.initialPos.y = 1;

        // Player position
        json.ppX = PlayerMovement.m_PosX;
        json.ppY = PlayerMovement.m_PosY;

        // What tyle is there on the map
        for (int i = 0; i < NOOF_ROWS_TILE; ++i)
        {
            for (int j = 0; j < NOOF_COLUMNS_TILE; ++j)
            {
                if (m_WhatsThere[i, j] == ETileType.Wall)
                {
                    json.whatsThere[i, j] = 0;
                }
                else if (m_WhatsThere[i, j] == ETileType.Floor)
                {
                    json.whatsThere[i, j] = 1;
                }
                else if (m_WhatsThere[i, j] == ETileType.CollectableFloor)
                {
                    json.whatsThere[i, j] = 2;

                    if (m_WhatItem[i, j] == Item.ItemType.Pokeball)
                    {
                        json.whatItem[i, j] = 0;
                    }
                    else if (m_WhatItem[i, j] == Item.ItemType.Starbucksball)
                    {
                        json.whatItem[i, j] = 1;
                    }
                    else if (m_WhatItem[i, j] == Item.ItemType.Grenade)
                    {
                        json.whatItem[i, j] = 2;
                    }
                }

                json.whereIsIt[i, j].x = m_WheresIt[i, j].x;
                json.whereIsIt[i, j].y = m_WheresIt[i, j].y;
            }
        }

        json.pocket = Inventory<Item>.GetPocket();

        string savedStuff = JsonConvert.SerializeObject(json, Formatting.Indented);
        File.WriteAllText("save.json", savedStuff);
    }


    public bool IsThereSavedGame()
    {
        if(File.Exists("save.json"))
        {
            return true;
        }

        return false;
    }

    public int[] GetPos()
    {
        int[] pos = new int[2] { ppX, ppY };
        return pos;
    }
	
}
