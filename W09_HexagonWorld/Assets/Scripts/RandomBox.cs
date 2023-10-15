using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBox : MonoBehaviour
{
    [SerializeField] public List<GameObject> ItemList;
    [SerializeField] float spawnTime = 1f;
    [SerializeField] Transform gaugeBar;
    float time = 0f;

    public void Init(List<GameObject> list)
    {
        foreach(GameObject go in list)
        {
            ItemList.Add(go);
        }
    }

    private void Update()
    {
        time += Time.deltaTime;
        gaugeBar.localScale = new Vector3(1f * time / spawnTime, gaugeBar.localScale.y, gaugeBar.localScale.z);
        if (time > spawnTime)
        {
            CreateItem();
            time = 0f;
        }
    }

    void CreateItem()
    {
        int randomIndex = Random.Range(0, ItemList.Count);
        ItemPos itemPos = this.transform.GetComponent<ItemPos>();
        Vector2 targetPos = TileMapManager.Instance.ReturnAroundEmptyPosition(itemPos.x, itemPos.y);
        if (targetPos.x != -1)
        {
            ItemManager.Instance.SpawnItem(ItemList[randomIndex], HexTileMapGenerator.MapLists[(int)targetPos.y][(int)targetPos.x].baseTile.transform
                .position, (int)targetPos.x, (int)targetPos.y);
        }
    }
}
