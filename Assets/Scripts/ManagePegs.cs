using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagePegs : MonoBehaviour {

    public BoardManager boardManagerScript;

    float timeMouseDown;
    float boardRotationX;
    float boardRotationY;
    float boardRotationZ;
    int dragRotationTolerance = 1;

    // Use this for initialization
    void Start () {
        boardRotationX = boardManagerScript.gameObject.transform.localRotation.x;
        boardRotationY = boardManagerScript.gameObject.transform.localRotation.y;
        boardRotationZ = boardManagerScript.gameObject.transform.localRotation.z;
    }

    void OnMouseDown()
    {
        timeMouseDown = Time.time;
        // update stored handles to board rotation
        boardRotationX = boardManagerScript.gameObject.transform.localRotation.x;
        boardRotationY = boardManagerScript.gameObject.transform.localRotation.y;
        boardRotationZ = boardManagerScript.gameObject.transform.localRotation.z;
    }

    void OnMouseUp()
    {
        // if mouse was pressed and released quickly and the rotation of the board was not changed much (as caused by a drag)
        if (Time.time - timeMouseDown < 0.25f 
            && System.Math.Abs(boardManagerScript.gameObject.transform.localRotation.x - boardRotationX) < dragRotationTolerance
            && System.Math.Abs(boardManagerScript.gameObject.transform.localRotation.y - boardRotationY) < dragRotationTolerance
            && System.Math.Abs(boardManagerScript.gameObject.transform.localRotation.z - boardRotationZ) < dragRotationTolerance)
        {
            PlacePiece();
        }
        else
        {
            // update stored handles to board rotation
            boardRotationX = boardManagerScript.gameObject.transform.localRotation.x;
            boardRotationY = boardManagerScript.gameObject.transform.localRotation.y;
            boardRotationZ = boardManagerScript.gameObject.transform.localRotation.z;
        }
    }

    void PlacePiece()
    {
        string clickedPeg = CheckWhichPeg();
        ShowHidePiece(clickedPeg);
    }

    string CheckWhichPeg()
    {
        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                string compareName = "Peg" + i + j;
                if (gameObject.name == compareName)
                {
                    return compareName;
                }
            }
        }
        // should not reach this point
        throw new System.Exception("Script on peg said script was clicked, but no peg was matched");
    }

    void ShowHidePiece(string pegName)
    {
        int row = (int)System.Char.GetNumericValue(pegName[3]);
        int col = (int)System.Char.GetNumericValue(pegName[4]);

        if (boardManagerScript.isXTurn)
        {
            boardManagerScript.pieceTypeChar = 'X';
        }
        else
        {
            boardManagerScript.pieceTypeChar = 'O';
        }

        // hide previously placed temporary piece
        if (boardManagerScript.currentGamePiece != null)
        {
            boardManagerScript.currentGamePiece.SetActive(false);
        }

        if (boardManagerScript.currentBoardLayout[col, 0, row] == 'E' && !boardManagerScript.isGameOver && !boardManagerScript.flipped)
        {
            // unhide prefab on bottom level of board
            if (boardManagerScript.isXTurn)
            {
                boardManagerScript.piecesPlacedX[col, 0, row].SetActive(true);
                // keep track of reference to temporary piece enabled
                boardManagerScript.currentGamePiece = boardManagerScript.piecesPlacedX[col, 0, row];
            }
            else
            {
                boardManagerScript.piecesPlacedO[col, 0, row].SetActive(true);
                // keep track of reference to temporary piece enabled
                boardManagerScript.currentGamePiece = boardManagerScript.piecesPlacedO[col, 0, row];
            }

            // updates temporary piece location so the piece can be officially placed when SubmitMove button is clicked
            boardManagerScript.curPieceCol = col;
            boardManagerScript.curPieceHeight = 0;
            boardManagerScript.curPieceRow = row;
        }
        else if (boardManagerScript.currentBoardLayout[col, 1, row] == 'E' && !boardManagerScript.isGameOver && !boardManagerScript.flipped)
        {
            // unhide prefab on middle level of board
            if (boardManagerScript.isXTurn)
            {
                boardManagerScript.piecesPlacedX[col, 1, row].SetActive(true);
                // keep track of reference to temporary piece enabled
                boardManagerScript.currentGamePiece = boardManagerScript.piecesPlacedX[col, 1, row];
            }
            else
            {
                boardManagerScript.piecesPlacedO[col, 1, row].SetActive(true);
                // keep track of reference to temporary piece enabled
                boardManagerScript.currentGamePiece = boardManagerScript.piecesPlacedO[col, 1, row];
            }

            // updates temporary piece location so the piece can be officially placed when SubmitMove button is clicked
            boardManagerScript.curPieceCol = col;
            boardManagerScript.curPieceHeight = 1;
            boardManagerScript.curPieceRow = row;
        }
        else if (boardManagerScript.currentBoardLayout[col, 2, row] == 'E' && !boardManagerScript.isGameOver && !boardManagerScript.flipped)
        {
            // unhide prefab on bottom level of board
            if (boardManagerScript.isXTurn)
            {
                boardManagerScript.piecesPlacedX[col, 2, row].SetActive(true);
                // keep track of reference to temporary piece enabled
                boardManagerScript.currentGamePiece = boardManagerScript.piecesPlacedX[col, 2, row];
            }
            else
            {
                boardManagerScript.piecesPlacedO[col, 2, row].SetActive(true);
                // keep track of reference to temporary piece enabled
                boardManagerScript.currentGamePiece = boardManagerScript.piecesPlacedO[col, 2, row];
            }

            // updates temporary piece location so the piece can be officially placed when SubmitMove button is clicked
            boardManagerScript.curPieceCol = col;
            boardManagerScript.curPieceHeight = 2;
            boardManagerScript.curPieceRow = row;
        }
        //else if (boardManagerScript.isGameOver)
        //{
        //    // handle clicks after game is over such as tap to restart
        //}
    }
}
