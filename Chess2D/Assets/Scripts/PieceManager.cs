using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class PieceManager : MonoBehaviour
{

    public Text player;
    public Text winner;

    public GameObject mPiecePrefab;
    private List<BasePiece> mWhitePiece = null;
    private List<BasePiece> mBlackPiece = null;
    private List<BasePiece> mPromotePieces = new List<BasePiece>();
    private string[] mPieceOrder = new string[16]{
"P","P","P","P","P","P","P","P",
"R","KN","B","Q","K","B","KN","R"
    };

    public bool mIsKingAlive = true;
    private Dictionary<string, Type> mPieceLibrary = new Dictionary<string, Type>()
    {
        {"P",typeof(Pawn)},
        {"R",typeof(Rook)},
        {"KN",typeof(Knight)},
        {"B",typeof(Bishop)},
        {"K",typeof(King)},
        {"Q",typeof(Queen)},
    };
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("playerName").GetComponent<Text>();
        winner = GameObject.Find("winner").GetComponent<Text>();
    }


    public void Setup(Board board)
    {
        mWhitePiece = CreatePieces(Color.white, new Color32(80, 124, 159, 255), board);
        mBlackPiece = CreatePieces(Color.black, new Color32(210, 95, 64, 255), board);
        //place pieces
        PlacePieces(1, 0, mWhitePiece, board);
        PlacePieces(6, 7, mBlackPiece, board);

        SwitchSides(Color.black);
    }

    private void SetInteractive(List<BasePiece> allPieces, bool value)
    {
        foreach (BasePiece piece in allPieces)
            piece.enabled = value;
    }
    public void SwitchSides(Color color)
    {

        if (!mIsKingAlive)
        {
            if (color == Color.black)
            {
                winner.text = "Black Win";


            }
            else
            {
                winner.text = "White Win";

            }

            ResetPieces();

            mIsKingAlive = true;
            color = Color.black;

            player.text = "White Player";

        }
        bool isBlackTurn = color == Color.white ? true : false;
        if (isBlackTurn)
        {

            player.text = "Black Player";
        }
        else
        {
            player.text = "White Player";
        }

        SetInteractive(mWhitePiece, !isBlackTurn);
        SetInteractive(mBlackPiece, isBlackTurn);

        //set Promotes interactiviel
        foreach (BasePiece piece in mPromotePieces)
        {
            bool isBlackpiece = piece.mColor != Color.white ? true : false;
            bool isPartOfTeam = isBlackpiece == true ? isBlackTurn : !isBlackTurn;

            piece.enabled = isPartOfTeam;

        }

    }

    public void ResetPieces()
    {

        foreach (BasePiece piece in mPromotePieces)
        {
            piece.Kill();
            Destroy(piece.gameObject);

        }
        foreach (BasePiece piece in mWhitePiece)
            piece.Reset();
        foreach (BasePiece piece in mBlackPiece)
            piece.Reset();

    }


    private List<BasePiece> CreatePieces(Color teamColor, Color32 spriteColor, Board board)
    {
        List<BasePiece> newPiece = new List<BasePiece>();
        for (int i = 0; i < mPieceOrder.Length; i++)
        {

            //get type aplly to obj 
            string key = mPieceOrder[i];
            Type pieceType = mPieceLibrary[key];


            //store new pieces
            BasePiece newPiecee = CreatePiece(pieceType);

            newPiece.Add(newPiecee);

            //setip piewce
            newPiecee.Setup(teamColor, spriteColor, this);
        }

        return newPiece;

    }

    private BasePiece CreatePiece(Type pieceType)
    {
        GameObject newPieceObject = Instantiate(mPiecePrefab);

        //set paretn to pieceamange
        newPieceObject.transform.SetParent(transform);

        //set scale ansd position
        newPieceObject.transform.localScale = new Vector3(1, 1, 1);
        newPieceObject.transform.localRotation = Quaternion.identity;
        BasePiece newPiece = (BasePiece)newPieceObject.AddComponent(pieceType);
        return newPiece;
    }
    private void PlacePieces(int pawnRow, int royalityRow, List<BasePiece> pieces, Board board)
    {
        for (int i = 0; i < 8; i++)
        {
            //place pawn
            pieces[i].Place(board.mAllCells[i, pawnRow]);
            //place royality 
            pieces[i + 8].Place(board.mAllCells[i, royalityRow]);

        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PromotePieces(Pawn pawn, Cell cell, Color teamColor, Color SpriteColor)
    {
        //kill pawn
        pawn.Kill();
        //create 
        BasePiece promotedPiece = CreatePiece(typeof(Queen));
        promotedPiece.Setup(teamColor, SpriteColor, this);

        //p[lacing 
        promotedPiece.Place(cell);


        //addin to promote 
        mPromotePieces.Add(promotedPiece);
    }

}
