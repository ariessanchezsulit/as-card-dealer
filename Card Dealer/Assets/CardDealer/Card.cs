using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardDealer
{
    public enum CardOrientation
    {
        Horizontal,
        Vertical
    }
    
    public class Card : MonoBehaviour
    {
        // TODO: Use Service Locator for the camera
        [SerializeField] private Camera _camera;
        [SerializeField] private SpriteRenderer _frontSprite;
        [SerializeField] private SpriteRenderer _backSprite;

        [SerializeField] private float rotateSpeed = 720f; 
        
        [SerializeField]
        private bool _isFaceDown = true;

        [SerializeField] 
        private CardOrientation _orientation = CardOrientation.Vertical;

        private void Update()
        {
            AnimateCardFlip();
            AnimateCardTilt();
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
            var targetY = !_isFaceDown ? 0f : 180f;
            var current = transform.localEulerAngles;
            
            var newY = Mathf.MoveTowardsAngle(
                current.y,
                targetY,
                rotateSpeed * Time.deltaTime
            );
            
            transform.localRotation = Quaternion.Euler(0f, newY, 0f);
        }
        
        [Button]
        public void Tilt(CardOrientation orientation)
        {
            _orientation = orientation;
        }

        private void AnimateCardTilt()
        {
            if (_orientation == CardOrientation.Horizontal)
            {
                var targetZ = 90f;
                var currentZ = _frontSprite.transform.localEulerAngles;
                
                var newZ = Mathf.MoveTowardsAngle(
                    currentZ.z,
                    targetZ,
                    rotateSpeed * Time.deltaTime
                );
                
                _frontSprite.transform.localRotation = Quaternion.Euler(0f, 0f, newZ);
                _backSprite.transform.localRotation = Quaternion.Euler(0f, 0f, newZ);
            }
        }
    }
}