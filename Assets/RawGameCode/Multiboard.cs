using System.Collections.Generic;
using System.Linq;

public class Multiboard : RowScoredBoard
{
	private int layer;
	private Multiboard[] children;
	private Multiboard parentBoard;
	private int indexOnParent;

	public Multiboard(int h, int w, int p, int l, Multiboard parent = null, int i = -1) : base(h, w, p)
	{
		layer = l;
		parentBoard = parent;
		indexOnParent = i;
		if (layer > 1) CreateChildren(h, w, p, l - 1);
	}

	public void Move(List<int> multiIndex, int playerIndex)
	{
		int leadIndex = multiIndex[0];
		multiIndex.Remove(leadIndex);
		UpdateScore(IndexToPos(leadIndex), playerIndex);
		children[leadIndex].Move(multiIndex, playerIndex);
	}

	public int[] IndexToPos(int i)
	{
		return new int[]{ i % Width, i / Width };
	}

	void CreateChildren(int h, int w, int p, int l)
	{
		children = new Multiboard[h * w];
		for (int i = 0; i < Height * Width; i++)
			children[i] = new Multiboard(h, w, p, l, this, i);
	}
}
