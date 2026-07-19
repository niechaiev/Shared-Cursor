using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Radial
{
    public class RadialMenuItemInput : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private RadialMenu _radialMenu;
        private RadialMenuItem _radialMenuItem;
        private RectTransform _rect;
        private Image _backgroundImage;

        private Vector3 _baseScale;
        private Vector3 _targetScale;

        [SerializeField] private float hoverSmooth = 10f;

        private void Awake()
        {
            _radialMenu = GetComponentInParent<RadialMenu>();
            _radialMenuItem = GetComponent<RadialMenuItem>();
            _rect = GetComponent<RectTransform>();
            _backgroundImage = GetComponent<Image>();

            _baseScale = _rect.localScale;
            _targetScale = _baseScale;
        }

        private void Update()
        {
            _rect.localScale = Vector3.Lerp(_rect.localScale, _targetScale, hoverSmooth * Time.unscaledDeltaTime);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            _targetScale = _baseScale * _radialMenu.HoverScale;
            if (_backgroundImage) _backgroundImage.color = _radialMenu.HoverColor;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _targetScale = _baseScale;
            if (_backgroundImage) _backgroundImage.color = _radialMenu.DefaultColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_radialMenu || !_radialMenu.IsOpen) return;

            _radialMenuItem?.Invoke();
        }
    }
}
