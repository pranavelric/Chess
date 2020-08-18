using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum CellState
{
    None,
    Friendly,
    Enemy,
    Free,
    OutOfBounds
}




public class Board : MonoBehaviour
{

    public GameObject mCellPreFab;
    public Cell[,] mAllCells = new Cell[8, 8];
    // Start is called before the first frame update



    void Start()
    {

    }
    public void Create()
    {

        for (int y = 0; y < 8; y++)
        {
            for (int x = 0; x < 8; x++)
            {
                //create cell
                GameObject newCell = Instantiate(mCellPreFab, transform);
                //create position
                RectTransform rectTransform = newCell.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2((x * 100) + 50, (y * 100) + 50);
                //setup
                mAllCells[x, y] = newCell.GetComponent<Cell>();
                mAllCells[x, y].Setup(new Vector2Int(x, y), this);




            }
        }

        //colouring
        for (int x = 0; x < 8; x += 2)
        {
            for (int y = 0; y < 8; y++)
            {
                int offset = (y % 2 != 0) ? 0 : 1;
                int finalx = x + offset;
                mAllCells[finalx, y].GetComponent<Image>().color = new Color32(230, 220, 187, 255);

            }
        }

    }
    // Update is called once per frame
    void Update()
    {

    }



    public CellState ValidateCell(int targetx, int targety, BasePiece checkingPiece)
    {
        if (targetx < 0 || targetx > 7)
        {
            return CellState.OutOfBounds;
        }
        if (targety < 0 || targety > 7)
        {
            return CellState.OutOfBounds;
        }
        Cell targetCell = mAllCells[targetx, targety];

        if (targetCell.mCurrentPiece != null)
        {
            if (checkingPiece.mColor == targetCell.mCurrentPiece.mColor)
                return CellState.Friendly;
            if (checkingPiece.mColor != targetCell.mCurrentPiece.mColor)
                return CellState.Enemy;


        }
        return CellState.Free;
    }

}
