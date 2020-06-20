using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public int layers = 3;
    public int width = 3;
    public int height = 3;
    public Material activeTileMaterial;
    private List<Material> playerMaterials = new List<Material>();
    private Multiboard board;

    public static Game CurrentGame { get; private set; }

    public int TurnIndex { get; private set; } = 0;

    public int Players
    {
        get { return transform.childCount; }
    }

    public void MakeMove(List<int> moveIndex)
    {
        //board.Move(moveIndex, TurnIndex);
        NextPlayer();
    }

    void Awake()
    {
        CurrentGame = this;
        board = new Multiboard(height, width, Players, layers);
    }

    private void Start()
    {
        FindPlayerMaterials();
        UpdateActiveMaterial();
    }

    void FindPlayerMaterials()
    {
        for (int i = 0; i < transform.childCount; i++)
            if (transform.GetChild(i).gameObject.TryGetComponent(out MeshRenderer renderer))
                playerMaterials.Add(renderer.material);
    }

    void UpdateActiveMaterial()
    {
        Color emission = playerMaterials[TurnIndex].GetColor("_Color");
        emission.a = 1f;
        activeTileMaterial.SetColor("_EmissionColor", emission);
    }

    void NextPlayer()
    {
        TurnIndex = (TurnIndex + 1) % Players;
        UpdateActiveMaterial();
    }
}
