using System.Collections.Generic;

public class Json
{
    public int level;
    public int[,] whatsThere;
    public struct Vector2 { public float x, y; }

    public int ppX, ppY;

    public Dictionary<string, int> pocket;

    public Vector2[,] whereIsIt;
    public Vector2 initialPos;
    public int[,] whatItem;

    private const int NOOF_ROWS_TILE = 10;
    private const int NOOF_COLUMNS_TILE = 15;


    public Json(int aLevel)
    {
        whatsThere = new int[NOOF_ROWS_TILE, NOOF_COLUMNS_TILE];
        whereIsIt = new Vector2[NOOF_ROWS_TILE, NOOF_COLUMNS_TILE];
        whatItem = new int[NOOF_ROWS_TILE, NOOF_COLUMNS_TILE];
        level = aLevel;
    }

    public Json()
    {
        whatsThere = new int[NOOF_ROWS_TILE, NOOF_COLUMNS_TILE];
        whereIsIt = new Vector2[NOOF_ROWS_TILE, NOOF_COLUMNS_TILE];
        whatItem = new int[NOOF_ROWS_TILE, NOOF_COLUMNS_TILE];
        pocket = new Dictionary<string, int>();
    }
}
