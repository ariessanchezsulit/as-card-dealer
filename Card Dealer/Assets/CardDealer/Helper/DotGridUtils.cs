using System.Collections.Generic;
using UnityEngine;

namespace CardDealer.Helper
{
    // TODO: Change this from being static to instance, and move to the Locator Service
    public static class DotGridUtils
    {
        // Your mapping: returns dot number 1..12
        public static int DotNumber(int row, int col) => 1 + row + (3 * col);

        // Helper: returns the dot Transform at row/col, assuming dots are stored by dot number (1..12)
        public static Transform DotAt(IList<Transform> dotsByNumber, int row, int col)
        {
            int num = DotNumber(row, col);     // 1..12
            return dotsByNumber[num - 1];      // 0-based list index
        }
        
        /// Returns N world positions centered on the middle row of a 3x4 layout.
        /// Matches your examples for N=1..4.
        public static List<Vector3> GetCenteredPositions(IList<Transform> dotsByNumber, int nCards)
        {
            if (dotsByNumber == null || dotsByNumber.Count < 12)
            {
                Debug.LogError($"Invalid parameter! {dotsByNumber}");
                return null;
            }

            nCards = Mathf.Clamp(nCards, 1, 4);

            const int row = 1;      // middle row (dots 2,5,8,11)
            const int leftCol = 1;  // col of dot 5
            const int rightCol = 2; // col of dot 8

            var dot5 = DotAt(dotsByNumber, row, leftCol);
            var dot8 = DotAt(dotsByNumber, row, rightCol);

            // Base spacing along the row (distance between adjacent columns on the middle row)
            float step = Vector3.Distance(
                DotAt(dotsByNumber, row, 1).position, // dot 5
                DotAt(dotsByNumber, row, 2).position  // dot 8
            );

            // Direction from left to right along the row
            var dir = (dot8.position - dot5.position).normalized;

            // Center point between dot5 and dot8 (your "center" for 1 card)
            var center = (dot5.position + dot8.position) * 0.5f;

            // Build N offsets centered around 0
            // N=1 => {0}
            // N=2 => {-0.5, +0.5} -> lands on dot5 and dot8
            // N=3 => {-1, 0, +1} -> between (2,5), (5,8), (8,11)
            // N=4 => {-1.5, -0.5, +0.5, +1.5} -> lands on dots 2,5,8,11
            var result = new List<Vector3>(nCards);
            var start = -(nCards - 1) * 0.5f;

            for (var i = 0; i < nCards; i++)
            {
                var offsetSteps = start + i;        // -1.5..+1.5
                var pos = center + dir * (offsetSteps * step);
                result.Add(pos);
            }

            return result;
        }
    }
}