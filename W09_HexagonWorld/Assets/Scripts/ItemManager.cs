using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : Singleton<ItemManager>
{
    public GameObject[] ItemPrefabs;

    //public GameObject SpawnItem(GameObject itemPrefab, Vector2 position)
    //{
    //    GameObject item = Instantiate(itemPrefab);
    //    item.transform.position = position;
    //    return item;
    //}

    public GameObject SpawnItem(Vector2 position, int x, int y)
    {
        GameObject item = Instantiate(ItemPrefabs[0]);
        SetItemPos(item.GetComponent<ItemPos>(), x, y);
        item.transform.position = position;
        HexTileMapGenerator.MapLists[y][x].item = item;
        return item;
    }
    public GameObject SpawnItem(int index, Vector2 position, int x, int y)
    {
        GameObject item = Instantiate(ItemPrefabs[index]);
        SetItemPos(item.GetComponent<ItemPos>(), x, y);
        item.transform.position = position;
        HexTileMapGenerator.MapLists[y][x].item = item;
        return item;
    }
    public GameObject SpawnItem(GameObject targetItem, Vector2 position, int x, int y)
    {
        if (targetItem == null)
            return null;
        GameObject item = Instantiate(targetItem);
        SetItemPos(item.GetComponent<ItemPos>(), x, y);
        item.transform.position = position;
        HexTileMapGenerator.MapLists[y][x].item = item;
        return item;
    }

    public void SetItemPos(ItemPos item, int x, int y)
    {
        item.x = x;
        item.y = y;
    }
}
