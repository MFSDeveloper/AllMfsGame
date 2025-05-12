using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class Cell : MonoBehaviour
    {
        private bool _hasMine;
        private Vector2Int _coordinate;
        private Image _image;
        private CellStatus _status;
        private CellTheme _theme;

        [SerializeField] private Image _backgroundImage;
        internal bool IsLightTile { get; private set; }
        private void Awake()
        {
            _image = GetComponentInChildren<Image>();
        }

        internal CellStatus Status
        {
            get => _status;
            set
            {
                _status = value;

                switch (value)
                {
                    case CellStatus.Unknown:
                        _image.sprite = _theme.unknown; // This handles the foreground (number/mine)
                        _backgroundImage.sprite = IsLightTile ? _theme.unknown : _theme.backgroundDarkGreen;
                        break;
                    case CellStatus.Revealed:
                        Reveal();
                        break;

                    case CellStatus.Flag:
                        _image.sprite = IsLightTile ? _theme.flagLight : _theme.flagDark;
                        break;
                    case CellStatus.Exploded:
                        _image.sprite = IsLightTile ? _theme.mineExplodedLight : _theme.mineExplodedDark;
                        break;

                    //default:
                    //    throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }


        internal bool HasMine
        {
            get => _hasMine;
            set => _hasMine = value;
        }

        internal Vector2Int Coordinate
        {
            get => _coordinate;
            set => _coordinate = value;
        }

        internal uint MineCount { get; private set; }

        internal void Init(Vector2Int coordinate, bool hasMine, in CellTheme theme)
        {
            Coordinate = coordinate;
            _hasMine = hasMine;
            _theme = theme;

            Status = CellStatus.Unknown;
            _image.enabled = true;

            IsLightTile = (coordinate.x + coordinate.y) % 2 == 0;

            if (IsLightTile)
            {
                _backgroundImage.sprite = _theme.unknown; // Light background
            }
            else
            {
                _backgroundImage.sprite = _theme.backgroundDarkGreen;
            }
        }


        internal void AddMineCount()
        {
            MineCount++;
        }

        internal void Reveal()
        {
            if (HasMine)
            {
                Status = CellStatus.Exploded;
                return;
            }

            // Here we check whether the tile is light or dark
            if (IsLightTile)
            {
                // If it's a light tile, set the light disarmed sprite
                _backgroundImage.sprite = _theme.disarmedLight; // Assuming you have a disarmedLight sprite
            }
            else
            {
                // If it's a dark tile, set the dark disarmed sprite
                _backgroundImage.sprite = _theme.disarmedDark; // Assuming you have a disarmedDark sprite
            }

            // Handle the MineCount for the revealed tiles
            switch (MineCount)
            {
                case 1:
                    _image.sprite = IsLightTile ? _theme.mine1Light : _theme.mine1Dark;
                    break;
                case 2:
                    _image.sprite = IsLightTile ? _theme.mine2Light : _theme.mine2Dark;
                    break;
                case 3:
                    _image.sprite = IsLightTile ? _theme.mine3Light : _theme.mine3Dark;
                    break;
                case 4:
                    _image.sprite = IsLightTile ? _theme.mine4Light : _theme.mine4Dark;
                    break;
                default:
                    Debug.LogWarning("[Cell] Invalid MineCount");
                    break;
            }

            _status = CellStatus.Revealed;
        }
    }
}