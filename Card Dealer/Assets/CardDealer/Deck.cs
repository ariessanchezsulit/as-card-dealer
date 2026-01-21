using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardDealer
{
    public struct CardEntry
    {
        public Card Card;
        public Vector3 From;
        public Vector3 To;
    }
    
    public class Deck : MonoBehaviour
    {
        [SerializeField] private Card _cardTemplate;
        [SerializeField] private List<Card> _cards; // TODO: Move this to pool

        [SerializeField] private Grid _grid; // TODO: Move this to Service Locator

        [SerializeField] private float _moveDuration = 0.25f;
        [SerializeField] private float _delayInterval = 0.05f;
        private readonly Queue<CardEntry> _queue = new();
        private bool _isRunning;
        
        [Button]
        public void DistributeCards()
        {
            var dots = _grid.Dots;
            
            foreach (var dot in dots)
            {
                Debug.Log($"{dot.name} Pos: {dot.transform.position}");
                
                // TODO: Get the card(s) from the pool
                var card = GameObject.Instantiate<Card>(_cardTemplate, this.transform);
                _cards.Add(card);

                AnimateTheCard(card, dot.transform.position);
            }
        }
        
        // TOOD: Move this animation out of this Deck, and remove the usage of Coroutine
        private void AnimateTheCard(Card card, Vector3 to)
        {
            // Convert this to tween
            var from = _cardTemplate.transform.position;
            EnqueueCard(card, from, to);
        }
        
        public void EnqueueCard(Card card, Vector3 from, Vector3 to)
        {
            CardEntry entry;
            entry.Card = card;
            entry.From = from;
            entry.To = to;
            
            _queue.Enqueue(entry);

            if (!_isRunning)
                StartCoroutine(ProcessQueue());
        }

        private IEnumerator ProcessQueue()
        {
            _isRunning = true;

            while (_queue.Count > 0)
            {
                var entry = _queue.Dequeue();
                yield return MoveCard(entry.Card.transform, entry.From, entry.To, _moveDuration);
                yield return new WaitForSeconds(_delayInterval);
                
                entry.Card.Flip();
                _cards.Add(entry.Card);
            }

            _isRunning = false;
        }

        private IEnumerator MoveCard(Transform card, Vector3 from, Vector3 to, float duration)
        {
            var t = 0f;
            card.position = from;

            while (t < 1f)
            {
                t += Time.deltaTime / duration;
                card.position = Vector3.Lerp(from, to, t);
                yield return null;
            }

            card.position = to;
        }
    }
}