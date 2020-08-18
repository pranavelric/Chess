using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Board mBoard;
    public PieceManager mPieceManager;
    void Start()
    {

        mBoard.Create();


        //create pieces
        mPieceManager.Setup(mBoard);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
