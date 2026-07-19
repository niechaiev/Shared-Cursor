using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Radial
{
    public class RadialMenuItem : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image iconImage;
        [SerializeField] private Sprite iconSprite;
        [SerializeField] private TMP_Text title;

        private event Action OnClick;

        private void EnsureReferences()
        {
            if (!backgroundImage) backgroundImage = GetComponent<Image>();
            if (!iconImage) iconImage = GetComponentInChildren<Image>();
            if (!title) title = GetComponentInChildren<TMP_Text>();
        }

        private void ApplyIcon()
        {
            if (iconImage)
                iconImage.sprite = iconSprite;
        }

        public void Invoke()
        {
            OnClick?.Invoke();
        }

        private void OnEnable()
        {
            EnsureReferences();
            ApplyIcon();
        }

#if UNITY_EDITOR
        private void OnValidate() => OnEnable();
#endif

        public void Setup(EmoteSO.Emote emote, Action onClick)
        {
            title.text = emote.title;
            OnClick -= onClick;
            OnClick += onClick;
        }
    }
}
