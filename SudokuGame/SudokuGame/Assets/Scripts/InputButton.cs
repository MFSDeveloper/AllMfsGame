using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputButton : MonoBehaviour
{
    public static InputButton instance;
    SudokuCell lastCell;
    [SerializeField] GameObject wrongText;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        wrongText.SetActive(false);
        this.gameObject.SetActive(true);
    }

    public void ActivateInputButton(SudokuCell cell)
    {
        this.gameObject.SetActive(true);
        lastCell= cell;
    }

    public void ClickedButton(int num)
    {
        if (lastCell != null)
        {
            lastCell.UpdateValue(num);
            SudokuManager.Instance.OnCellClick(lastCell.row, lastCell.col, lastCell.value, lastCell);
        }
        wrongText.SetActive(false);
        this.gameObject.SetActive(true);
    }
}
