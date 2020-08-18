using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public abstract class BasePiece : EventTrigger
{


    public bool mIsFirstMove = true;
    [HideInInspector]
    public Color mColor = Color.clear;


    protected Cell mOriginalCell = null;
    protected Cell mCurrentCell = null;

    protected RectTransform mRectTransform = null;
    protected PieceManager mPieceManager;

    protected Cell mTargetCell = null;

    protected Vector3Int mMovement = Vector3Int.one;
    protected List<Cell> mHighlightedCells = new List<Cell>();

    public virtual void Setup(Color newTeamColor, Color32 newSpriteColor, PieceManager newPieceManager)
    {
        mPieceManager = newPieceManager;
        mColor = newTeamColor;
        Debug.Log("COLOER IS ISISISISI   " + mColor);
        GetComponent<Image>().color = newSpriteColor;
        mRectTransform = GetComponent<RectTransform>();

    }

    public virtual void Place(Cell newCell)
    {
        mCurrentCell = newCell;
        mOriginalCell = newCell;
        mCurrentCell.mCurrentPiece = this;

        //object 
        transform.position = newCell.transform.position;
        gameObject.SetActive(true);

    }

    public void Reset()
    {
        Kill();
        Place(mOriginalCell);
    }

    public virtual void Kill()
    {
        mCurrentCell.mCurrentPiece = null;
        gameObject.SetActive(false);
    }


    private void CreateCellPath(int xDirection, int yDirection, int movement)
    {
        Debug.Log("On CreateCellPath");
        int currentX = mCurrentCell.mBoardPosition.x;
        int currentY = mCurrentCell.mBoardPosition.y;
        for (int i = 1; i <= movement; i++)
        {

            currentX += xDirection;
            currentY += yDirection;

            //get state of target cell
            CellState cellState = CellState.None;

            cellState = mCurrentCell.mBoard.ValidateCell(currentX, currentY, this);

            if (cellState == CellState.Enemy)
            {
                mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);
                break;
            }

            if (cellState != CellState.Free)
                break;



            Debug.Log(currentX + "  " + currentY);
            mHighlightedCells.Add(mCurrentCell.mBoard.mAllCells[currentX, currentY]);



        }


    }


    protected virtual void CheckPathing()
    {
        Debug.Log("checkpathing");
        //horizontal
        CreateCellPath(1, 0, mMovement.x);
        CreateCellPath(-1, 0, mMovement.x);

        //vertical
        CreateCellPath(0, 1, mMovement.y);
        CreateCellPath(0, -1, mMovement.y);

        //upper diagon
        CreateCellPath(1, 1, mMovement.z);
        CreateCellPath(-1, 1, mMovement.z);
        //lower dia
        CreateCellPath(-1, -1, mMovement.z);
        CreateCellPath(1, -1, mMovement.z);

    }

    protected void ShowCells()
    {
        Debug.Log("cell are ");
        foreach (Cell cell in mHighlightedCells)
        {
            cell.mOutlineImage.enabled = true;

        }


    }

    protected void ClearCells()
    {
        Debug.Log("clear cell");
        foreach (Cell cell in mHighlightedCells)
        {
            cell.mOutlineImage.enabled = false;

        }

        mHighlightedCells.Clear();
    }

    protected virtual void Move()
    {

        Debug.Log("MOVE");
        mTargetCell.RemovePiece();
        mCurrentCell.mCurrentPiece = null;
        //switch cells 
        mCurrentCell = mTargetCell;
        mCurrentCell.mCurrentPiece = this;
        ////move on board 
        transform.position = mCurrentCell.transform.position;
        mTargetCell = null;

    }




    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        Debug.Log("On begin Drag");
        CheckPathing();
        ShowCells();

    }

    public override void OnPointerDown(PointerEventData eventData)
    {

        base.OnPointerDown(eventData);


        Debug.Log("On pointer click");

        CheckPathing();

        ShowCells();

    }
    public override void OnPointerUp(PointerEventData eventData)
    {


        base.OnPointerUp(eventData);


        Debug.Log("On pointer  up click");

        Debug.Log("mah" + mHighlightedCells);
        ClearCells();


    }

    public override void OnDrag(PointerEventData eventData)
    {
        Debug.Log("On  Drag");
        base.OnDrag(eventData);
        //follow pointer


        transform.position += (Vector3)eventData.delta;

        foreach (Cell cell in mHighlightedCells)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(cell.mRectTransform, Input.mousePosition))
            {
                //in in highliter region
                mTargetCell = cell;
                break;
            }
            mTargetCell = null;
        }

    }



    public override void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("On end Drag");
        base.OnEndDrag(eventData);
        ClearCells();
        if (!mTargetCell)
        {
            transform.position = mCurrentCell.gameObject.transform.position;
            return;
        }

        Move();
        mPieceManager.SwitchSides(mColor);
    }



}
