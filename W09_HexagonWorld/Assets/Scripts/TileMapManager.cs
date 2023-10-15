using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TileMapManager : Singleton<TileMapManager>
{
    public bool CheckAroundSixTiles(int targetX, int targetY)
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
                x = targetX + i;
                y = targetY + j;
                Debug.Log("x:" + x + " y:" + y);
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

    public List<GameObject> ReturnAroundSixItems(int targetX, int targetY)
    {
        List<GameObject> items = new List<GameObject>();
        int x, y;
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (CheckY(targetY, i, j))
                {
                    continue;
                }
                x = targetX + i;
                y = targetY + j;
                if (!CheckMapSizeIndex(x, y))
                {
                    continue;
                }
                GameObject targetItem = HexTileMapGenerator.MapLists[y][x].item;
                if (targetItem != null)
                {
                    items.Add(targetItem);
                }
            }
        }
        return items;
    }

    public void DeleteAroundSixTiles(int targetX, int targetY)
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
                x = targetX + i;
                y = targetY + j;
                if (!CheckMapSizeIndex(x, y))
                {
                    continue;
                }
                HexTileMapGenerator.TileInfo tempTile = HexTileMapGenerator.MapLists[y][x];
                if (tempTile.item != null)
                {
                    Destroy(tempTile.item);
                    tempTile.item = null;
                }
            }
        }
    }

    public Vector2 ReturnAroundEmptyPosition(int targetX, int targetY)
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
                x = targetX + i;
                y = targetY + j;
                if (!CheckMapSizeIndex(x, y))
                {
                    continue;
                }
                HexTileMapGenerator.TileInfo tempTile = HexTileMapGenerator.MapLists[y][x];
                if (tempTile.item == null)
                {
                    return new Vector2(x, y);
                }
            }
        }
        return new Vector2(-1, -1);
    }

    public void CheckCraftableTile(int targetX, int targetY)
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
                x = targetX + i;
                y = targetY + j;
                if (!CheckMapSizeIndex(x, y))
                {
                    continue;
                }
                GameObject recipeResult = CraftingManager.Instance.TargetPositionTileToRecipeResult(x, y);
                if (recipeResult != null)
                {
                    //빨간색으로 표시 혹은 제작 가능 물품 표시
                    //HexTileMapGenerator.MapLists[y][x].baseTile.GetComponent<MeshRenderer>().material.color = Color.red;
                    HexTileMapGenerator.MapLists[y][x].baseTile.GetComponentInChildren<TextMeshProUGUI>().text = "Make\n" + recipeResult.GetComponent<ItemPos>().name;
                    HexTileMapGenerator.MapLists[y][x].baseTile.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
                }
                else
                {
                    HexTileMapGenerator.MapLists[y][x].baseTile.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
                }
            }
        }
    }
    //public bool CheckAroundThreeTopTile(int targetX, int targetY)
    //{
    //    int x, y;
    //    for (int i = -1; i <= 1; i++)
    //    {
    //        for (int j = -1; j <= 1; j++)
    //        {
    //            if (CheckY(targetY, i, j))
    //            {
    //                continue;
    //            }
    //            y = targetY + i; x = targetX + j;
    //            if (!CheckMapSizeIndex(x, y))
    //            {
    //                continue;
    //            }
    //            HexTileMapGenerator.TileInfo tempTile = HexTileMapGenerator.MapLists[y][x];
    //            if (tempTile.item == null)
    //                return false;
    //        }
    //    }
    //    return true;
    //}
    public bool CheckMapSizeIndex(int x, int y)
    {
        if (x >= 0 && x < HexTileMapGenerator.mapWidth && y >= 0 && y < HexTileMapGenerator.mapHeight)
        {
            return true;
        }
        return false;
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

}
