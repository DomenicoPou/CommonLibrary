using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Handlers
{
    public static class RandomHandler
    {
        /// <summary>
        /// Random variable used
        /// </summary>
        private static Random random = new Random();

        /// <summary>
        /// Obtains a random key from a dictionary thats weighted
        /// </summary>
        /// <param name="weightedDictionary">The weighted dictionary</param>
        /// <returns>Returns a random key from the weighted dictionary</returns>
        public static string GetRandomKey(Dictionary<string, int> weightedDictionary) 
        {
            // Add the current total from the weighed values
            int currentTotal = 0;
            foreach (string key in weightedDictionary.Keys)
            {
                currentTotal += weightedDictionary[key];
            }

            // Get a random number between the total given
            int randSequence = random.Next(currentTotal);

            // Minus the values from the weighted dictionary 
            foreach (string key in weightedDictionary.Keys)
            { 
                // If the sequence is bellow or equal to 0. Return the current key.
                randSequence -= weightedDictionary[key];
                if (randSequence <= 0)
                {
                    return key;
                }
            }
            
            // Something probably wen't wrong here. Return null.
            return null;
        }
    }
}
