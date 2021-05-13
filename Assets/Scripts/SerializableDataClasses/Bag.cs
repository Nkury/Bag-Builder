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
public class OrbInfo
{
    public string orb_name;
    public int quantity;
}

public class OrbInfoObject
{
    public Orb orb;
    public int quantity;
}

[Serializable]
public class Bag
{
    public string BagName;

    public List<OrbInfo> OrbBagInfo = new List<OrbInfo>();

    [NonSerialized]
    public List<Orb> Orbs = new List<Orb>();

    [NonSerialized]
    public Dictionary<string, int> OrbInfoDictionary = new Dictionary<string, int>();

    [NonSerialized]
    public List<int> TypeTableInBag; // array position represents orbType. Value in array is number of that orb type

    [NonSerialized]
    public List<int> TypeTableDrawn; // array position represents orbType. Value in array is number of that orb type

    [NonSerialized]
    public Dictionary<string , int> QuantityTable; // <orb_name, quantity>

    [NonSerialized]
    public int NumberOfOrbsInBag;

    [NonSerialized]
    public Dictionary<string , int> DrawnQuantityTable; // <orb_name, quantity>

    [NonSerialized]
    public int NumberOfOrbsDrawn;

    [NonSerialized]
    private int _totalWeight;

    [NonSerialized]
    private List<WeightedEntry> _weightTable = new List<WeightedEntry>();

    [NonSerialized]
    private IContext _context; 

