using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardDealer
{
    public class Card : MonoBehaviour
    {
        // TODO: Use Service Locator for the camera
        [SerializeField] private Camera _camera;
        [SerializeField] private SpriteRenderer _frontSprite;
        [SerializeField] private SpriteRenderer _backSprite;

        [SerializeField] private float rotateSpeed = 720f; 
        
        [SerializeField]
        private bool _isFaceDown = true;

        private void Update()
        {
            AnimateCardFlip();
        }

        private void LateUpdate()
        {
            UpdateCardFace();
        }

        [Button]
        public void Flip()
        {
            // Animate and flip the card here
            _isFaceDown = !_isFaceDown;
        }
        
        [Button]
        public void UpdateCardFace()
        {
            var frontFacing = Vector3.Dot(transform.forward, _camera.transform.forward) < 0f;
            _frontSprite.enabled = frontFacing;
            _backSprite.enabled = !frontFacing;
        }

        private void AnimateCardFlip()
        {
            var targetY = _isFaceDown ? 0f : 180f;
            var current = transform.localEulerAngles;
            var newY = Mathf.MoveTowardsAngle(
                current.y,
                targetY,
                rotateSpeed * Time.deltaTime
            );

            transform.localRotation = Quaternion.Euler(0f, newY, 0f);
        }
    }
}