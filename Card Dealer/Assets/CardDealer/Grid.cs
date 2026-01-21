using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardDealer
{
    public class Grid : MonoBehaviour
    {
        // Use the proper bounds, for now use, min and max from the center
        [SerializeField]
        private GridBounds _bounds; // Width and Height * 2 bounds representation

        [SerializeField]
        private int _rows;

        [SerializeField]
        private int _cols;

        [SerializeField]
        private Transform _dotTemplate;

        [SerializeField]
        private List<Transform> _dots;

        [SerializeField]
        private Deck _deck;

        [SerializeField]
        private int _positionCards;

        [Button]
        public void GenerateGrid()
        {
            // Update the bounds first
            _bounds.UpdateBounds();
            
            for (var i = 0; i < _cols; i++)
            {
                for (var j = 0; j < _rows; j++)
                {
                    // TODO: Get the dots from the grid
                    var posX = (i - (_cols - 1) * 0.5f) * (_bounds.Size.x / _cols);
                    var posY = (j - (_rows - 1) * 0.5f) * (_bounds.Size.y / _rows);

                    var dot = GameObject.Instantiate(_dotTemplate, new Vector3(posX, posY, 0), Quaternion.identity, transform);
                    dot.name = $"Pos {posX}-{posY}";
                    dot.gameObject.SetActive(true);
                }
            }
        }
    }
}