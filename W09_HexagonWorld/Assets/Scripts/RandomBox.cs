using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBox : MonoBehaviour
{
    [SerializeField] List<GameObject> ItemList;
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
        int randomIndex = Random.Range(0, 6);
        //ItemList[randomIndex];
        ItemPos itemPos = this.transform.GetComponent<ItemPos>();
        Vector2 targetPos = TileMapManager.Instance.ReturnAroundEmptyPosition(itemPos.x, itemPos.y);
        if (targetPos.x != -1)
        {
            //index를 넘기는 것이 아닌 프리팹을 넘겨서 하는 방식으로 바꾸기.
            ItemManager.Instance.SpawnItem(randomIndex, HexTileMapGenerator.MapLists[(int)targetPos.y][(int)targetPos.x].baseTile.transform
                .position, (int)targetPos.x, (int)targetPos.y);
        }
    }
}
