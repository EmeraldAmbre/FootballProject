using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FootballProject
{
    public class PlayerSwitcher : MonoBehaviour
    {
        public Rigidbody ball;

        [SerializeField] private GameObject[] players;

        void Start()
        {
            players = GameObject.FindGameObjectsWithTag("Player");
        }
    }
}
