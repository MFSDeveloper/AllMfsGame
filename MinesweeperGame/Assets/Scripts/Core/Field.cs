using System.Collections.Generic;
using System.Linq;
using Managers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core
{
    public class Field : Singleton<Field>
    {
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private Transform cellHolder;
        [SerializeField] private CellTheme cellTheme;

        private Cell[,] _cells;
        private List<Cell> _mineCells;
        private const float TileSize = 1f;
        private FieldSetup _setup;

        private bool _isGameOver = false;

        public delegate void GameEvent();
        public static event GameEvent OnGameStart;
        public static event GameEvent OnGameLost;
        public static event GameEvent OnGameWon;

        public delegate void UpdateFieldSetup(FieldSetup setup);
        public static event UpdateFieldSetup OnUpdateFieldSetup;

        private void Start()
        {
            InputManager.OnLeftClickCell += HandleLeftClickCell;
            InputManager.OnRightClickCell += CycleFlagStatus;
        }

        private void OnDestroy()
        {
            InputManager.OnLeftClickCell -= HandleLeftClickCell;
            InputManager.OnRightClickCell -= CycleFlagStatus;
        }

        private void HandleLeftClickCell(Cell cell)
        {
            TryReveal(cell);
        }

        private void TryReveal(Cell cell)
        {
            if (_isGameOver || cell.Status != CellStatus.Unknown) return;

            if (cell.HasMine)
            {
                cell.Reveal();
                GameIsLost();
                return;
            }

            cell.Reveal();
            foreach (Cell neighbor in GetAllUnknownNeighbors(cell))
            {
                if (neighbor.HasMine) continue;
                if (neighbor.MineCount == 0)
                {
                    TryReveal(neighbor);
                }
                else
                {
                    neighbor.Reveal();
                }
            }

            if (IsGameWon())
            {
                Debug.Log("[Field] You won!");
            }
        }

        private void CycleFlagStatus(Cell cell)
        {
            if (_isGameOver) return;

            var status = cell.Status;
            switch (status)
            {
                case CellStatus.Unknown:
                    cell.Status = CellStatus.Flag;
                    if (IsGameWon()) return;
                    break;

                case CellStatus.Flag:
                    cell.Status = CellStatus.Unknown;
                    break;
            }
        }

        private void GameIsLost()
        {
            Debug.Log("[Field] GameOver");
            _isGameOver = true;
            CheckWrongPlacedFlags();
            CheckMinesNotFlagged();
            OnGameLost?.Invoke();
        }

        private void CheckWrongPlacedFlags()
        {
            foreach (var cell in _cells)
            {
                if (cell.Status == CellStatus.Flag && !cell.HasMine)
                {
                    cell.Status = CellStatus.FlagWrong;
                }
            }
        }

        private void CheckMinesNotFlagged()
        {
            foreach (var mineCell in _mineCells.Where(mineCell => mineCell.Status == CellStatus.Unknown))
            {
                mineCell.Status = CellStatus.Exploded;
            }
        }

        private void PurgeField()
        {
            if (_cells != null)
            {
                foreach (Transform child in cellHolder.transform)
                {
                    Destroy(child.gameObject);
                }
            }

            _cells = null;
            _mineCells = null;
        }

        public void Configure(GameObject prefab, Transform holder, CellTheme theme)
        {
            cellPrefab = prefab;
            cellHolder = holder;
            cellTheme = theme;
        }


        public void Init(FieldSetup fieldSetup)
        {
            // ✅ Purge previous state
            PurgeField();
            _isGameOver = false;
            _setup = fieldSetup;

            Vector2 offset = new Vector2(
                fieldSetup.size.x / 2f - TileSize / 2f,
                fieldSetup.size.y / 2f - TileSize / 2f
            );

            _cells = new Cell[fieldSetup.size.x, fieldSetup.size.y];

            for (int i = 0; i < fieldSetup.size.x; i++)
            {
                for (int j = 0; j < fieldSetup.size.y; j++)
                {
                    GameObject newGameObject = Instantiate(cellPrefab, cellHolder);
                    Cell newCell = newGameObject.GetComponent<Cell>();

                    newGameObject.name = $"Cell {i},{j}";
                    newGameObject.transform.position = new Vector3(i - offset.x, j - offset.y, 0);
                    newCell.Init(new Vector2Int(i, j), false, in cellTheme);
                    _cells[i, j] = newCell;
                }
            }

            _mineCells = new List<Cell>();
            for (uint i = 0; i < fieldSetup.mines; i++)
            {
                Vector2Int randomPos = new Vector2Int(
                    Random.Range(0, fieldSetup.size.x),
                    Random.Range(0, fieldSetup.size.y)
                );

                if (!_cells[randomPos.x, randomPos.y].HasMine)
                {
                    _cells[randomPos.x, randomPos.y].HasMine = true;
                    _mineCells.Add(_cells[randomPos.x, randomPos.y]);
                }
                else
                {
                    i--;
                }
            }

            foreach (Cell mineCell in _mineCells)
            {
                AddMineCountToNeighbors(mineCell.Coordinate);
            }

            OnUpdateFieldSetup?.Invoke(_setup);
            OnGameStart?.Invoke();
        }

        private void AddMineCountToNeighbors(Vector2Int coordinate)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int nx = coordinate.x + i;
                    int ny = coordinate.y + j;

                    if (nx >= 0 && nx < _setup.size.x && ny >= 0 && ny < _setup.size.y && (i != 0 || j != 0))
                    {
                        _cells[nx, ny].AddMineCount();
                    }
                }
            }
        }

        private IEnumerable<Cell> GetAllUnknownNeighbors(Cell cell)
        {
            var neighbors = new List<Cell>();
            Vector2Int coord = cell.Coordinate;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int nx = coord.x + i;
                    int ny = coord.y + j;

                    if (nx >= 0 && nx < _setup.size.x && ny >= 0 && ny < _setup.size.y && (i != 0 || j != 0))
                    {
                        var neighbor = _cells[nx, ny];
                        if (neighbor.Status == CellStatus.Unknown)
                        {
                            neighbors.Add(neighbor);
                        }
                    }
                }
            }

            return neighbors;
        }

        private bool IsGameWon()
        {
            foreach (var cell in _cells)
            {
                if (cell.Status == CellStatus.Unknown || (cell.Status == CellStatus.Flag && !cell.HasMine))
                {
                    return false;
                }
            }

            _isGameOver = true;
            OnGameWon?.Invoke();
            return true;
        }
    }


}