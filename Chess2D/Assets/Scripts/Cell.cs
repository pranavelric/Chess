using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{

    public Image mOutlineImage;

    public Vector2Int mBoardPosition = Vector2Int.zero;

    public Board mBoard = null;

    public RectTransform mRectTransform = null;
    // Start is called before the first frame update
    public BasePiece mCurrentPiece = null;
    void Start()
    {

    }


    public void Setup(Vector2Int newBoardPosition, Board newBoard)
    {
        mBoardPosition = newBoardPosition;
        mBoard = newBoard;
        mRectTransform = GetComponent<RectTransform>();

    }

    public void RemovePiece()
    {
        if (mCurrentPiece != null)
        {
            mCurrentPiece.Kill();

        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
