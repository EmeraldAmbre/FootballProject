using FootballProject.AI;
using FootballProject.Audio;
using FootballProject.Events;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
namespace FootballProject
{
    public class MatchManager : MonoBehaviour
    {
        [Header("Durée des mi-temps (en secondes)")]
        public float matchDuration = 150f; // 2700f = 45min

        [Header("Références")]
        [SerializeField] TextMeshProUGUI timerText;
        [SerializeField] GameObject[] teamAPlayers;
        [SerializeField] GameObject[] teamBPlayers;
        [SerializeField] AIBallChaser chaser;
        [SerializeField] AISupport[] supportAIs;

        private float currentTime = 0f;
        private bool isPaused = false;
        private bool matchRunning = false;

        Vector3[] teamAStartPositions;
        Quaternion[] teamAStartRotations;
        Vector3[] teamBStartPositions;
        Quaternion[] teamBStartRotations;

        private UI.MatchStartUI matchStartUI;

        #region Unity API
        private void Start()
        {
            SetStartPositions();
            SetPlayerControl(false);
            matchStartUI = GetComponent<UI.MatchStartUI>();
            matchStartUI.Show("Commencer");
            matchStartUI.OnStartRequested += () => StartCoroutine(MatchFlow());
        }
        private void Update()
        {
            if (matchRunning && !isPaused)
            {
                currentTime += Time.deltaTime;
                UpdateTimerUI();
            }
        }
        private void OnDisable()
        {
            matchStartUI.OnStartRequested -= () => StartCoroutine(MatchFlow());
            matchStartUI.OnStartRequested -= () => StartCoroutine(RestartMatchSequence());
        }
        #endregion

        #region Private Methods
        private void UpdateTimerUI()
        {
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
        private IEnumerator MatchFlow()
        {
            yield return StartCoroutine(StartMatch());
            EndMatch();
        }
        private IEnumerator StartMatch()
        {
            MatchAudio.Current.audioManager.Play("RefereeLongWhistle", 0.15f);
            SetPlayerControl(true);
            chaser.SetActiveAI(true);
            foreach (var ai in supportAIs){ ai.SetActiveAI(true); }
            currentTime = 0f;
            matchRunning = true;

            while (currentTime < matchDuration)
            {
                yield return null;
            }

            matchRunning = false;
        }
        private void EndMatch()
        {
            Debug.Log("Match terminé !");
            chaser.SetActiveAI(false);
            foreach (var ai in supportAIs) { ai.SetActiveAI(false); }
            timerText.text = "05:00";
            matchStartUI.Show("Rejouer?");
            matchStartUI.OnStartRequested += () => StartCoroutine(RestartMatchSequence());
        }
        private IEnumerator RestartMatchSequence()
        {
            currentTime = 0f;
            matchRunning = false;
            isPaused = false;

            ResetPlayers();

            yield return new WaitForSeconds(5f);
            yield return StartCoroutine(MatchFlow());
        }
        private void ResetPlayers()
        {
            for (int i = 0; i < teamAPlayers.Length; i++)
            {
                teamAPlayers[i].transform.position = teamAStartPositions[i];
                teamAPlayers[i].transform.rotation = teamAStartRotations[i];
            }
            for (int i = 0; i < teamBPlayers.Length; i++)
            {
                teamBPlayers[i].transform.position = teamBStartPositions[i];
                teamBPlayers[i].transform.rotation = teamBStartRotations[i];
            }
        }
        private void SetStartPositions()
        {
            teamAStartPositions = new Vector3[teamAPlayers.Length];
            teamAStartRotations = new Quaternion[teamAPlayers.Length];
            teamBStartPositions = new Vector3[teamBPlayers.Length];
            teamBStartRotations = new Quaternion[teamBPlayers.Length];

            for (int i = 0; i < teamAPlayers.Length; i++)
            {
                teamAStartPositions[i] = teamAPlayers[i].transform.position;
                teamAStartRotations[i] = teamAPlayers[i].transform.rotation;
            }

            for (int i = 0; i < teamBPlayers.Length; i++)
            {
                teamBStartPositions[i] = teamBPlayers[i].transform.position;
                teamBStartRotations[i] = teamBPlayers[i].transform.rotation;
            }
        }
        private void SetPlayerControl(bool enabled)
        {
            foreach (GameObject player in teamAPlayers)
            {
                var controller = player.GetComponent<PlayerController>();
                if (controller != null) controller.SetStuck(!enabled);
            }
        }
        private void GoalRoutine()
        {
            StartCoroutine(HandleGoalRoutine());
        }

        private IEnumerator HandleGoalRoutine()
        {
            matchRunning = false;

            yield return new WaitForSeconds(0.1f);

            ResetPlayers();

            MatchAudio.Current.audioManager.Play("RefereeLongWhistle", 0.15f);
            matchRunning = true;
        }
        #endregion

        #region Public Methods
        public void OnGoalScored()
        {
            GoalRoutine();
        }
        #endregion
    }
}
