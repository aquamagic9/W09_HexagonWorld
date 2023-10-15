using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MouseSelect : MonoBehaviour
{
    [SerializeField] float max = 10f;
    [SerializeField] float min = 3f;
    [SerializeField] float zoomSpeed = 2f;

    GameObject SelectedItem = null;
    GameObject RecentSelectedTile;
    HexTileMapGenerator.TileInfo PrevSelectedTile;
    Vector2 mousePosition;
    Vector2 prevMousePosition;
    Color originColor;
    int x, y;
    bool Selecting = false;
    private Vector3 dragOrigin;
    void Awake()
    {
        originColor = GetComponent<MeshRenderer>().material.color;
        Selecting = false;
        mousePosition = new Vector2(0f, 0f);
    }

    void Update()
    {
        prevMousePosition = mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = mousePosition;
        SetActiveSelf();
        SelectItem();
        Draging();
        PutItem();
        CameraZoomInZoomOut();
    }

    void SetActiveSelf()
    {
        Ray ray = new Ray(transform.position + new Vector3(0, 0, -5), new Vector3(0, 0, 1));
        RaycastHit hitData;
        if (Physics.Raycast(ray, out hitData, 50f, LayerMask.GetMask("Ground")))
        {
            GetComponent<MeshRenderer>().enabled = true;
            GetComponent<MeshRenderer>().material.color = originColor;
            this.transform.position = hitData.transform.position;
            string[] splitData = hitData.transform.name.Split(',');
            RecentSelectedTile = hitData.transform.gameObject;
            x = int.Parse(splitData[0]);
            y = int.Parse(splitData[1]);
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }

    void SelectItem()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Selecting = true;
            Ray ray = new Ray((Vector3)mousePosition + new Vector3(0, 0, -5), new Vector3(0, 0, 1));
            RaycastHit hitData;
            LayerMask layerMask = LayerMask.GetMask("Default");
            //layerMask &= ~layerMask;
            Debug.DrawRay((Vector3)mousePosition + new Vector3(0, 0, -5), new Vector3(0, 0, 1) * 10f, Color.red);
            if (Physics.Raycast(ray, out hitData, 50f, layerMask))
            {
                SelectedItem = hitData.transform.gameObject;
                HexTileMapGenerator.MapLists[y][x].item = null;
                TileMapManager.Instance.CheckCraftableTile(x, y);
                PrevSelectedTile = HexTileMapGenerator.MapLists[y][x];
            }
            else
            {
                Debug.Log("ºó°ø°£!");
                SelectedItem = null;
                dragOrigin = Input.mousePosition;
                CombineItems();
            }
        }
    }

    void CombineItems()
    {
        if (HexTileMapGenerator.MapLists[y][x].item == null && TileMapManager.Instance.CheckAroundSixTiles(x, y))
        {
            GameObject craftedItem = CraftingManager.Instance.TargetPositionTileToRecipeResult(x, y);
            if (craftedItem != null)
            {
                ItemManager.Instance.SpawnItem(craftedItem, RecentSelectedTile.transform.position, x, y);
                if (craftedItem.GetComponent<RandomBox>())
                {
                    craftedItem.GetComponent<RandomBox>().ItemList = ItemManager.Instance.ReturnItemPrefabs(TileMapManager.Instance.ReturnAroundSixItems(x, y));
                }
                TileMapManager.Instance.DeleteAroundSixTiles(x, y);
                HexTileMapGenerator.MapLists[y][x].baseTile.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            }
        }
    }

    void Draging()
    {
        if (CanDragingItem())
        {
            SelectedItem.transform.position = mousePosition;
        }
        else if (CanDragingCamera())
        {
            Vector2 movement = Camera.main.ScreenToViewportPoint(dragOrigin - Input.mousePosition);
            Camera.main.transform.Translate(new Vector3(movement.x, movement.y, 0) * Time.deltaTime * 10f, Space.World);
        }
    }

    bool CanDragingItem()
    {
        if (Selecting && Input.GetMouseButton(0) && SelectedItem != null)
        {
            return true;
        }
        return false;
    }

    bool CanDragingCamera()
    {
        if (Selecting && Input.GetMouseButton(0) && SelectedItem == null)
        {
            return true;
        }
        return false;
    }

    void CameraZoomInZoomOut()
    {
        Camera.main.orthographicSize += Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        if (Camera.main.orthographicSize > max)
        {
            Camera.main.orthographicSize = max;
        }
        else if (Camera.main.orthographicSize < min)
        {
            Camera.main.orthographicSize = min;
        }
    }

    void PutItem()
    {
        if (Input.GetMouseButtonUp(0) && SelectedItem != null)
        {
            Selecting = false;
            if (HexTileMapGenerator.MapLists[y][x].item == null)
            {
                SelectedItem.transform.position = RecentSelectedTile.transform.position;
                HexTileMapGenerator.MapLists[y][x].item = SelectedItem;
            }
            else
            {
                SelectedItem.transform.position = PrevSelectedTile.baseTile.transform.position;
                HexTileMapGenerator.MapLists[PrevSelectedTile.y][PrevSelectedTile.x].item = SelectedItem;
            }
            ItemManager.Instance.SetItemPos(SelectedItem.GetComponent<ItemPos>(), x, y);
            TileMapManager.Instance.CheckCraftableTile(x, y);
            HexTileMapGenerator.MapLists[y][x].baseTile.GetComponentInChildren<TextMeshProUGUI>().enabled = false;
            SelectedItem = null;
        }
    }
}
