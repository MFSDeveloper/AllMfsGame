using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UI.Text;
using Text = UnityEngine.UI.Text;
using static System.Net.Mime.MediaTypeNames;
using System;


public class SudokuManager : MonoBehaviour
{
    public Text[] gridTextObjects;
    public Text[,] gridTexts = new Text[9, 9]; // UI Cells
    private int[,] grid = new int[9, 9]; // Sudoku Numbers
    private Stack<Move> moveStack = new Stack<Move>(); // Undo Stack
    public Button submitButton; // Submit Button
    private int[,] correctSolution = new int[9, 9]; // Correct Sudoku Solution
    public GameObject winMenu;

    private int selectedRow = -1;
    private int selectedCol = -1;
    private int selectedNumber = 0;

    public static SudokuManager Instance;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
    }

    void Start()
    {
        //submitButton.interactable = false; // Disable submit at start
        winMenu.SetActive(false);

        if (gridTextObjects.Length == 81)
        {
            AssignGridTextObjects();
        }
        else
        {
            //Debug.LogWarning("⚠ UI Text elements are NOT assigned manually! Trying to auto-assign...");
            AutoAssignGridTextObjects();
        }

        UpdateGridUI();
        CheckIfAllFilled();
    }

    void AssignGridTextObjects()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                gridTexts[i, j] = gridTextObjects[i * 9 + j];
            }
        }
    }

    void AutoAssignGridTextObjects()
    {
        Text[] allTexts = FindObjectsOfType<Text>();

        if (allTexts.Length < 81)
        {
            Debug.LogError($"❌ Auto-assignment failed! Found {allTexts.Length} text elements, expected at least 81.");
            return;
        }

        gridTextObjects = new Text[81];

        System.Array.Sort(allTexts, (a, b) => a.transform.position.y.CompareTo(b.transform.position.y));

        for (int i = 0; i < 81; i++)
        {
            gridTextObjects[i] = allTexts[i];
        }

        Debug.Log("✅ Auto-assignment successful! All 81 Sudoku cells assigned.");
    }

    public void SetCorrectSolution(int[,] solution)
    {
        correctSolution = solution;
    }

    public void OnCellClick(int raw, int columun, int number, SudokuCell cell)
    {
        Debug.Log("✅ Cell Clicked!");

        PlaceNumber(raw, columun, number, cell);
    }

    public void PlaceNumber(int row, int col, int number, SudokuCell cell)
    {
        if (grid[row, col] == 0)  // Only allow placing if the cell is empty
        {
            moveStack.Push(new Move(row, col, grid[row, col], cell)); // Store previous state
            grid[row, col] = number;
            //gridTexts[row, col].text = number.ToString(); // Update UI immediately
            Debug.Log($"📌 Move Stored: {row}, {col} → {number}");
        }
    }




    public void Undo()
    {
        if (moveStack.Count > 0) // Check if there are moves to undo
        {
            Move lastMove = moveStack.Pop(); // Get last move
            grid[lastMove.Row, lastMove.Col] = lastMove.PreviousNumber; // Restore number
            lastMove.PreviousCell.UpdateValue(lastMove.PreviousNumber);
            //gridTexts[lastMove.Row, lastMove.Col].text = lastMove.PreviousNumber == 0 ? "" : lastMove.PreviousNumber.ToString(); // Update UI

            Debug.Log($"🔄 Undo: {lastMove.Row}, {lastMove.Col} → {lastMove.PreviousNumber}");
        }
        else
        {
            Debug.Log("⚠ Cannot undo! No moves in history.");
        }
    }




    public void Erase(int row, int col, SudokuCell sudokuCell)
    {
        if (grid[row, col] != 0)
        {
            moveStack.Push(new Move(row, col, grid[row, col], sudokuCell));
            grid[row, col] = 0;
            UpdateGridUI();
            CheckIfAllFilled();
        }
    }

    void CheckIfAllFilled()
    {
        bool allFilled = true;

        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (grid[row, col] == 0) // If any cell is empty, disable submit
                {
                    allFilled = true;
                    submitButton.interactable = false;
                    return;
                }
            }
        }

        submitButton.interactable = allFilled; // Enable submit if all are filled
    }

    public void ValidateSudoku()
    {
        bool isCorrect = true;

        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (grid[row, col] != correctSolution[row, col])
                {
                    gridTexts[row, col].color = Color.red; // Highlight wrong cells
                    isCorrect = false;
                }
            }
        }

        if (isCorrect)
        {
            Debug.Log("✅ Congratulations! Sudoku Solved Correctly.");
            // Show win menu
            winMenu.SetActive(true);
        }
        else
        {
            Debug.Log("❌ Incorrect! Check the highlighted cells.");
        }
    }

    private void UpdateGridUI()
    {
        for (int row = 0; row < 9; row++)
        {
            for (int col = 0; col < 9; col++)
            {
                if (gridTexts[row, col] = null)
                {
                    gridTexts[row, col].text = grid[row, col] == 0 ? "" : grid[row, col].ToString();
                }
            }
        }
        CheckIfAllFilled(); // Re-check after updating UI
    }
}

public class Move
{
    public int Row { get; }
    public int Col { get; }
    public int PreviousNumber { get; }

    public SudokuCell PreviousCell { get; }

    public Move(int row, int col, int previousNumber, SudokuCell previousCell)
    {
        Row = row;
        Col = col;
        PreviousNumber = previousNumber;
        PreviousCell = previousCell;
    }
}
