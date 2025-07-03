using UnityEngine;
using FootballProject.Events;

namespace FootballProject.Audio
{
    public class MatchAudio : AudioMaster<MatchAudio>
    {
        private const float VELOCITY_TO_SOUND_VOLUME = 0.0275f;
        private const float BALL_CONTROL_SOUND_MOD = 0.4f;
        private const float BALL_HIT_GOAL_SOUND_MOD = 0.75f;
        private const float WHISTLE_MOD = 0.25f;

        protected override void OnEnable()
        {
            base.OnEnable();
            EventManager.Subscribe<PlayerPassEvent>(PlayerPass);
            EventManager.Subscribe<PlayerShootEvent>(PlayerShoot);
            EventManager.Subscribe<RefereeLastWhistleEvent>(RefereeLastWhistle);
            EventManager.Subscribe<RefereeLongWhistleEvent>(RefereeLongWhistle);
            EventManager.Subscribe<RefereeShortWhistleEvent>(RefereeShortWhistle);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            EventManager.UnSubscribe<PlayerPassEvent>(PlayerPass);
            EventManager.UnSubscribe<PlayerShootEvent>(PlayerShoot);
            EventManager.UnSubscribe<RefereeLastWhistleEvent>(RefereeLastWhistle);
            EventManager.UnSubscribe<RefereeLongWhistleEvent>(RefereeLongWhistle);
            EventManager.UnSubscribe<RefereeShortWhistleEvent>(RefereeShortWhistle);
        }

        private void PlayerPass(PlayerPassEvent eventObject)
        {
            audioManager.Play("PlayerPass", BALL_CONTROL_SOUND_MOD);
        }
        private void PlayerShoot(PlayerShootEvent eventObject)
        {
            audioManager.Play("PlayerShoot", BALL_HIT_GOAL_SOUND_MOD);
        }
        private void RefereeLastWhistle(RefereeLastWhistleEvent eventObject)
        {
            audioManager.Play("RefereeLastWhistle", WHISTLE_MOD);
        }
        private void RefereeLongWhistle(RefereeLongWhistleEvent eventObject)
        {
            audioManager.Play("RefereeLongWhistle", WHISTLE_MOD);
        }
        private void RefereeShortWhistle(RefereeShortWhistleEvent eventObject)
        {
            audioManager.Play("RefereeShortWhistle", WHISTLE_MOD);
        }
    }
}

