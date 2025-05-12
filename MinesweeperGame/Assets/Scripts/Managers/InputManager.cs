using Core;
using UnityEngine;

namespace Managers
{
    public class InputManager : Singleton<InputManager>
    {
        public bool IsInputEnabled = true;
        private Camera _mainCamera;

        public delegate void ClickCell(Cell cell);
        public static event ClickCell OnLeftClickCell;   // Tap / Left click
        public static event ClickCell OnRightClickCell;  // Long press / Right click

        private float touchTimeThreshold = 0.2f; // You can tweak this for long press timing
        private float touchStartTime;
        private bool longPressTriggered = false;

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (!IsInputEnabled) return;
#if UNITY_EDITOR || UNITY_STANDALONE
            HandleMouseInput();
#else
            HandleTouchInput();
#endif
        }

        // For desktop/testing in editor
        private void HandleMouseInput()
        {
            if (Input.GetMouseButtonUp(0))
            {
                TryRaycast(Input.mousePosition, isRightClick: false);
            }
            if (Input.GetMouseButtonUp(1))
            {
                TryRaycast(Input.mousePosition, isRightClick: true);
            }
        }

        // For mobile devices
        private void HandleTouchInput()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        touchStartTime = Time.time;
                        longPressTriggered = false;
                        break;

                    case TouchPhase.Stationary:
                        if (!longPressTriggered && (Time.time - touchStartTime) > touchTimeThreshold)
                        {
                            longPressTriggered = true;
                            TryRaycast(touch.position, isRightClick: true); // long press = flag
                        }
                        break;

                    case TouchPhase.Ended:
                        if (!longPressTriggered)
                        {
                            TryRaycast(touch.position, isRightClick: false); // tap = reveal
                        }
                        break;
                }
            }
        }

        // Common Raycast logic
        //private void TryRaycast(Vector2 screenPosition, bool isRightClick)
        //{
        //    Ray ray = _mainCamera.ScreenPointToRay(screenPosition);
        //    RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

        //    if (!hit.collider) return;

        //    Cell cell = hit.collider.GetComponent<Cell>();
        //    if (cell)
        //    {
        //        if (isRightClick)
        //            OnRightClickCell?.Invoke(cell);
        //        else
        //            OnLeftClickCell?.Invoke(cell);
        //    }
        //}



        private void TryRaycast(Vector2 screenPosition, bool isRightClick)
        {
            if (_mainCamera == null)
            {
                Debug.LogWarning("[InputManager] MainCamera is null. Possibly scene unloading.");
                return;
            }

            Ray ray = _mainCamera.ScreenPointToRay(screenPosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (!hit.collider) return;

            Cell cell = hit.collider.GetComponent<Cell>();
            if (cell)
            {
                if (isRightClick)
                    OnRightClickCell?.Invoke(cell);
                else
                    OnLeftClickCell?.Invoke(cell);
            }
        }

    }
}



