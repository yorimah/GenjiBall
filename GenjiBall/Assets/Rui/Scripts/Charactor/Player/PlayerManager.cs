using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerMovement), typeof(PlayerMovement))]
    public class PlayerManager : Charactor
    {
        PlayerMovement playerMovement;
        PlayerInputAction playerInput;

        private new void Start()
        {
            base.Start();
            playerMovement = GetComponent<PlayerMovement>();
            playerInput = GetComponent<PlayerInputAction>();
            playerInput.inputMove = (Vector2 vector) => playerMovement.OnMove(vector);
            dieAction += _dieAction;
        }

        void _dieAction()
        {
            gameObject.SetActive(false);
        }
    }
}