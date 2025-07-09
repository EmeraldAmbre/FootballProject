using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FootballProject.UI
{
    public class MatchStartUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject panel;
        [SerializeField] private TextMeshProUGUI buttonText;
        [SerializeField] private Button button;

        public event Action OnStartRequested;

        public void Show(string label)
        {
            panel.SetActive(true);
            buttonText.text = label;
        }

        /// <summary>
        /// Méthode à appeler via l'OnClick du bouton dans l'inspecteur.
        /// </summary>
        public void OnButtonPressed()
        {
            StartCoroutine(StartButtonRoutine());
        }

        private IEnumerator StartButtonRoutine()
        {
            panel.SetActive(false);
            yield return new WaitForSeconds(5f);
            OnStartRequested?.Invoke();
        }
    }
}
