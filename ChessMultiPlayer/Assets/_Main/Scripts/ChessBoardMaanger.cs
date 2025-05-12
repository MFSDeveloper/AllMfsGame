using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessBoardMaanger : MonoBehaviour
{
    public static ChessBoardMaanger Instance;

    private void Awake()
    {
        Instance = this;
    }

    public Vector3 GetTileCenter(int x, int y)
    {
        // Your logic to get center position of tile
        return new Vector3(x, 0, y); // Example placeholder
    }
}
