using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerMovement), typeof(PlayerMovement))]
    public class PlayerManager : Charactor
    {
        public bool isGuarding { get; private set; }
        public bool isAvoiding { get; private set; }

        PlayerMovement playerMovement;
        PlayerInputAction playerInput;

        private new void Start()
        {
            base.Start();
            playerMovement = GetComponent<PlayerMovement>();
            playerInput = GetComponent<PlayerInputAction>();
            playerInput.inputAvoid = Avoid;
            playerInput.inputDefend = (bool isInput) => Defend(isInput);
            playerInput.inputMove = (Vector2 vector) => playerMovement.OnMove(vector);
            dieAction += _dieAction;
        }

        void _dieAction()
        {
            gameObject.SetActive(false);
        }

        void Avoid()
        {
            charactorAnimation.StartAnimation("Avoid");
            isAvoiding = true;
        }

        void Defend(bool isInput)
        {
            if (isInput == true){ charactorAnimation.StartAnimation("Defend"); }
            if (isInput ==false){ charactorAnimation.EndAnimation("Defend"); }
            isGuarding = isInput;
        }
    }
}