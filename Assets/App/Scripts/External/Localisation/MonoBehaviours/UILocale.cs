using TMPro;
using UnityEngine;

namespace App.Scripts.External.Localisation.MonoBehaviours
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UILocale : MonoBehaviour
    {
        [SerializeField] private string LocaleKey;
        [SerializeField] private TMP_Text _text;

        public TMP_Text Text => _text;

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