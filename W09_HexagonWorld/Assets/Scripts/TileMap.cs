using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    public bool CheckAroundTile(int targetX, int targetY)
    {
        int x, y;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (CheckY(targetY, i, j))
                {
                    continue;
                }
                y = targetY + i; x = targetX + j;
                if (!CheckMapSizeIndex(x, y))
                {
                    continue;
                }
                HexTileMapGenerator.TileInfo tempTile = HexTileMapGenerator.MapLists[y][x];
                if (tempTile.item == null)
                    return false;
            }
        }
        return true;
    }

    public bool CheckAroundThreeTopTile(int targetX, int targetY)
    {
        int x, y;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (CheckY(targetY, i, j))
                {
                    continue;
                }
                y = targetY + i; x = targetX + j;
                if (!CheckMapSizeIndex(x, y))
                {
                    continue;
                }
                HexTileMapGenerator.TileInfo tempTile = HexTileMapGenerator.MapLists[y][x];
                if (tempTile.item == null)
                    return false;
            }
        }
        return true;
    }

    bool CheckY(int targetY, int i, int j)
    {
        if (targetY % 2 == 1)
        {
            return CheckYOdd(i, j);
        }
        else
        {
            return CheckYEven(i, j);
        }
    }
    bool CheckYEven(int i, int j)
    {
        if ((i == 1 && j == -1) || (i == 0 && j == 0) || (i == 1 && j == 1))
        {
            return true;
        }
        return false;
    }
    bool CheckYOdd(int i, int j)
    {
        if ((i == -1 && j == -1) || (i == 0 && j == 0) || (i == -1 && j == 1))
        {
            return true;
        }
        return false;
    }

    public bool CheckMapSizeIndex(int x, int y)
    {
        if (x >= 0 && x < HexTileMapGenerator.mapWidth && y >= 0 && y < HexTileMapGenerator.mapHeight)
        {
            return true;
        }
        return false;
    }
}
