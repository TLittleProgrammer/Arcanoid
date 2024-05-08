using TMPro;
using UnityEngine;

namespace External.Localisation.MonoBehaviours
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UILocale : MonoBehaviour
    {
        [SerializeField] private string LocaleKey;

        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        public void SetText(string text)
        {
            _text.text = text;
        }
    }
}