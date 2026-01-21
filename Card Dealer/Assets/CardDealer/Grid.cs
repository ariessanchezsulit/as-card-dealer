using System.Collections.Generic;
using UnityEngine;

namespace CardDealer
{
    public class Grid : MonoBehaviour
    {
        // Use the proper bounds, for now use, min and max from the center
        [SerializeField]
        private Vector2 _bounds; // Width and Height * 2 bounds representation

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


        [ContextMenu("Generate Grid")]
        public void GenerateGrid()
        {
            var min = new Vector3(-_bounds.x, -_bounds.y);
            var max = new Vector3(_bounds.x, _bounds.y);

            var colDistance = (max.x - min.x);
            var unitCol = colDistance / _cols;
            var startX = (colDistance * 0.5f) * -1f;

            var rowDistance = (max.y - min.y);
            var unitRow = rowDistance / _rows;
            var startY = (rowDistance * 0.5f) * -1f;

            for (var i = 0; i < _cols; i++)
            {
                for (var j = 0; j < _rows; j++)
                {
                    // TODO: Get the dots from the grid
                    var posX = startX + (i * unitCol);
                    var posY = startY + (j * unitRow);

                    var dot = GameObject.Instantiate<Transform>(_dotTemplate, new Vector3(posX, posY, 0), Quaternion.identity, this.transform);
                }
            }
        }
    }
}