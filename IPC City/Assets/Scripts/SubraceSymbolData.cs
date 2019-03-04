using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SubraceSymbolData", menuName = "SubraceSymbolData", order = 1)]
public class SubraceSymbolData : ScriptableObject
{
    public Sprite[] subraceSymbols;

    public Sprite GetSubraceSymbol(string subraceString)
    {
        if (subraceString.Contains("Hill"))
        {
            return subraceSymbols[1];
        }
        else if (subraceString.Contains("Lumber"))
        {
            return subraceSymbols[2];
        }
        else if (subraceString.Contains("Mountain"))
        {
            return subraceSymbols[3];
        }
        else if (subraceString.Contains("Quarry"))
        {
            return subraceSymbols[4];
        }
        else if (subraceString.Contains("Volcano"))
        {
            return subraceSymbols[5];
        }
        else if (subraceString.Contains("Dark"))
        {
            return subraceSymbols[7];
        }
        else if (subraceString.Contains("High"))
        {
            return subraceSymbols[8];
        }
        else if (subraceString.Contains("Night"))
        {
            return subraceSymbols[9];
        }
        else if (subraceString.Contains("Sun"))
        {
            return subraceSymbols[10];
        }
        else if (subraceString.Contains("Wood"))
        {
            return subraceSymbols[11];
        }
        else if (subraceString.Contains("Coastal"))
        {
            return subraceSymbols[13];
        }
        else if (subraceString.Contains("Eastern"))
        {
            return subraceSymbols[14];
        }
        else if (subraceString.Contains("Mythic"))
        {
            return subraceSymbols[15];
        }
        else if (subraceString.Contains("Nordic"))
        {
            return subraceSymbols[16];
        }
        else if (subraceString.Contains("Southern"))
        {
            return subraceSymbols[17];
        }
        else if (subraceString.Contains("Ash"))
        {
            return subraceSymbols[19];
        }
        else if (subraceString.Contains("Blood"))
        {
            return subraceSymbols[20];
        }
        else if (subraceString.Contains("Plains"))
        {
            return subraceSymbols[21];
        }
        else if (subraceString.Contains("Sand"))
        {
            return subraceSymbols[22];
        }
        else if (subraceString.Contains("Swamp"))
        {
            return subraceSymbols[23];
        }
        else
            return null;
    }
}
