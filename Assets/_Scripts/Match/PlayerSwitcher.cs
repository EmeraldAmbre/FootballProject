using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FootballProject
{
    public class PlayerSwitcher : MonoBehaviour
    {
        [SerializeField] private PlayerController[] players;
        private Controls controls;
        private int currentIndex = 0;
        private CameraMovement cam;

        private void Awake()
        {
            players = FindObjectsOfType<PlayerController>();
            cam = FindObjectOfType<CameraMovement>();
            controls = new Controls();
            controls.PlayerControls.Switch.performed += ctx => Switch();
        }

        private void OnEnable()
        {
            controls.Enable();
        }

        private void OnDisable()
        {
            controls.Disable();
        }

        private void Switch()
        {
            currentIndex = (currentIndex + 1) % players.Length;
            cam.SetTarget(players[currentIndex].transform);
            for (int i = 0; i < players.Length; i++)
            {
                players[i].isActive = (i == currentIndex);
            }
        }
    }

}
