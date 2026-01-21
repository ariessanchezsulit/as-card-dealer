using System;
using System.Collections;
using System.Collections.Generic;
using Common.Pool;
using PP.Tools.Common;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CardDealer
{
    public struct CardEntry
    {
        public Card Card;
        public Vector3 From;
        public Vector3 To;
        public CardOrientation Or;
    }
    
    public class Deck : MonoBehaviour, ILocatable
    {
        [SerializeField] private Card _cardTemplate;
        [SerializeField] private Pool _cardPool;
        [SerializeField] private List<Card> _cards; // TODO: Move this to pool

        [SerializeField] private float _moveDuration = 0.25f;
        [SerializeField] private float _delayInterval = 0.05f;
        private readonly Queue<CardEntry> _queue = new();
        private bool _isRunning;

        public CardOrientation Orientation = CardOrientation.Vertical;

        // For debug only
        private int _distributeIndex = 0;

        private Grid _grid;

        private void Start()
        {
            _grid = Dealer.Locator.Get<Grid>();
        }

        [Button]
        public void ClearCards()
        {
            _cardPool.ReturnAll();
            _cards.Clear();
        }
        
        public void DistributeCard(Vector3 pos)
        {
            // TODO: Get the card(s) from the pool
            var item = _cardPool.Get<Transform>();
            item.Item.SetParent(this.transform);
            
            var card = item.Item.GetComponent<Card>();//GameObject.Instantiate<Card>(_cardTemplate, this.transform);

            AnimateTheCard(card, pos);
        }
        
        [Button]
        public void DistributeCard()
        {
            var dot = _grid.Dots[_distributeIndex];
            
            Debug.Log($"{dot.name} Pos: {dot.transform.position}");
            
            // TODO: Get the card(s) from the pool
            var item = _cardPool.Get<Transform>();
            item.Item.SetParent(this.transform);
            
            var card = item.Item.GetComponent<Card>();//GameObject.Instantiate<Card>(_cardTemplate, this.transform);

            AnimateTheCard(card, dot.transform.position);
            
            _distributeIndex++;
            if (_distributeIndex > 11)
            {
                _distributeIndex = 0;
            }
        }
        
        [Button]
        public void DistributeCards()
        {
            var dots = _grid.Dots;
            
            foreach (var dot in dots)
            {
                Debug.Log($"{dot.name} Pos: {dot.transform.position}");
                
                // TODO: Get the card(s) from the pool
                var item = _cardPool.Get<Transform>();
                item.Item.SetParent(this.transform);
                
                var card = item.Item.GetComponent<Card>();//GameObject.Instantiate<Card>(_cardTemplate, this.transform);
                AnimateTheCard(card, dot.transform.position);
            }
        }

        // TOOD: Move this animation out of this Deck, and remove the usage of Coroutine
        public void AnimateTheCard(Card card, Vector3 to)
        {
            _cards.Add(card);
            
            // Convert this to tween
            var from = _cardTemplate.transform.localPosition;
            from.z = -1f;
            
            card.ResetCard(_cardTemplate);
            card.transform.localPosition = from;
            
            EnqueueCard(card, from, to);
        }
        
        public void EnqueueCard(Card card, Vector3 from, Vector3 to)
        {
            CardEntry entry;
            entry.Card = card;
            entry.From = from;
            entry.To = to;
            entry.Or = Orientation;
            
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
                entry.Card.Tilt(entry.Or);
            }

            _isRunning = false;
        }

        private IEnumerator MoveCard(Transform card, Vector3 from, Vector3 to, float duration)
        {
            var t = 0f;
            card.position = from;

            while (t < 1f)
            {
                // Move the card
                t += Time.deltaTime / duration;
                card.position = Vector3.Lerp(from, to, t);
                yield return null;
            }

            card.position = to;
        }
        
        public void Dispose()
        {
            // Cleanup whatever you needed to clean here
        }
    }
}