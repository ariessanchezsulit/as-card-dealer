using System;
using CardDealer.Helper;
using PP.Tools.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

namespace CardDealer
{
    public class Dealer : MonoBehaviour
    {
        public static readonly Locator Locator = new Locator();
        
        [SerializeField] private Grid _grid;
        [SerializeField] private Deck _deck;
        
        [SerializeField] Button _dealButton;
        [SerializeField] private TMP_InputField _numInputField;

        private void Awake()
        {
            Locator.Register(_grid);
            Locator.Register(_deck);
        }

        private void OnEnable()
        {
            _dealButton.onClick.AddListener(OnDeal);
        }

        private void OnDisable()
        {
            _dealButton.onClick.RemoveListener(OnDeal);
        }

        [Button]
        public void OnDeal()
        {
            _deck.ClearCards();
            
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