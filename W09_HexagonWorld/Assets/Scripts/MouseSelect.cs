using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSelect : MonoBehaviour
{
    GameObject SelectedItem = null;
    GameObject RecentSelectedTile;
    Vector2 mousePosition;
    Color originColor;
    int x, y;
    bool Selecting = false;
    // Start is called before the first frame update
    void Awake()
    {
        originColor = GetComponent<MeshRenderer>().material.color;
        Selecting = false;
    }

    // Update is called once per frame
    void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        this.transform.position = mousePosition;
        SetActiveSelf();
        SelectItem();
        DragingItem();
        PutItem();
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
            string []splitData = hitData.transform.name.Split(',');
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
                SelectedItem = null;
            }
        }
    }

    void DragingItem()
    {
        if (Selecting && Input.GetMouseButton(0) && SelectedItem != null)
        {
            SelectedItem.transform.position = mousePosition;
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
            SelectedItem = null;
        }
    }
}
