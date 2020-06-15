using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct WeightedEntry
{
    public int accumulatedWeight;
    public Orb orb;
}

[Serializable]
public class Bag
{
    public List<Orb> Orbs = new List<Orb>();

    [NonSerialized]
    public List<int> QuantityTable;

    [NonSerialized]
    public List<Orb> DrawnOrbs = new List<Orb>();

    [NonSerialized]
    public List<int> DrawnQuantityTable;

    [NonSerialized]
    private int _totalWeight;

    [NonSerialized]
    private List<WeightedEntry> _weightTable = new List<WeightedEntry>();

    public void SetupWeightTable()
    {
        int orbCount = Orbs.Count;
        for( int i = 0; i < orbCount; i++ )
        {
            // populate the weight table
            WeightedEntry weightedEntry;
            _totalWeight += Orbs[ i ].weight;
            weightedEntry.orb = Orbs[ i ];
            weightedEntry.accumulatedWeight = _totalWeight;
            _weightTable.Add( weightedEntry );

            // populate the quantity table
            QuantityTable[ ( int ) Orbs[ i ].type ]++;
        }

        int numOfOrbTypes = Enum.GetValues( typeof( OrbType ) ).Length;
        QuantityTable = new List<int>( numOfOrbTypes );
        DrawnQuantityTable = new List<int>( numOfOrbTypes );
    }

    public void AddOrb( Orb orb )
    {
        Orbs.Add( orb );
        QuantityTable[ ( int ) orb.type ]++;
    }

    public Orb DrawOrb()
    {
        System.Random rand = new System.Random();
        int chosen = rand.Next( 0 , _totalWeight );
        Orb returnOrb = null;

        int weightCount = _weightTable.Count;
        for( int weightIndex = 0; weightIndex < weightCount; weightIndex++ )
        {
            if( chosen < _weightTable[ weightIndex ].accumulatedWeight  )
            {
                returnOrb = _weightTable[ weightIndex ].orb;
                ReadjustWeightTable( weightIndex );

                int orbType = ( int ) returnOrb.type;
                // add to drawn orb list and quantity table
                DrawnOrbs.Add( returnOrb );
                DrawnQuantityTable[ orbType ]++;

                // remove from orb list and quantity table
                Orbs.RemoveAt( weightIndex );
                QuantityTable[ orbType ]--;
                break;
            }
        }

        return returnOrb;
    }

    private void ReadjustWeightTable( int removedIndex )
    {
        int weightToRemove = Orbs[ removedIndex ].weight;
        _weightTable.RemoveAt( removedIndex );
        int weightCount = _weightTable.Count;

        for( int weightIndex = removedIndex; weightIndex < weightCount; weightIndex++ )
        {
            WeightedEntry entry = _weightTable[ weightIndex ];
            entry.accumulatedWeight -= weightToRemove;
            _weightTable[ weightIndex ] = entry;
        }

        _totalWeight -= weightToRemove;
    }

    private void ResetBag()
    {
        QuantityTable.Clear();
        DrawnQuantityTable.Clear();

        Orbs.AddRange( DrawnOrbs );

        DrawnOrbs.Clear();

        int orbCount = Orbs.Count;
        for( int orbIndex = 0; orbIndex < orbCount; orbIndex++ )
        {
            QuantityTable[ ( int ) Orbs[ orbIndex ].type ]++;
        }
    }
}
