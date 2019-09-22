using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

    // region encompasses board state for reset between games
    #region
    // placement boxes do not need to be reset
    public GameObject[,,] placementBoxes = new GameObject[3, 3, 3];
    public GameObject[,,] piecesPlacedX = new GameObject[3, 3, 3];
    public GameObject[,,] piecesPlacedO = new GameObject[3, 3, 3];

    // used to hold reference to hide piece if player changes mind before clicking submit move
    public GameObject currentGamePiece;

    // used to store where the temporarily placed piece is located
    // do not necessarily need to be reset for a new game
    public int curPieceCol;
    public int curPieceHeight;
    public int curPieceRow;

    public char[,,] currentBoardLayout = new char[3, 3, 3];
    #endregion

    // checks current board orientation
    public bool flipped;
    public bool flipUsedX;
    public bool flipUsedO;

    float X_SEPARATION = 40;
    float Y_SEPARATION = 25;
    float Z_SEPARATION = 40;
    float placementBoxScaleY = 24;
    float placementBoxScaleXZ = 30;

    public Material placementBoxMaterial;

    public bool isXTurn;
    public bool isGameOver;

    public int[,,] winningCombinations = new int[49, 3, 3] { 
        // YZ plane slice 0
        { { 0, 0, 0 }, { 0, 0, 1 }, { 0, 0, 2 } },
        { { 0, 1, 0 }, { 0, 1, 1 }, { 0, 1, 2 } },
        { { 0, 2, 0 }, { 0, 2, 1 }, { 0, 2, 2 } },
        { { 0, 0, 0 }, { 0, 1, 0 }, { 0, 2, 0 } },
        { { 0, 0, 1 }, { 0, 1, 1 }, { 0, 2, 1 } },
        { { 0, 0, 2 }, { 0, 1, 2 }, { 0, 2, 2 } },
        { { 0, 0, 0 }, { 0, 1, 1 }, { 0, 2, 2 } },
        { { 0, 2, 0 }, { 0, 1, 1 }, { 0, 0, 2 } },
        // YZ plane slice 1
        { { 1, 0, 0 }, { 1, 0, 1 }, { 1, 0, 2 } },
        { { 1, 1, 0 }, { 1, 1, 1 }, { 1, 1, 2 } },
        { { 1, 2, 0 }, { 1, 2, 1 }, { 1, 2, 2 } },
        { { 1, 0, 0 }, { 1, 1, 0 }, { 1, 2, 0 } },
        { { 1, 0, 1 }, { 1, 1, 1 }, { 1, 2, 1 } },
        { { 1, 0, 2 }, { 1, 1, 2 }, { 1, 2, 2 } },
        { { 1, 0, 0 }, { 1, 1, 1 }, { 1, 2, 2 } },
        { { 1, 2, 0 }, { 1, 1, 1 }, { 1, 0, 2 } },
        // YZ plane slice 2
        { { 2, 0, 0 }, { 2, 0, 1 }, { 2, 0, 2 } },
        { { 2, 1, 0 }, { 2, 1, 1 }, { 2, 1, 2 } },
        { { 2, 2, 0 }, { 2, 2, 1 }, { 2, 2, 2 } },
        { { 2, 0, 0 }, { 2, 1, 0 }, { 2, 2, 0 } },
        { { 2, 0, 1 }, { 2, 1, 1 }, { 2, 2, 1 } },
        { { 2, 0, 2 }, { 2, 1, 2 }, { 2, 2, 2 } },
        { { 2, 0, 0 }, { 2, 1, 1 }, { 2, 2, 2 } },
        { { 2, 2, 0 }, { 2, 1, 1 }, { 2, 0, 2 } },
        // XY plane slice 0
        { { 0, 2, 0 }, { 1, 2, 0 }, { 2, 2, 0 } },
        { { 0, 1, 0 }, { 1, 1, 0 }, { 2, 1, 0 } },
        { { 0, 0, 0 }, { 1, 0, 0 }, { 2, 0, 0 } },
        { { 0, 0, 0 }, { 1, 1, 0 }, { 2, 2, 0 } },
        { { 0, 2, 0 }, { 1, 1, 0 }, { 2, 0, 0 } },
        // XY plane slice 1
        { { 0, 2, 1 }, { 1, 2, 1 }, { 2, 2, 1 } },
        { { 0, 1, 1 }, { 1, 1, 1 }, { 2, 1, 1 } },
        { { 0, 0, 1 }, { 1, 0, 1 }, { 2, 0, 1 } },
        { { 0, 0, 1 }, { 1, 1, 1 }, { 2, 2, 1 } },
        { { 0, 2, 1 }, { 1, 1, 1 }, { 2, 0, 1 } },
        // XY plane slice 2
        { { 0, 2, 2 }, { 1, 2, 2 }, { 2, 2, 2 } },
        { { 0, 1, 2 }, { 1, 1, 2 }, { 2, 1, 2 } },
        { { 0, 0, 2 }, { 1, 0, 2 }, { 2, 0, 2 } },
        { { 0, 0, 2 }, { 1, 1, 2 }, { 2, 2, 2 } },
        { { 0, 2, 2 }, { 1, 1, 2 }, { 2, 0, 2 } },
        // XZ plane slice diagonals
        { { 0, 0, 0 }, { 1, 0, 1 }, { 2, 0, 2 } },
        { { 0, 0, 2 }, { 1, 0, 1 }, { 2, 0, 0 } },
        { { 0, 1, 0 }, { 1, 1, 1 }, { 2, 1, 2 } },
        { { 0, 1, 2 }, { 1, 1, 1 }, { 2, 1, 0 } },
        { { 0, 2, 0 }, { 1, 2, 1 }, { 2, 2, 2 } },
        { { 0, 2, 2 }, { 1, 2, 1 }, { 2, 2, 0 } },
        // long diagonals 
        { { 0, 2, 2 }, { 1, 1, 1 }, { 2, 0, 0 } },
        { { 2, 2, 2 }, { 1, 1, 1 }, { 0, 0, 0 } },
        { { 2, 2, 0 }, { 1, 1, 1 }, { 0, 0, 2 } },
        { { 0, 2, 0 }, { 1, 1, 1 }, { 2, 0, 2 } },
    };

    public char pieceTypeChar = 'O';

    public GameObject XPrefab;
    public GameObject OPrefab;


    public UIManager uiManagerScript;

    // Use this for initialization
    void Start () {
        // initialize all placement boxes
		for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                for (int k = 0; k < 3; ++k)
                {
                    placementBoxes[i, j, k] = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    placementBoxes[i, j, k].transform.parent = gameObject.transform;
                    placementBoxes[i, j, k].transform.localPosition = new Vector3(
                        (i * X_SEPARATION) - X_SEPARATION, 
                        (j * Y_SEPARATION) - Y_SEPARATION, 
                        (k * - Z_SEPARATION) + Z_SEPARATION);
                    placementBoxes[i, j, k].transform.localScale = new Vector3(placementBoxScaleXZ, placementBoxScaleY, placementBoxScaleXZ);
                    placementBoxes[i, j, k].GetComponent<MeshRenderer>().material = placementBoxMaterial;
                    placementBoxes[i, j, k].name = "Box" + i + j + k;

                    // every board piece is initialized to E for empty
                    currentBoardLayout[i, j, k] = 'E';
                }
            }
        }

        InstantiatePieces();

        // X player starts
        isXTurn = true;
        isGameOver = false;

        // holds reference to temporarily placed piece
        currentGamePiece = null;

    }

    void InstantiatePieces()
    {
        for (int col = 0; col < 3; ++col)
        {
            for (int height = 0; height < 3; ++height)
            {
                for (int row = 0; row < 3; ++row)
                {
                    // instantiate all X prefabs
                    piecesPlacedX[col, height, row] = Instantiate(XPrefab, new Vector3(
                        placementBoxes[col, height, row].transform.position.x,
                        placementBoxes[col, height, row].transform.position.y,
                        placementBoxes[col, height, row].transform.position.z),
                        gameObject.transform.localRotation,
                        placementBoxes[col, height, row].transform);
                    // hide X prefab
                    piecesPlacedX[col, height, row].SetActive(false);
                    // instantiate all O prefabs
                    piecesPlacedO[col, height, row] = Instantiate(OPrefab, new Vector3(
                        placementBoxes[col, height, row].transform.position.x,
                        placementBoxes[col, height, row].transform.position.y,
                        placementBoxes[col, height, row].transform.position.z),
                        gameObject.transform.localRotation,
                        placementBoxes[col, height, row].transform);
                    // hide O prefab
                    piecesPlacedO[col, height, row].SetActive(false);
                }
            }
        }
    }

    public void CheckIfWinner(char pieceTypePlaced)
    {
        int X0index = -1;
        int Y0index = -1;
        int Z0index = -1;

        int X1index = -1;
        int Y1index = -1;
        int Z1index = -1;

        int X2index = -1;
        int Y2index = -1;
        int Z2index = -1;

        for (int i = 0; i < 49; ++i)
        {
            X0index = winningCombinations[i, 0, 0];
            Y0index = winningCombinations[i, 0, 1];
            Z0index = winningCombinations[i, 0, 2];

            X1index = winningCombinations[i, 1, 0];
            Y1index = winningCombinations[i, 1, 1];
            Z1index = winningCombinations[i, 1, 2];

            X2index = winningCombinations[i, 2, 0];
            Y2index = winningCombinations[i, 2, 1];
            Z2index = winningCombinations[i, 2, 2];

            if (currentBoardLayout[X0index, Y0index, Z0index] == pieceTypePlaced
                && currentBoardLayout[X1index, Y1index, Z1index] == pieceTypePlaced
                && currentBoardLayout[X2index, Y2index, Z2index] == pieceTypePlaced)
            {
                HandleGameOver(pieceTypePlaced);
            }
        }
    }

    public void CheckIfFlippedWinner()
    {
        bool XWon = false;
        bool OWon = false;

        int X0index = -1;
        int Y0index = -1;
        int Z0index = -1;

        int X1index = -1;
        int Y1index = -1;
        int Z1index = -1;

        int X2index = -1;
        int Y2index = -1;
        int Z2index = -1;

        for (int i = 0; i < 49; ++i)
        {
            X0index = winningCombinations[i, 0, 0];
            Y0index = winningCombinations[i, 0, 1];
            Z0index = winningCombinations[i, 0, 2];

            X1index = winningCombinations[i, 1, 0];
            Y1index = winningCombinations[i, 1, 1];
            Z1index = winningCombinations[i, 1, 2];

            X2index = winningCombinations[i, 2, 0];
            Y2index = winningCombinations[i, 2, 1];
            Z2index = winningCombinations[i, 2, 2];

            if (currentBoardLayout[X0index, Y0index, Z0index] == 'X'
                && currentBoardLayout[X1index, Y1index, Z1index] == 'X'
                && currentBoardLayout[X2index, Y2index, Z2index] == 'X')
            {
                XWon = true;
            }
            if (currentBoardLayout[X0index, Y0index, Z0index] == 'O'
                && currentBoardLayout[X1index, Y1index, Z1index] == 'O'
                && currentBoardLayout[X2index, Y2index, Z2index] == 'O')
            {
                OWon = true;
            }
        }

        if (XWon && OWon)
        {
            HandleDraw();
        }
        else if (XWon)
        {
            HandleGameOver('X');
        }
        else if (OWon)
        {
            HandleGameOver('O');
        }
    }

    void HandleDraw()
    {
        isGameOver = true;
        uiManagerScript.txtGameOver.text = "It's a Draw";
        uiManagerScript.txtGameOver.gameObject.SetActive(true);
        uiManagerScript.btnNewGame.gameObject.SetActive(true);
        uiManagerScript.txtTurn.gameObject.SetActive(false);
        uiManagerScript.btnSubmit.gameObject.SetActive(false);
        uiManagerScript.btnFlipBoard.gameObject.SetActive(false);
    }

    void HandleGameOver(char winner)
    {
        isGameOver = true;
        uiManagerScript.btnFlipBoard.gameObject.SetActive(false);
        uiManagerScript.txtGameOver.text = winner + " Won!";
        uiManagerScript.txtGameOver.gameObject.SetActive(true);
        uiManagerScript.btnNewGame.gameObject.SetActive(true);
        uiManagerScript.txtTurn.gameObject.SetActive(false);
        uiManagerScript.btnSubmit.gameObject.SetActive(false);
    }

    public void ResetBoard()
    {
        isXTurn = true;
        isGameOver = false;
        currentGamePiece = null;

        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                for (int k = 0; k < 3; ++k)
                {
                    // hide all X and O prefabs
                    piecesPlacedX[i, j, k].SetActive(false);
                    piecesPlacedO[i, j, k].SetActive(false);

                    // every board piece is initialized to E for empty
                    currentBoardLayout[i, j, k] = 'E';
                }
            }
        }
    }

    public void FlipBoard()
    {
        char[,,] tmpBoardLayout = currentBoardLayout;

        // flip pieces by iterating through pegs
        for (int col = 0; col < 3; ++col)
        {
            for (int row = 0; row < 3; ++row)
            {
                // if there are three pieces on a given peg, swap the top and the bottom
                if (currentBoardLayout[col, 0, row] != 'E' &&
                    currentBoardLayout[col, 1, row] != 'E' &&
                    currentBoardLayout[col, 2, row] != 'E')
                {
                    tmpBoardLayout[col, 0, row] = currentBoardLayout[col, 2, row];
                    tmpBoardLayout[col, 2, row] = currentBoardLayout[col, 0, row];
                }
                // if there are two pieces on a given peg, swap them
                else if (currentBoardLayout[col, 0, row] != 'E' &&
                    currentBoardLayout[col, 1, row] != 'E')
                {
                    tmpBoardLayout[col, 0, row] = currentBoardLayout[col, 1, row];
                    tmpBoardLayout[col, 1, row] = currentBoardLayout[col, 0, row];
                }
                // if there is one piece on a given peg, do nothing
                else if (currentBoardLayout[col, 0, row] != 'E')
                {
                    // leave it where it is
                }
            }
        }

        // swap pegs (col,row) 00->02, 10->12, 20->22
        // create copies of pegs 00, 10, 20
        char[] tmpPeg00 = { tmpBoardLayout[0, 0, 0], tmpBoardLayout[0, 1, 0], tmpBoardLayout[0, 2, 0] };
        char[] tmpPeg10 = { tmpBoardLayout[1, 0, 0], tmpBoardLayout[1, 1, 0], tmpBoardLayout[1, 2, 0] };
        char[] tmpPeg20 = { tmpBoardLayout[2, 0, 0], tmpBoardLayout[2, 1, 0], tmpBoardLayout[2, 2, 0] };
        // move peg 02->00
        tmpBoardLayout[0, 0, 0] = tmpBoardLayout[0, 0, 2];
        tmpBoardLayout[0, 1, 0] = tmpBoardLayout[0, 1, 2];
        tmpBoardLayout[0, 2, 0] = tmpBoardLayout[0, 2, 2];
        // move peg 12->10
        tmpBoardLayout[1, 0, 0] = tmpBoardLayout[1, 0, 2];
        tmpBoardLayout[1, 1, 0] = tmpBoardLayout[1, 1, 2];
        tmpBoardLayout[1, 2, 0] = tmpBoardLayout[1, 2, 2];
        // move peg 22->20
        tmpBoardLayout[2, 0, 0] = tmpBoardLayout[2, 0, 2];
        tmpBoardLayout[2, 1, 0] = tmpBoardLayout[2, 1, 2];
        tmpBoardLayout[2, 2, 0] = tmpBoardLayout[2, 2, 2];
        // move peg 00->02
        tmpBoardLayout[0, 0, 2] = tmpPeg00[0];
        tmpBoardLayout[0, 1, 2] = tmpPeg00[1];
        tmpBoardLayout[0, 2, 2] = tmpPeg00[2];
        // move peg 10->12
        tmpBoardLayout[1, 0, 2] = tmpPeg10[0];
        tmpBoardLayout[1, 1, 2] = tmpPeg10[1];
        tmpBoardLayout[1, 2, 2] = tmpPeg10[2];
        // move peg 20->22
        tmpBoardLayout[2, 0, 2] = tmpPeg20[0];
        tmpBoardLayout[2, 1, 2] = tmpPeg20[1];
        tmpBoardLayout[2, 2, 2] = tmpPeg20[2];

        currentBoardLayout = tmpBoardLayout;

        UpdatePieceVisibility();
    }

    void UpdatePieceVisibility()
    {
        HideAllPieces();
        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                for (int k = 0; k < 3; ++k)
                {
                    if (currentBoardLayout[i,j,k] == 'X')
                    {
                        piecesPlacedX[i, j, k].SetActive(true);
                    }
                    else if (currentBoardLayout[i, j, k] == 'O')
                    {
                        piecesPlacedO[i, j, k].SetActive(true);
                    }
                }
            }
        }
    }

    void HideAllPieces()
    {
        for (int i = 0; i < 3; ++i)
        {
            for (int j = 0; j < 3; ++j)
            {
                for (int k = 0; k < 3; ++k)
                {
                    piecesPlacedX[i, j, k].SetActive(false);
                    piecesPlacedO[i, j, k].SetActive(false);
                }
            }
        }
    }
}
