using Solitaire.Services;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Solitaire.Presenters
{
    public class GameInfoPresenter : OrientationAwarePresenter
    {
        [SerializeField] private TextMeshProUGUI _labelPoints;
        [SerializeField] private TextMeshProUGUI _labelMoves;
        [SerializeField] private TextMeshProUGUI _labelTime;
        //[SerializeField] private RectTransform _rectPoints;
        //[SerializeField] private RectTransform _rectTime;
        //[SerializeField] private RectTransform _rectMoves;
        [Inject] private readonly IMovesService _movesService;

        [Inject] private readonly IPointsService _pointsService;

        protected override void Start()
        {
            base.Start();

            _pointsService.Points.SubscribeToText(_labelPoints).AddTo(this);
            _movesService.Moves.SubscribeToText(_labelMoves).AddTo(this);
        }

        protected override void OnOrientationChanged(bool isLandscape)
        {
            //if (Application.platform == RuntimePlatform.Android)
            //{
            //    _rectPoints.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, isLandscape ? 60 : 10, _rectPoints.sizeDelta.x);
            //    _rectPoints.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, isLandscape ? 60 : 10, _rectPoints.sizeDelta.y);

            //    _rectTime.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, isLandscape ? 60 : 20, _rectTime.sizeDelta.x);
            //    _rectTime.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, isLandscape ? 40 : 10, _rectTime.sizeDelta.y);

            //    _rectMoves.anchorMin = isLandscape ? Vector2.zero : Vector2.one;
            //    _rectMoves.anchorMax = isLandscape ? Vector2.zero : Vector2.one;
            //    _rectMoves.pivot = isLandscape ? Vector2.zero : Vector2.one;
            //    _rectMoves.SetInsetAndSizeFromParentEdge(
            //        isLandscape ? RectTransform.Edge.Left : RectTransform.Edge.Right,
            //        isLandscape ? 80 : 10,
            //        _rectMoves.sizeDelta.x
            //    );
            //    _rectMoves.SetInsetAndSizeFromParentEdge(
            //        isLandscape ? RectTransform.Edge.Bottom : RectTransform.Edge.Top,
            //        isLandscape ? 140 : 20,
            //        _rectMoves.sizeDelta.y
            //    );
            //}
        }

    }
}