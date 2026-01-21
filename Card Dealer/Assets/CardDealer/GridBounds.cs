using Sirenix.OdinInspector;
using UnityEngine;

namespace CardDealer
{
    public class GridBounds : MonoBehaviour
    {
        [SerializeField] private Bounds _bounds;

        public Vector3 Size => _bounds.size;
        
        [Button]
        public void UpdateBounds()
        {
            transform.position = Vector3.zero;
            _bounds = new Bounds(
                transform.position,
                transform.localScale
            );
        }
    }
}