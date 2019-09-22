using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    ScreenOrientation prevOrientation;

    public Button btnSubmit;
    public Button btnFlipBoard;
    public Button btnNewGame;
    public Text txtGameOver;
    public Text txtTurn;

    public BoardManager boardManagerScript;

    // region contains screen rotation handling
    //#region
    //// landscape left
    //private Vector2 anchorMinLandscapeLeft = new Vector2(1.0f, 0.5f);
    //private Vector2 anchorMaxLandscapeLeft = new Vector2(1.0f, 0.5f);
    //private Vector3 posLandscapeLeft = new Vector3(-140, 0);

    //// landscape right
    //private Vector2 anchorMinLandscapeRight = new Vector2(0.0f, 0.5f);
    //private Vector2 anchorMaxLandscapeRight = new Vector2(0.0f, 0.5f);
    //private Vector3 posLandscapeRight = new Vector3(140, 0);

    //// portrait
    //private Vector2 anchorMinPortrait = new Vector2(0.5f, 0.0f);
    //private Vector2 anchorMaxPortrait = new Vector2(0.5f, 0.0f);
    //private Vector3 posPortrait = new Vector3(200, 70);

    //// portrait upside down
    //private Vector2 anchorMinPortraitUpsideDown = new Vector2(0.5f, 1.0f);
    //private Vector2 anchorMaxPortraitUpsideDown = new Vector2(0.5f, 1.0f);
    //private Vector3 posPortraitUpsideDown = new Vector3(200, -70);
    //#endregion

    void Start()
    {


        //prevOrientation = Screen.orientation;
        //Debug.Log(Screen.orientation);
        //HandleOrientationChanged();

        btnSubmit.onClick.AddListener(SubmitMoveOnClick);
        btnNewGame.onClick.AddListener(NewGameOnClick);
        btnFlipBoard.onClick.AddListener(FlipBoardOnClick);

        btnNewGame.gameObject.SetActive(false);
        txtGameOver.gameObject.SetActive(false);
        txtTurn.text = "X's Turn";
        btnFlipBoard.gameObject.SetActive(true);
        boardManagerScript.flipped = false;
        boardManagerScript.flipUsedX = false;
        boardManagerScript.flipUsedO = false;
    }

	// Update is called once per frame
	void Update () {
        // if the orientation has changed since the last time it was checked
        //if (prevOrientation != Screen.orientation)
        //{
        //    HandleOrientationChanged();
        //}
        //prevOrientation = Screen.orientation;
	}

    // handles changes in display based on screen orientation
    //void HandleOrientationChanged()
    //{
    //    UpdateButtonLayout();
    //}

    // updates button anchors and layouts
    //void UpdateButtonLayout()
    //{
    //    switch (Screen.orientation)
    //    {
    //        case ScreenOrientation.LandscapeLeft:
    //            btnSubmit.GetComponent<RectTransform>().anchorMin = anchorMinLandscapeLeft;
    //            btnSubmit.GetComponent<RectTransform>().anchorMin = anchorMaxLandscapeLeft;
    //            btnSubmit.transform.localPosition = posLandscapeLeft;

    //            Debug.Log("Landscape Left");

    //            //btnFlipBoard.GetComponent<RectTransform>().anchorMin = anchorMinLandscapeLeft;
    //            //btnFlipBoard.GetComponent<RectTransform>().anchorMin = anchorMaxLandscapeLeft;
    //            break;
    //        case ScreenOrientation.LandscapeRight:
    //            btnSubmit.GetComponent<RectTransform>().anchorMin = anchorMinLandscapeRight;
    //            btnSubmit.GetComponent<RectTransform>().anchorMax = anchorMaxLandscapeRight;
    //            btnSubmit.transform.localPosition = posLandscapeRight;

    //            Debug.Log("Landscape Right");

    //            //btnFlipBoard.GetComponent<RectTransform>().anchorMin = anchorMinLandscapeRight;
    //            //btnFlipBoard.GetComponent<RectTransform>().anchorMax = anchorMaxLandscapeRight;
    //            break;
    //        case ScreenOrientation.Portrait:
    //            btnSubmit.GetComponent<RectTransform>().anchorMin = anchorMinPortrait;
    //            btnSubmit.GetComponent<RectTransform>().anchorMin = anchorMaxPortrait;
    //            btnSubmit.transform.localPosition = posPortrait;

    //            Debug.Log("Portrait");

    //            //btnFlipBoard.GetComponent<RectTransform>().anchorMin = anchorMinPortrait;
    //            //btnFlipBoard.GetComponent<RectTransform>().anchorMin = anchorMaxPortrait;
    //            break;
    //        case ScreenOrientation.PortraitUpsideDown:
    //            btnSubmit.GetComponent<RectTransform>().anchorMin = anchorMinPortraitUpsideDown;
    //            btnSubmit.GetComponent<RectTransform>().anchorMin = anchorMaxPortraitUpsideDown;
    //            btnSubmit.transform.localPosition = posPortraitUpsideDown;

    //            Debug.Log("Portrait Upside Down");

    //            //btnFlipBoard.GetComponent<RectTransform>().anchorMin = anchorMinPortraitUpsideDown;
    //            //btnFlipBoard.GetComponent<RectTransform>().anchorMin = anchorMaxPortraitUpsideDown;
    //            break;
    //    }
    //}

    void SubmitMoveOnClick()
    {
        if (boardManagerScript.flipped)
        {
            // player uses their power up
            if (boardManagerScript.isXTurn)
            {
                boardManagerScript.flipUsedX = true;
            }
            else
            {
                boardManagerScript.flipUsedO = true;
            }
            // switch to next person's turn
            boardManagerScript.isXTurn = !boardManagerScript.isXTurn;

            // check if there is a winner or a tie
            boardManagerScript.CheckIfFlippedWinner();
            boardManagerScript.flipped = false;
            btnFlipBoard.gameObject.SetActive(false);
        }
        else
        {
            HandlePlacePiece();
        }
    }

    void HandlePlacePiece()
    {
        if (boardManagerScript.currentGamePiece != null && boardManagerScript.currentGamePiece.activeSelf)
        {
            boardManagerScript.currentBoardLayout[
                    boardManagerScript.curPieceCol,
                    boardManagerScript.curPieceHeight,
                    boardManagerScript.curPieceRow
                ] = boardManagerScript.pieceTypeChar;

            boardManagerScript.CheckIfWinner(boardManagerScript.pieceTypeChar);

            // switch to next person's turn
            boardManagerScript.isXTurn = !boardManagerScript.isXTurn;

            if (boardManagerScript.isXTurn && !boardManagerScript.flipUsedX)
            {
                btnFlipBoard.gameObject.SetActive(true);
            }
            else if (!boardManagerScript.isXTurn && !boardManagerScript.flipUsedO)
            {
                btnFlipBoard.gameObject.SetActive(true);
            }

            if (boardManagerScript.pieceTypeChar == 'X')
            {
                boardManagerScript.pieceTypeChar = 'O';
                txtTurn.text = "O's Turn";
            }
            else if (boardManagerScript.pieceTypeChar == 'O')
            {
                boardManagerScript.pieceTypeChar = 'X';
                txtTurn.text = "X's Turn";
            }

            boardManagerScript.currentGamePiece = null;
        }
    }

    void NewGameOnClick()
    {
        txtGameOver.gameObject.SetActive(false);
        btnNewGame.gameObject.SetActive(false);
        txtTurn.text = "X's Turn";
        txtTurn.gameObject.SetActive(true);
        btnSubmit.gameObject.SetActive(true);
        btnFlipBoard.gameObject.SetActive(true);
        btnFlipBoard.GetComponentInChildren<Text>().text = "Flip Board";

        boardManagerScript.ResetBoard();
    }

    void FlipBoardOnClick()
    {
        // toggle flipped condition
        boardManagerScript.flipped = !boardManagerScript.flipped;
        // set flip button text
        btnFlipBoard.GetComponentInChildren<Text>().text = boardManagerScript.flipped ? "Revert" : "Flip Board";
        // hide visible temporary pieces
        if (boardManagerScript.currentGamePiece != null)
        {
            boardManagerScript.currentGamePiece.SetActive(false);
        }
        boardManagerScript.currentGamePiece = null;
        // flip board
        boardManagerScript.FlipBoard();
    }
}
