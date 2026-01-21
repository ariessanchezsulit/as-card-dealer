using CardDealer.Helper;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace CardDealer
{
    public class Dealer : MonoBehaviour
    {
        [SerializeField] private Grid _grid;
        [SerializeField] private Deck _deck;
        
        [SerializeField] Button _deadButton;
        [SerializeField] private TMP_InputField _numInputField;

        [Button]
        public void OnDeal()
        {
            if (int.TryParse(_numInputField.text, out var num))
            {
                CalculateTargetPositions(num);
            }
        }

        private void CalculateTargetPositions(int num)
        {
            Debug.Log(num);
            
            // TODO: Change this to instance type
            var positions = DotGridUtils.GetCenteredPositions(_grid.Dots, num);

            foreach (var pos in positions)
            {
                _deck.DistributeCard(pos);
            }
            
            
            /*
            if (num <= 4)
            {
                // Center
            }
            else if (num <= 8)
            {
                // Center and Top
            }
            else
            {
                // Center, Top, and Bottom
            }
            //*/
        }
    }
}