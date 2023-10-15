using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HexTileMapGenerator : MonoBehaviour
{
    public class TileInfo
    {
        public TileInfo(int x, int y, GameObject item, GameObject baseTile)
        {
            this.x = x;
            this.y = y;
            this.item = item;
            this.baseTile = baseTile;
        }
        public int x { get; set; }
        public int y { get; set; }
        public GameObject item { get; set; }
        public GameObject baseTile { get; set; }
    }
    
    public static List<List<TileInfo>> MapLists;
    public GameObject hexTilePrefab;
    public static int mapWidth = 25;
    public static int mapHeight = 12;

    [SerializeField] float tileXOffset = 1.8f;
    [SerializeField] float tileZOffset = 1.565f;

    [SerializeField] Transform trash;
    [SerializeField] List<GameObject> FirstItemLists;
    
    private void Start()
    {
        MapLists = new List<List<TileInfo>>();
        for (int i = 0; i < mapHeight; i++)
        {
            MapLists.Add(new List<TileInfo>());
        }
        CreateHexTileMap();
        CreateFirstRandomBox();
    }

    public void CreateFirstRandomBox()
    {
        GameObject randomBox = Instantiate(CraftingManager.Instance.RandomBoxPrefab);
        randomBox.transform.position = MapLists[6][9].baseTile.transform.position;
        MapLists[9][6].item = randomBox;
        randomBox.GetComponent<ItemPos>().x = 9;
        randomBox.GetComponent<ItemPos>().y = 6;
        List<GameObject> list = new List<GameObject>();
        GameObject temp;
        temp = Instantiate(FirstItemLists[0]);
        temp.transform.parent = trash;
        temp.transform.position = trash.position;
        list.Add(temp);

        temp = Instantiate(FirstItemLists[1]);
        temp.transform.parent = trash;
        temp.transform.position = trash.position;
        list.Add(temp);

        temp = Instantiate(FirstItemLists[2]);
        temp.transform.parent = trash;
        temp.transform.position = trash.position;
        list.Add(temp);

        temp = Instantiate(FirstItemLists[3]);
        temp.transform.parent = trash;
        temp.transform.position = trash.position;
        list.Add(temp);

        randomBox.GetComponent<RandomBox>().ItemList = list;
    }

    public void CreateHexTileMap()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int y = 0; y < mapHeight; y++)
            {
                GameObject TempGO = Instantiate(hexTilePrefab);
                
                if (y % 2 == 0)
                {
                    TempGO.transform.position = SetStartPosition(new Vector3(x * tileXOffset, y * tileZOffset, 0));
                }
                else
                {
                    TempGO.transform.position = SetStartPosition(new Vector3(x * tileXOffset + tileXOffset / 2, y * tileZOffset, 0));
                }
                SetTileInfo(TempGO, x, y);
                
                TileInfo TempTile = new TileInfo(x, y, null, TempGO);
                MapLists[y].Add(TempTile);

            }
        }
    }

    void SetTileInfo(GameObject GO, int x, int z)
    {
        GO.transform.parent = transform;
        GO.name = x.ToString() + ", " + z.ToString();
    }

    Vector3 SetStartPosition(Vector3 pos)
    {
        return pos + new Vector3(transform.position.x, transform.position.y, 0);
    }
}
