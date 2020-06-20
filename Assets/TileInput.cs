using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInput : MonoBehaviour
{
    [System.NonSerialized]
    public Tile attachedTile;
    public Material activatedMaterial;
    public Material unactivatedMaterial;
    private MeshRenderer selectionRenderer;

    public void SetValid()
    {
        selectionRenderer.material = activatedMaterial;
    }

    public void SetInvalid()
    {
        selectionRenderer.material = unactivatedMaterial;
    }

    void Awake()
    {
        selectionRenderer = gameObject.GetComponent<MeshRenderer>();
        SetInvalid();
    }

    void Start()
    {
        attachedTile = transform.parent.gameObject.GetComponent<Tile>();
    }

    void OnMouseDown()
    {
        attachedTile.MoveHere(Game.CurrentGame.TurnIndex);
    }
}
