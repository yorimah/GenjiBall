using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputAction : MonoBehaviour
    {
        InputController _inputController;
        Vector2 moveVector;

        public delegate void Callback_Void();
        public delegate void Callback_Bool(bool isInput);
        public delegate void Callback_Vectotr2(Vector2 vector);
        public Callback_Void inputAvoid;
        public Callback_Bool inputDefend;
        public Callback_Vectotr2 inputMove;

        private void OnEnable()
        {
            _inputController = new InputController();

            _inputController.Player.Move.started += OnMove;
            _inputController.Player.Move.performed += OnMove;
            _inputController.Player.Move.canceled += OnMove;
            _inputController.Player.Avoid.started += OnAvoid;
            _inputController.Player.Defend.started += OnDefend;
            _inputController.Player.Defend.canceled += OnDefend;
            _inputController.Camera.AngleMovement.started += inputMoveCamera;

            _inputController.Enable();
        }

        private void OnDisable()
        {
            _inputController.Player.Move.started -= OnMove;
            _inputController.Player.Move.performed -= OnMove;
            _inputController.Player.Move.canceled -= OnMove;
            _inputController.Player.Avoid.started -= OnAvoid;
            _inputController.Player.Defend.started -= OnDefend;
            _inputController.Player.Defend.canceled -= OnDefend;
            _inputController.Camera.AngleMovement.started -= inputMoveCamera;

            _inputController.Disable();
        }

        private void OnMove(InputAction.CallbackContext obj)
        {
            if (inputMove == null) { return; }

            switch (obj.phase)
            {
                case InputActionPhase.Started:
                case InputActionPhase.Performed:
                    moveVector = obj.ReadValue<Vector2>();
                    break;
                case InputActionPhase.Canceled:
                    moveVector = Vector2.zero;
                    break;
            }

            inputMove(moveVector);
        }

        private void OnAvoid(InputAction.CallbackContext obj)
        {
            if (inputAvoid == null) { return; }

            inputAvoid();
        }

        private void OnDefend(InputAction.CallbackContext obj)
        {
            if (inputDefend == null) { return; }

            bool isInput = false;
            switch (obj.phase)
            {
                case InputActionPhase.Started:
                    isInput = true;
                    break;
            }
            inputDefend(isInput);
        }

        private void inputMoveCamera(InputAction.CallbackContext obj)
        {
            if (inputMove == null) { return; }

            //　カメラの移動でもプレイヤーの向きの更新をさせたいため
            inputMove(moveVector);
        }
    }
}