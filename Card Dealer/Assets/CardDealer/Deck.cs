using System.Collections.Generic;
using UnityEngine;

namespace CardDealer
{
    public class Deck : MonoBehaviour
    {
        [SerializeField] private Card _cardPrefab;
        [SerializeField] private Transform _deckPosition; // Calculate the position based on the bounds
        [SerializeField] private List<Card> _cards; // TODO: Move this to pool

        private void DistributeCards(int numberOfCards)
        {
            for (var i = 0; i < numberOfCards; i++)
            {
                // TODO: Get the card(s) from the pool
                var card = GameObject.Instantiate<Card>(_cardPrefab, this.transform);
                card.Set(i, _deckPosition.position);

                _cards.Add(card);
                AnimateTheCard(card, i);
            }
            
        }

        private void AnimateTheCard(Card card, int index)
        {
            // After animating, flip the card
            card.Flip();
        }

        private Vector3 CalculateDeckPosition(int index)
        {
            // Calculate the center position based on the bounds of the deck area
            return Vector3.zero; // Placeholder
        }
    }
}