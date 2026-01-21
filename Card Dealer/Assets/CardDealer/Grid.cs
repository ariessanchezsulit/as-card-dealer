using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

using Common.Pool;

namespace CardDealer
{
    public class Grid : MonoBehaviour
    {
        // Use the proper bounds, for now use, min and max from the center
        [SerializeField]
        private GridBounds _bounds; // Width and Height * 2 bounds representation

        [SerializeField]
        private int _rows;
        public int Rows => _rows;

        [SerializeField]
        private int _cols;
        public int Cols => _cols;

        [SerializeField]
        private Pool _dotPool;

        [SerializeField]
        private List<Transform> _dots;

        public IList<Transform> Dots => _dots;

        [Button]
        public void GenerateGrid()
        {
            // Update the bounds first
            _bounds.UpdateBounds();
            _dotPool.Preload(10);
            ClearGrid();

            var index = 0;
            
            for (var i = 0; i < _cols; i++)
            {
                for (var j = 0; j < _rows; j++)
                {
                    // TODO: Get the dots from the grid
                    var posX = (i - (_cols - 1) * 0.5f) * (_bounds.Size.x / _cols);
                    var posY = (j - (_rows - 1) * 0.5f) * (_bounds.Size.y / _rows);

                    var pos = _bounds.transform.position + new Vector3(posX, posY, -10f);
                    // var dot = GameObject.Instantiate(_dotTemplate, Vector3.zero, Quaternion.identity, transform);
                    var item = _dotPool.Get<Transform>();
                    var dot = item.Item;
                    dot.SetParent(transform);
                    dot.name = $"[{index}] Pos {posX}-{posY}";
                    dot.localPosition = pos;
                    dot.rotation = Quaternion.identity;
                    dot.gameObject.SetActive(true);
                    index++;
                    
                    
                    _dots.Add(dot);
                }
            }
        }

        [Button]
        public void ClearGrid()
        {
            _dots.Clear();
            _dotPool.ReturnAll();
        }

        [Button]
        public void GetCenterPosition()
        {
            // NOTE: Hardcoded calculaton.
            //  The best way to get the center is:
            //
            // 1. Check if both row and column is odd:
            //      Then use the formula to get the index of item: (total items - 1)/2 
            
            var targetIndex = -1;
            var offsetX = 0f;
            var offsetY = 0f;
            
            // Check if row and col are odd numbers
            if ((_rows & 1) != 0 && (_cols & 1) != 0)
            {
                targetIndex = (_dots.Count -1) / 2;
                offsetX = 0;
                offsetY = 0f;
            }
            // // Check of offset X
            // else if ((_rows & 1) != 0)
            // {
            //     
            // }
            // // Check of offset Y
            // else if ((_cols & 1) != 0)
            // {
            //     
            // }
            else
            {
                targetIndex = (_dots.Count / 2) - 2;
                offsetX = (_bounds.Size.x / _cols) * 0.5f;
                offsetY = 0f;
            }
            
            
            var targetTransform = _dots[targetIndex];
            var targetPos = targetTransform.position;
            targetPos.x += offsetX;
            targetPos.y += offsetY;
            
            var item = _dotPool.Get<Transform>();
            var dot = item.Item;
            dot.SetParent(transform);
            dot.name = $"[X] {targetTransform.name}";
            dot.localPosition = targetPos;
            dot.rotation = Quaternion.identity;
            dot.gameObject.SetActive(true);
        }
    }
}