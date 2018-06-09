using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static class to generate sets of character ID's
/// </summary>
public static class RandomSet
{
    /// <summary>
    /// Generates a list of random ints from a given range
    /// </summary>
    /// <param name="min">lower bound (inclusive)</param>
    /// <param name="max">upper bound (exclusive)</param>
    /// <param name="count">number of ints to pull from range</param>
    /// <param name="isUnique">whether ints in list should be unique</param>
    /// <returns></returns>
    public static List<int> RandomSetOfInts(int min, int max, int count, bool isUnique)
    {
        List<int> baseList = new List<int>();       // list of possible integers to pull from
        List<int> returnList = new List<int>();     // list containing the randomly generated integers

        // protect from invalid min-max range
        if (max <= min)
        {
            Debug.Log("Error: Max cannot be lesser or equal to min.");
            return null;
        }

        // protect from invalid count parameter
        if (count < 1)
        {
            Debug.Log("Error: Cannot generate list of less than 1 int.");
            return null;
        }

        // add ints to base list according to specified range
        for (int i = min; i < max; i++)
        {
            baseList.Add(i);
        }

        // for as long as specified count
        for (int i = 0; i < count; i++)
        {
            // generate random number from base list
            int randLoc = (int)Random.Range(0, baseList.Count);
            int numToAdd = baseList[randLoc];

            // if return list must be unique
            if (isUnique)
            {
                // remove generated number from list
                baseList.RemoveAt(randLoc);
            }

            // add generated number from base list to return list
            returnList.Add(numToAdd);
        }

        return returnList;
    }
}
