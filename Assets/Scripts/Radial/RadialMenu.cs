using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Radial
{
    public class RadialMenu : MonoBehaviour
    {
        [Header("Layout")]
        [SerializeField] private float radius = 150f;
        [SerializeField] private float startAngle = 90f;
        [SerializeField] private RadialMenuItem radialMenuItemPrefab;
        private readonly List<RadialMenuItem> _items = new();

        [Header("Button Design")]
        [SerializeField] private Sprite buttonBackgroundSprite;
        [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private Color hoverColor = Color.cyan;
        [SerializeField] private float hoverScale = 1.15f;

        [Header("Open & Close")]
        [SerializeField] private float openCloseDuration = 0.2f;
        [SerializeField] private AnimationCurve openCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private GameObject radialMenuObj;
        [SerializeField] private InputActionReference emoteAction;

        [Header("Data")]
        [SerializeField] private EmoteSO emoteSo;

        //[Header("Selected")]
        public Action OnSelect;

        private CanvasGroup _canvasGroup;
        private Coroutine _openCloseCoroutine;

        public bool IsOpen { get; private set; }
        public Color DefaultColor => defaultColor;
        public Color HoverColor => hoverColor;
        public float HoverScale => hoverScale;

        public EmotePlayer emotePlayer;

        private void Awake()
        {
            if (!emoteSo) return;
            for (var i = 0; i < emoteSo.emotes.Length; i++)
            {
                var emote = emoteSo.emotes[i];
                var item = Instantiate(radialMenuItemPrefab, transform);
                var i1 = i;
                item.Setup(emote, () => StartCoroutine(emotePlayer.PlayEmote(emote.clip, emote.isTwoHanded)));
                _items.Add(item);
            }

            _canvasGroup = GetComponent<CanvasGroup>();
            if(!_canvasGroup) _canvasGroup = gameObject.AddComponent<CanvasGroup>();

            Close();
        }

        private void OnEnable()
        {
            emoteAction.action.performed += Open;
            emoteAction.action.canceled += SelectAndClose;
        }

        private void OnDisable()
        {
            emoteAction.action.performed -= Open;
            emoteAction.action.canceled -= SelectAndClose;
        }

        private void Update()
        {
            UpdateLayout();
        }

        private void UpdateLayout()
        {
            var count = _items.Count;
            if(count == 0) return;

            var step = 360f / count;

            for (var i = 0; i < count; i++)
            {
                if(!_items[i]) continue;

                var rect = _items[i].GetComponent<RectTransform>();

                var angle = startAngle - step * i;
                var rad = angle * Mathf.Deg2Rad;

                rect.anchoredPosition = new Vector2(Mathf.Cos(rad) * radius, Mathf.Sin(rad) * radius);
            }
        }

        private void SetInteractable(bool value)
        {
            _canvasGroup.interactable = value;
            _canvasGroup.blocksRaycasts = value;
        }

        private void SetVisualState(float v)
        {
            _canvasGroup.alpha = v;
            if (radialMenuObj != null)
                radialMenuObj.transform.localScale = Vector3.one * v;
        }

        private IEnumerator OpenCloseCoroutine(float from, float to)
        {
            var t = 0f;
            while (t < 1f)
            {
                t += Time.unscaledDeltaTime / openCloseDuration;
                var v = Mathf.Lerp(from, to, openCurve.Evaluate(t));
                SetVisualState(v);
                yield return null;
            }
            SetVisualState(to);
        }

        private void StartOpenClose(float from, float to)
        {
            if (_openCloseCoroutine != null)
            {
                StopCoroutine(_openCloseCoroutine);
            }

            _openCloseCoroutine = StartCoroutine(OpenCloseCoroutine(from, to));
        }

        private void Open(InputAction.CallbackContext _)
        {
            IsOpen = true;
            SetInteractable(true);

            StartOpenClose(0f, 1f);
        }

        private void SelectAndClose(InputAction.CallbackContext _)
        {
            OnSelect?.Invoke();
            Close();
        }

        private void Close()
        {
            IsOpen = false;
            SetInteractable(false);

            StartOpenClose(1f, 0f);
        }
    }
}