    public void SetupOrbInfo( IContext context )
    {
        _context = context;
        int numOfOrbTypes = Enum.GetValues( typeof( OrbType ) ).Length;
        TypeTableInBag = new List<int>( new int[ numOfOrbTypes ] );
        TypeTableDrawn = new List<int>( new int[ numOfOrbTypes ] );
        QuantityTable = new Dictionary<string , int>();
        DrawnQuantityTable = new Dictionary<string , int>();
        NumberOfOrbsInBag = 0;
        NumberOfOrbsDrawn = 0;

        for( int infoIndex = 0; infoIndex < OrbBagInfo.Count; infoIndex++ )
        {
            string orbName = OrbBagInfo[ infoIndex ].orb_name;
            int quantity = OrbBagInfo[ infoIndex ].quantity;

            Orb orb = context.OrbManager.GetOrbFromDictionary( orbName );
            if( orb == null )
            {
                Debug.LogError( string.Format( "Orb not found in Orb Dictionary: {0}" , orbName ) );
                return;
            }

            NumberOfOrbsInBag += quantity;
            // if we have a orb with a power of 2, it will add 2 of that type in the type table
            TypeTableInBag[ ( int ) orb.type ] += quantity * orb.power;

            if( QuantityTable.ContainsKey( orb.asset_name ) )
            {
                QuantityTable[ orb.asset_name ] += quantity;
            }
            else
            {
                QuantityTable.Add( orb.asset_name , quantity );
            }
            
            for( int numOfOrbs = 0; numOfOrbs < quantity; numOfOrbs++ )
            {
                Orbs.Add( orb );
            }
        }
    }

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
        }       
    }
    
    public void AddPermanentOrb( Orb orb, int quantity = 1 )
    {
        Orbs.Add( orb ); // add to orb list so it's saved into bag.json

        // adding a permanent orb is very similar to adding a temporary but also adding it to the Orbs list for json saving
        AddTemporaryOrbToBag( orb , quantity );
    }
    
    public void AddTemporaryOrbToBag( Orb orb, int quantity = 1 )
    {
        TypeTableInBag[ ( int ) orb.type ] += orb.power;

        if( QuantityTable.ContainsKey( orb.asset_name ) )
        {
            QuantityTable[ orb.asset_name ] += quantity;
        }
        else
        {
            QuantityTable.Add( orb.asset_name , quantity );
        }

        NumberOfOrbsInBag += quantity;

        ReadjustWeightTableAfterAdd( orb , quantity );
    }

    public void AddTemporaryOrbToHand( Orb orb, int quantity = 1 )
    {
        TypeTableDrawn[ ( int ) orb.type ] += orb.power;

        if( DrawnQuantityTable.ContainsKey( orb.asset_name ) )
        {
            DrawnQuantityTable[ orb.asset_name ] += quantity;
        }
        else
        {
            DrawnQuantityTable.Add( orb.asset_name , quantity );
        }

        NumberOfOrbsDrawn += quantity;

        // No need to readjust any weight tables. 
        // When this orb gets added back into the bag, the weight table will be adjusted to compensate
    }

    private void ReadjustWeightTableAfterAdd( Orb orb , int quantity = 1 )
    {
        for( int i = 0; i < quantity; i++ )
        {
            WeightedEntry weightedEntry;
            _totalWeight += orb.weight;
            weightedEntry.orb = orb;
            weightedEntry.accumulatedWeight = _totalWeight;
            _weightTable.Add( weightedEntry );
        }
    }

    public Orb DrawOrb()
    {
        System.Random rand = new System.Random();
        int chosen = rand.Next( 0 , _totalWeight );
        Orb returnOrb = null;

        // all orbs have been drawn (for enemies mostly) if this gets here by accident
        // another script should handle this case.
        if( NumberOfOrbsInBag <= 0 )
        {
            return null;
        }

        int weightCount = _weightTable.Count;
        for( int weightIndex = 0; weightIndex < weightCount; weightIndex++ )
        {
            if( chosen < _weightTable[ weightIndex ].accumulatedWeight  )
            {
                returnOrb = _weightTable[ weightIndex ].orb;
                ReadjustWeightTableAfterRemove( weightIndex );

                int orbType = ( int ) returnOrb.type;

                // add to drawn type table and remove from bag type table
                TypeTableDrawn[ orbType ] += returnOrb.power;
                TypeTableInBag[ orbType ] -= returnOrb.power;

                // add to bag quantity table and drawn bag quantity tables
                if( QuantityTable.ContainsKey( returnOrb.asset_name ) )
                {
                    QuantityTable[ returnOrb.asset_name ]--;
                }
                else
                {
                    // TODO: THIS IS AN ERROR-- RAISE AN ERROR LATER WITH LOGS OR SOMETHING
                    return null;
                }

                NumberOfOrbsInBag--;

                if( DrawnQuantityTable.ContainsKey( returnOrb.asset_name ) )
                {
                    DrawnQuantityTable[ returnOrb.asset_name ]++;
                }
                else
                {
                    DrawnQuantityTable.Add( returnOrb.asset_name , 1 );
                }

                NumberOfOrbsDrawn++;
                break; // get out of this for loop
            }
        }

        return returnOrb;
    }

    /// <summary>
    /// Removing drawn orbs from hand and placing them back into the bag
    /// </summary>
    /// <param name="orb">Orb object to remove from hand</param>
    /// <param name="quantity">Number of that orb to remove from hand</param>
    /// <returns>A boolean true if successfully removed from hand to bag, false otherwise</returns>
    public bool RemoveDrawnOrbToBag( Orb orb, int quantity = 1 )
    {
        int orbTypeIndex = ( int ) orb.type;
        if( DrawnQuantityTable.ContainsKey( orb.asset_name ) )
        {
            int drawnOrbsOfType = DrawnQuantityTable[ orb.asset_name ];
            if( quantity <= drawnOrbsOfType )
            {
                DrawnQuantityTable[ orb.asset_name ] -= quantity;

                if(QuantityTable.ContainsKey( orb.asset_name ) )
                {
                    QuantityTable[ orb.asset_name ] += quantity;
                }
                else
                {
                    QuantityTable.Add( orb.asset_name , quantity );
                }

                TypeTableDrawn[ orbTypeIndex ] -= orb.power * quantity;
                TypeTableInBag[ orbTypeIndex ] += orb.power * quantity;

                NumberOfOrbsDrawn -= quantity;
                NumberOfOrbsInBag += quantity;

                ReadjustWeightTableAfterAdd( orb , quantity );

                return true;
            }
            else
            {
                Debug.LogError( string.Format( "ERROR in Bag.cs RemoveDrawnOrbToBag: Cannot remove {0} orb from hand. " +
                    "Currently have {1} in hand and requesting {2} to be removed" ,
                    orb.asset_name ,
                    drawnOrbsOfType ,
                    quantity ) );
            }
        }
        else
        {
            Debug.LogError( string.Format( "ERROR in Bag.cs RemoveDrawnOrbToBag: {0} cannot be found in DrawnQuantityTable dictionary" ,
                            orb.asset_name ) );
        }

        return false;
    }

    public bool RemoveOrbFromBagToHand( Orb orb, int quantity = 1 )
    {
        int orbTypeIndex = ( int ) orb.type;
        if( QuantityTable.ContainsKey( orb.asset_name ) )
        {
            int orbsInBag = QuantityTable[ orb.asset_name ];
            if( quantity <= orbsInBag )
            {
                QuantityTable[ orb.asset_name ] -= quantity;

                if( DrawnQuantityTable.ContainsKey( orb.asset_name ) )
                {
                    DrawnQuantityTable[ orb.asset_name ] += quantity;
                }
                else
                {
                    DrawnQuantityTable.Add( orb.asset_name , quantity );
                }

                TypeTableDrawn[ orbTypeIndex ] += orb.power * quantity;
                TypeTableInBag[ orbTypeIndex ] -= orb.power * quantity;

                NumberOfOrbsDrawn += quantity;
                NumberOfOrbsInBag -= quantity;

                ReadjustWeightTableAfterRemove( orb , quantity );

                return true;
            }
            else
            {
                Debug.LogError( string.Format( "ERROR in Bag.cs RemoveOrbFromBagToHand: Cannot remove {0} orb from bag. " +
                    "Currently have {1} in bag and requesting {2} to be removed" ,
                    orb.asset_name ,
                    orbsInBag ,
                    quantity ) );
            }
        }
        else
        {
            Debug.LogError( string.Format( "ERROR in Bag.cs RemoveOrbFromBagToHand: {0} cannot be found in QuantityTable dictionary" ,
                            orb.asset_name ) );
        }

        return false;
    }

    private void ReadjustWeightTableAfterRemove( Orb orb, int quantity = 1 )
    {
        int countToRemove = quantity;
        int weightTableCount = _weightTable.Count;

        for( int weightIndex = 0; weightIndex < _weightTable.Count; weightIndex++ )
        {
            if( countToRemove == 0 )
            {
                break;
            }

            if( _weightTable[ weightIndex ].orb == orb )
            {
                ReadjustWeightTableAfterRemove( weightIndex );
                weightTableCount--;
                countToRemove--;
            }
        }
    }

    private void ReadjustWeightTableAfterRemove( int removedIndex )
    {
        int weightToRemove = _weightTable[ removedIndex ].orb.weight;
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
 
    /// <summary>
    /// Place all drawn orbs back into the bag
    /// </summary>
    public void ResetBag()
    {
        // <KEY: orb name, VALUE: number of orbs in hand>
        foreach(KeyValuePair<string, int> orbInfo in DrawnQuantityTable )
        {
            if( orbInfo.Value > 0 )
            {
                if( QuantityTable.ContainsKey( orbInfo.Key ) )
                {
                    QuantityTable[ orbInfo.Key ] += orbInfo.Value;
                }
                else
                {
                    QuantityTable.Add( orbInfo.Key , orbInfo.Value );
                }

                Orb orb = _context.OrbManager.GetOrbFromDictionary( orbInfo.Key );
                int orbTypeIndex = ( int ) orb.type;

                TypeTableDrawn[ orbTypeIndex ] -= orb.power * orbInfo.Value;
                TypeTableInBag[ orbTypeIndex ] += orb.power * orbInfo.Value;

                DrawnQuantityTable[ orbInfo.Key ] = 0;

                ReadjustWeightTableAfterAdd( orb , orbInfo.Value );
            }
        }
    }

    public bool RemoveOrbPermanently( Orb orb, int quantity = 1 )
    {
        int orbTypeIndex = ( int ) orb.type;
        int countToRemove = quantity;

        // make sure quantity is less than or equal to number of that orb in hand and bag
        int numInBag = 0;
        int numInHand = 0;
        if( QuantityTable.ContainsKey( orb.asset_name ) )
        {
            numInBag = QuantityTable[ orb.asset_name ];
        }

        if( DrawnQuantityTable.ContainsKey( orb.asset_name ) )
        {
            numInHand = DrawnQuantityTable[ orb.asset_name ];
        }

        if( countToRemove <= ( numInBag + numInHand ) )
        {
            // removing from Orb list that will be saved to JSON
            RemoveOrbFromOrbList( orb , quantity );

            // remove from hand first then bag
            // num in hand is greater than count to remove so remove that much
            // no need to readjust weight table since this has already been taken out of the bag
            if( numInHand >= countToRemove )
            {
                DrawnQuantityTable[ orb.asset_name ] -= countToRemove;
                TypeTableDrawn[ orbTypeIndex ] -= countToRemove * orb.power;
                countToRemove = 0;
                NumberOfOrbsDrawn -= countToRemove;
            }
            else if( numInHand > 0 ) 
            {
                countToRemove -= DrawnQuantityTable[ orb.asset_name ];
                NumberOfOrbsDrawn -= DrawnQuantityTable[ orb.asset_name ];
                DrawnQuantityTable[ orb.asset_name ] = 0;
                TypeTableDrawn[ orbTypeIndex ] = 0;
            }

            // if we've removed all the orbs we need to then return true and leave function.
            // else, try to remove from bag
            if(countToRemove == 0 )
            {
                return true;
            }

            if( numInBag >= countToRemove )
            {
                QuantityTable[ orb.asset_name ] -= countToRemove;
                TypeTableInBag[ orbTypeIndex ] -= countToRemove * orb.power;
                ReadjustWeightTableAfterRemove( orb , countToRemove );
                NumberOfOrbsInBag -= countToRemove;
                countToRemove = 0;
            }
            else if( numInBag > 0 )
            {
                NumberOfOrbsInBag -= QuantityTable[ orb.asset_name ];
                ReadjustWeightTableAfterRemove( orb , QuantityTable[ orb.asset_name ] );
                countToRemove -= QuantityTable[ orb.asset_name ];
                QuantityTable[ orb.asset_name ] = 0;
                TypeTableInBag[ orbTypeIndex ] = 0;
            }

            return countToRemove == 0;
        }
        else
        {
            Debug.LogError( string.Format( "ERROR in Bag.cs RemoveOrbPermanently: Cannot remove {0} orb from bag / hand. " +
                    "Currently have {1} in bag / hand and requesting {2} to be removed" ,
                    orb.asset_name ,
                    numInHand + numInBag ,
                    quantity ) );
        }

        return false;
    }        

    private bool RemoveOrbFromOrbList( Orb orb, int quantity = 1 )
    {
        int countToRemove = quantity;
        for( int orbIndex = 0; orbIndex < Orbs.Count; orbIndex++ )
        {
            if(countToRemove == 0 )
            {
                break;
            }

            if( Orbs[orbIndex] == orb )
            {               
                Orbs.RemoveAt( orbIndex );
                countToRemove--;
            }
        }

        return countToRemove == 0;
    }

    public bool BagMeetsRequirements( List<Requirement> orbRequirements, out List<OrbInfo> orbsThatMeetRequirement )
    {
        int numRequirements = orbRequirements.Count;

        for( int reqIndex = 0; reqIndex < numRequirements; reqIndex++ )
        {
            Requirement req = orbRequirements[ reqIndex ];
            if( TypeTableInBag[ ( int ) req.orbType ] < req.quantity )
            {
                orbsThatMeetRequirement = null;
                return false;
            }
        }

        orbsThatMeetRequirement = GetRequiredOrbs( orbRequirements , QuantityTable );

        return true;
    }

    public bool DrawnOrbsMeetsRequirements( List<Requirement> orbRequirements, out List<OrbInfo> orbsThatMeetRequirement )
    {
        int numRequirements = orbRequirements.Count;

        for( int reqIndex = 0; reqIndex < numRequirements; reqIndex++ )
        {
            Requirement req = orbRequirements[ reqIndex ];
            if( TypeTableDrawn[ ( int ) req.orbType ] < req.quantity )
            {
                orbsThatMeetRequirement = null;
                return false;
            }
        }

        orbsThatMeetRequirement = GetRequiredOrbs( orbRequirements, DrawnQuantityTable );
        
        return true;
    }

    private List<OrbInfo> GetRequiredOrbs( List<Requirement> orbRequirements, Dictionary<string, int> quantityTable )
    {
        List<OrbInfo> requiredOrbs = new List<OrbInfo>();

        for( int reqIndex = 0; reqIndex < orbRequirements.Count; reqIndex++ )
        {
            List<string> orbNames = _context.OrbManager.GetOrbNamesMatchingType( orbRequirements[ reqIndex ].orbType );
            int required = orbRequirements[ reqIndex ].quantity;

            // go through each of those orb names, determine if it's in the quantity table, then extract orbInfo from there
            for( int orbIndex = 0; orbIndex < orbNames.Count; orbIndex++ )
            {
                if(required == 0 )
                {
                    break;
                }

                string orbName = orbNames[ orbIndex ];
                if( quantityTable.ContainsKey( orbNames[ orbIndex ] ) )
                {
                    int drawn = quantityTable[ orbName ];
                    if( drawn == 0 )
                    {
                        continue;
                    }

                    Orb orb = _context.OrbManager.GetOrbFromDictionary( orbName );
                    int numToAdd = required / orb.power;
                    if( numToAdd > 0 )
                    {
                        required %= orb.power;
                        OrbInfo orbInfo = new OrbInfo();
                        orbInfo.orb_name = orbName;
                        orbInfo.quantity = numToAdd;
                        requiredOrbs.Add( orbInfo );
                    } 
                }
            }
        }

        return requiredOrbs;
    }
}
