using System.Collections.Generic;
using UnityEngine;

public class MoveIndicatorManager : MonoBehaviour
{
    public GameObject moveIndicatorPrefab; // Red dot prefab
    private List<GameObject> activeIndicators = new List<GameObject>();

    public static MoveIndicatorManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowPossibleMoves(List<Vector2Int> moves)
    {
        ClearIndicators();

        foreach (Vector2Int move in moves)
        {
            Vector3 position = ChessBoardMaanger.Instance.GetTileCenter(move.x, move.y);
            GameObject indicator = Instantiate(moveIndicatorPrefab, position, Quaternion.identity);
            indicator.transform.SetParent(transform); // Keep hierarchy clean
            activeIndicators.Add(indicator);
        }
    }

    public void ClearIndicators()
    {
        foreach (var obj in activeIndicators)
        {
            Destroy(obj);
        }
        activeIndicators.Clear();
    }
}
