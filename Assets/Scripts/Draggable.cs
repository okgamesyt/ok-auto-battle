using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public LayerMask releaseMask;
    public Vector3 dragOffset = new Vector3(0, -0.4f, 0);
    
    private Camera cam;
    private SpriteRenderer spriteRenderer;
    
    private Vector3 oldPosition;
    private int oldSortingOrder;
    private Tile previousTile = null;
    
    public bool IsDragging = false;
    
    private void Start()
    {
        cam = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnStartDrag()
    {
        //Debug.Log(this.name + " start drag");

        oldPosition = this.transform.position;
        oldSortingOrder = spriteRenderer.sortingOrder;

        spriteRenderer.sortingOrder = 20;
        IsDragging = true;
    }

    public void OnDragging()
    {
        if (!IsDragging)
            return;
        
        //Debug.Log(this.name + " dragging");

        Vector3 newPosition = cam.ScreenToWorldPoint(Input.mousePosition) + dragOffset;
        newPosition.z = 0;
        this.transform.position = newPosition;

        Tile tileUnder = GetTileUnder();
        if (tileUnder != null)
        {
            tileUnder.SetHighlight(true, !GridManager.Instance.GetNodeForTile(tileUnder).IsOccupied);

            if (previousTile != null && tileUnder != previousTile)
            {
                //We are over a different tile.
                previousTile.SetHighlight(false, false);
            }

            previousTile = tileUnder;
        }
    }

    public void OnEndDrag()
    {
        if (!IsDragging)
            return;
        
       // Debug.Log(this.name + " end drag");

        if (!TryRelease())
        {
            //Nothing was found, return to original position.
            this.transform.position = oldPosition;
        }

        if (previousTile != null)
        {
            previousTile.SetHighlight(false, false);
            previousTile = null;
        }

        spriteRenderer.sortingOrder = oldSortingOrder;

        IsDragging = false;
    }

    private bool TryRelease()
    {
        //Released over something!
        Tile t = GetTileUnder();
        if (t != null)
        {
            //It's a tile!
            BaseEntity thisEntity = GetComponent<BaseEntity>();
            Node candidateNode = GridManager.Instance.GetNodeForTile(t);
            if (candidateNode != null && thisEntity != null)
            {
                if (!candidateNode.IsOccupied)
                {
                    //Let's move this unity to that node
                    thisEntity.CurrentNode.SetOccupied(false);
                    thisEntity.SetCurrentNode(candidateNode);
                    candidateNode.SetOccupied(true);
                    thisEntity.transform.position = candidateNode.worldPosition;

                    return true;
                }
            }
        }
        

        return false;
    }

    public Tile GetTileUnder()
    {
        RaycastHit2D hit =
            Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, 100, releaseMask);

        if (hit != null && hit.collider != null)
        {
            //Released over something!
            Tile t = hit.collider.GetComponent<Tile>();
            return t;
        }

        return null;
    }
}
