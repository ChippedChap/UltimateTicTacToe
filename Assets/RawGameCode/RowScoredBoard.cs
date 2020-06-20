using UnityEngine;

/*
    For each horizontal and vertical, antidiagonal and diagonal rows, a player has a score.
    When a player makes a move on an axis, their score for that corresponding axis increases.
    When the axis score reaches the length of that axis, that player wins.
    Supports multiple players. Players are zero indexed.

    Useful for if I have time and want to make ultimate tic tac toe or any other TTT variation.

    PORTED FROM JAVA
*/

public abstract class RowScoredBoard
{
    protected int[,] horzScores;
    protected int[,] vertScores;
    protected int[,] diagScores;
    protected int[,] antiDiagScores;

    public int Height { get; private set; }

    public int Width { get; private set; }

    public int PlayersSupported
    {
        get
        {
            return horzScores.Length;
        }
    }

    public RowScoredBoard(int h, int w, int p)
    {
        Height = h;
        Width = w;

        horzScores = new int[p, h];
        vertScores = new int[p, w];
        // Only the largest diagonal/antidiagonal will yield a win. |h - x| num of extra diagonals.
        diagScores = new int[p, Mathf.Abs(Height - Width) + 1];
        antiDiagScores = new int[p, Mathf.Abs(Height - Width) + 1];
    }

    protected void UpdateScore(int[] pos, int playerIndex)
    {
        UpdateScore(pos[0], pos[1], playerIndex);
    }

    protected bool PlayerWon(int playerIndex)
    {
        for (int y = 0; y < Height; y++)
            if (horzScores[playerIndex, y] == Width) return true;
        for (int x = 0; x < Width; x++)
            if (vertScores[playerIndex, x] == Height) return true;
        for (int d = 0; d < 1 + Mathf.Abs(Height - Width); d++)
        {
            if (diagScores[playerIndex, d] == Mathf.Min(Width, Height)) return true;
            if (antiDiagScores[playerIndex, d] == Mathf.Min(Width, Height)) return true;
        }
        return false;
    }

    protected void UpdateScore(int x, int y, int playerIndex)
    {
        // Update horizontal / vertical rows
        horzScores[playerIndex, y]++;
        vertScores[playerIndex, x]++;
        // Update antidiagonal row. Redundant constants for code clarity (check if point is on line of slope 1)
        for (int d = 0; d < 1 + Mathf.Abs(Height - Width); d++)
        {
            if (y >= 0)
            {
                if (y - d == x) antiDiagScores[playerIndex, d]++;
                if (y - Height - d == -x) diagScores[playerIndex, d]++;
            }
            else
            {
                if (y  == x - d) antiDiagScores[playerIndex, d]++;
                if (y - Height == -x - d) diagScores[playerIndex, d]++;
            }
        }
    }
}