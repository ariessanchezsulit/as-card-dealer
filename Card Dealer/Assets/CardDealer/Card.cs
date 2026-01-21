using UnityEngine;

namespace CardDealer
{
    public class Card : MonoBehaviour
    {
        [SerializeField] private GameObject _frontSprite;
        [SerializeField] private GameObject _backSprite;

        private bool _isSet = false;
        private int _gridIndex = -1;

        public void Set(int gridIndex, Vector3 position) {
            _gridIndex = gridIndex;
            _isSet = true;
            this.transform.position = position;
        }

        public void Flip()
        {
            // Animate and flip the card here
        }
    }
}