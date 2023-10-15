using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSelect : MonoBehaviour
{
    [SerializeField] float max = 10f;
    [SerializeField] float min = 3f;
    [SerializeField] float zoomSpeed = 2f;

    GameObject SelectedItem = null;
    GameObject RecentSelectedTile;
    Vector2 mousePosition;
    Vector2 prevMousePosition;
    Color originColor;
    int x, y;
    bool Selecting = false;
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
            }
            else
            {
                Debug.Log("빈공간!");
                SelectedItem = null;
                CombineItems();
            }
        }
    }

    void CombineItems()
    {
        if (HexTileMapGenerator.MapLists[y][x].item == null && TileMapManager.Instance.CheckAroundSixTiles(x, y))
        {
            Debug.Log("6개 존재 확인!");
            //조합법을 읽고 하나를 생성
            ItemManager.Instance.SpawnItem(RecentSelectedTile.transform.position, x, y);

            Debug.Log("아이템 스폰!");
            //주위의 6개의 타일에 있던 정보와 object 제거함
            TileMapManager.Instance.DeleteAroundSixTiles(x, y);
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
            Vector2 movement = prevMousePosition - mousePosition;
            Camera.main.transform.Translate(new Vector3(movement.x, movement.y, 0) * Time.deltaTime * 100f);
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
            SelectedItem.transform.position = RecentSelectedTile.transform.position;
            Debug.Log(x + ":" + y);
            HexTileMapGenerator.MapLists[y][x].item = SelectedItem;
            ItemManager.Instance.SetItemPos(SelectedItem.GetComponent<ItemPos>(), x, y);
            SelectedItem = null;
        }
    }
}
