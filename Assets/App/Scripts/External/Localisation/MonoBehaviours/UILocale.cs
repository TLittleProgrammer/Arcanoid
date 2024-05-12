using TMPro;
using UnityEngine;

namespace App.Scripts.External.Localisation.MonoBehaviours
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

        public void SetToken(string token)
        {
            LocaleKey = token;
        }

        public void SetText(string text)
        {
            _text.text = text;
        }
    }
}