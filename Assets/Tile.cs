using System;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int layer = 3;
    public int length = 3;
    public int width = 3;

    private Tile parent;
    private TileInput inputProcessor;

    private bool invalid;
    private Transform selectionHolder;
    private Transform childHolder;
    private Tile[] childrenIndexed;
    private Transform modelHolder;
    private List<GameObject> playerModels = new List<GameObject>();

    public void MoveHere(int player)
    {
        if(player > playerModels.Count - 1) throw new ArgumentException("No corresponding playermodel for player " + player);
        if (invalid) return;
        playerModels[player].SetActive(true);
        Debug.Log(string.Join(", ", BoardIndex()));
        Game.CurrentGame.MakeMove(BoardIndex());
        MarkInvalid();
    }   

    public List<int> BoardIndex()
    {
        if (!parent) return new List<int>();
        List<int> parentIndex = parent.BoardIndex();
        parentIndex.Add(parent.GetChildIndex(this));
        return parentIndex;
    }

    public int GetChildIndex(Tile t)
    {
        for (int i = 0; i < childrenIndexed.Length; i++)
            if (childrenIndexed[i] == t)
                return i;
        return 0;
    }

    public void MarkValid()
    {
        inputProcessor.SetValid();
        invalid = false;
    }

    public void MarkInvalid()
    {
        inputProcessor.SetInvalid();
        invalid = true;
    }

    void Start()
    {
        selectionHolder = transform.Find("Selection");
        inputProcessor = selectionHolder.gameObject.GetComponent<TileInput>();
        childHolder = transform.Find("Children");
        childrenIndexed = new Tile[length * width];
        modelHolder = transform.Find("PlayerModel");
        for (int i = 0; i < modelHolder.childCount; i++) playerModels.Add(modelHolder.GetChild(i).gameObject);

        Tile childTemplate = null;
        if (layer > 1)
        {
            int count = 0;
            for (int i = 0; i < length; i++)
                for (int j = 0; j < width; j++)
                {
                    childTemplate = CreateChild(i, j, childTemplate);
                    childrenIndexed[count] = childTemplate;
                    count++;
                }
        }
        else
        {
            selectionHolder.gameObject.SetActive(true);
            childHolder.gameObject.SetActive(true);
            MarkValid();
        }
    }

    Tile CreateChild(int x, int y, Tile copy)
    {
        Tile t = Instantiate((copy) ? copy : this, childHolder.transform, false);
        t.transform.localPosition = new Vector3(x - length / 2, 0, y - width / 2);
        t.parent = this;
        if (!copy)
        {
            // First copy setup
            t.transform.localScale = new Vector3(1, 1, 1);
            t.layer--;
        }
        copy = t;
        return copy;
    }
}
