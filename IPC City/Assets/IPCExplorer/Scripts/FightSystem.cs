using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class FightSystem : MonoBehaviour
{
    // Algorithms
    // A) Who starts first?
    // 1) DEXTERITY + random between zero and LUCK
    // 2) If tie, higher DEXTERITY wins
    // 3) If tie, 50/50

    public Text strength1 = null;
    public Text dexterity1 = null;
    public Text intelligence1 = null;
    public Text constitution1 = null;
    public Text luck1 = null;

    public Text strength2 = null;
    public Text dexterity2 = null;
    public Text intelligence2 = null;
    public Text constitution2 = null;
    public Text luck2 = null;

    public int strength1Value = 0;
    public int dexterity1Value = 0;
    public int intelligence1Value = 0;
    public int constitution1Value = 0;
    public int luck1Value = 0;

    public int strength2Value = 0;
    public int dexterity2Value = 0;
    public int intelligence2Value = 0;
    public int constitution2Value = 0;
    public int luck2Value = 0;

    public float critChance1 = 0f;
    public float dodgeChance1 = 0f;

    public float critChance2 = 0f;
    public float dodgeChance2 = 0f;

    private bool play = false;
    private int step = 0;

    public int starts1 = 0;
    public int starts2 = 0;

    public int leftWins = 0;
    public int rightWins = 0;

    public bool leftAttacks = false;
    public bool gameEnded = false;

    private int constitution1ValueOriginal = 0;
    private int constitution2ValueOriginal = 0;

    private void SetUItoValues()
    {
        strength1.text = strength1Value.ToString();
        dexterity1.text = dexterity1Value.ToString();
        intelligence1.text = intelligence1Value.ToString();
        constitution1.text = constitution1Value.ToString();
        luck1.text = luck1Value.ToString();

        strength2.text = strength2Value.ToString();
        dexterity2.text = dexterity2Value.ToString();
        intelligence2.text = intelligence2Value.ToString();
        constitution2.text = constitution2Value.ToString();
        luck2.text = luck2Value.ToString();
    }

    private void Start()
    {
        constitution1ValueOriginal = constitution1Value;
        constitution2ValueOriginal = constitution2Value;

        SetUItoValues();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            play = true;
        }

        step += 1;

        if (step == 1)
        {
            constitution1Value = (constitution1Value * 10) + 50;
            constitution2Value = (constitution2Value * 10) + 50;

            critChance1 = luck1Value / 3f;
            dodgeChance1 = luck1Value / 2f;

            critChance2 = luck2Value / 3f;
            dodgeChance2 = luck2Value / 2f;
        }
        else if (step == 2)
        {
            float random1 = Random.Range(0, luck1Value);
            float random2 = Random.Range(0, luck2Value);

            if (dexterity1Value + random1 > dexterity2Value + random2)
            {
                starts1 += 1;
            }
            else if (dexterity1Value + random1 < dexterity2Value + random2)
            {
                starts2 += 1;
            }
            else
            {
                if (dexterity1Value > dexterity2Value)
                {
                    starts1 += 1;
                }
                else if (dexterity1Value < dexterity2Value)
                {
                    starts2 += 1;
                }
                else
                {
                    float randomWin = Random.value;
                    if (randomWin > 0.5f)
                    {
                        starts1 += 1;
                    }
                    else
                    {
                        starts2 += 1;
                    }
                }
            }

            if (starts1 > starts2)
            {
                leftAttacks = true;
            }
        }
        else if (step == 3)
        {
            int strValue = 0;
            int constValue = 0;
            int dexValue = 0;
            int intValue = 0;
            float critValue = 0;

            if (leftAttacks)
            {
                strValue = strength1Value;
                constValue = constitution2Value;
                dexValue = dexterity1Value;
                intValue = intelligence1Value;
                critValue = critChance1;
            }
            else
            {
                strValue = strength2Value;
                constValue = constitution1Value;
                dexValue = dexterity2Value;
                intValue = intelligence2Value;
                critValue = critChance2;
            }

            int strRoll = Random.Range(1, strValue) * 2;
            int dexRoll = dexValue + Random.Range(0, 50);
            int intRoll = intValue + Random.Range(0, 50);
            float critRoll = critValue + Random.Range(0f, 50f);

            if (dexRoll >= 50) // Double hit (max)
            {
                Debug.Log("Chance to Double Hit!");

                if (intRoll >= 50) // Double hit
                {
                    Debug.Log("It is a Double Hit!");

                    if (critRoll >= 25) // Crit
                    {
                        Debug.Log("Double Hit with a Crit!");

                        constValue -= (int)(strRoll * 1.5f) * 2;

                        Debug.Log("DAMAGE: " + (int)(strRoll * 1.5f) * 2);
                    }
                    else // No crit
                    {
                        Debug.Log("Double Hit with No Crit!");

                        constValue -= (int)(strRoll) * 2;

                        Debug.Log("DAMAGE: " + (int)(strRoll) * 2);
                    }
                }
                else if (intRoll >= 25) // Single hit
                {
                    Debug.Log("It is a Single Hit!");

                    if (critRoll >= 25) // Crit
                    {
                        Debug.Log("Single Hit with a Crit!");

                        constValue -= (int)(strRoll * 1.5f);

                        Debug.Log("DAMAGE: " + (int)(strRoll * 1.5f));
                    }
                    else // No crit
                    {
                        Debug.Log("Single Hit with No Crit!");

                        constValue -= (int)(strRoll);

                        Debug.Log("DAMAGE: " + (int)(strRoll));
                    }
                }
                else // No hit
                {
                    Debug.Log("Miss!");
                }
            }
            else // Single hit (max)
            {
                Debug.Log("Chance to Single Hit!");

                if (intRoll >= 25) // Single hit
                {
                    Debug.Log("It is a Single Hit!");

                    if (critRoll >= 25) // Crit
                    {
                        Debug.Log("Single Hit with a Crit!");

                        constValue -= (int)(strRoll * 1.5f);

                        Debug.Log("DAMAGE: " + (int)(strRoll * 1.5f));
                    }
                    else // No crit
                    {
                        Debug.Log("Single Hit with No Crit!");

                        constValue -= (int)(strRoll);

                        Debug.Log("DAMAGE: " + (int)(strRoll));
                    }
                }
                else // No hit
                {
                    Debug.Log("Miss!");
                }
            }

            if (leftAttacks)
            {
                constitution2Value = constValue;
            }
            else
            {
                constitution1Value = constValue;
            }

            if (leftAttacks && constitution2Value > 0)
            {
                leftAttacks = false;
                step -= 1;
            }
            else if (!leftAttacks && constitution1Value > 0)
            {
                leftAttacks = true;
                step -= 1;
            }
            else
            {
                if (leftAttacks && constitution2Value <= 0)
                {
                    Debug.Log("Left Wins!");

                    leftWins += 1;
                    gameEnded = true;
                }
                else if (!leftAttacks && constitution1Value <= 0)
                {
                    Debug.Log("Right Wins!");

                    rightWins += 1;
                    gameEnded = true;
                }
            }
        }

        SetUItoValues();

        if (gameEnded)
        {
            step = 0;
            constitution1Value = constitution1ValueOriginal;
            constitution2Value = constitution2ValueOriginal;
            gameEnded = false;
            SetUItoValues();
        }
    }

    private void Fight()
    {
        if (play)
        {
            step += 1;

            if (step == 1)
            {
                constitution1Value *= 10;
                constitution2Value *= 10;

                critChance1 = luck1Value / 3f;
                dodgeChance1 = luck1Value / 2f;

                critChance2 = luck2Value / 3f;
                dodgeChance2 = luck2Value / 2f;
            }
            else if (step == 2)
            {
                float random1 = Random.Range(0, luck1Value);
                float random2 = Random.Range(0, luck2Value);

                if (dexterity1Value + random1 > dexterity2Value + random2)
                {
                    starts1 += 1;
                }
                else if (dexterity1Value + random1 < dexterity2Value + random2)
                {
                    starts2 += 1;
                }
                else
                {
                    if (dexterity1Value > dexterity2Value)
                    {
                        starts1 += 1;
                    }
                    else if (dexterity1Value < dexterity2Value)
                    {
                        starts2 += 1;
                    }
                    else
                    {
                        float randomWin = Random.value;
                        if (randomWin > 0.5f)
                        {
                            starts1 += 1;
                        }
                        else
                        {
                            starts2 += 1;
                        }
                    }
                }

                if (starts1 > starts2)
                {
                    leftAttacks = true;
                }
            }
            else if (step == 3)
            {
                int strValue = 0;
                int constValue = 0;
                int dexValue = 0;
                int intValue = 0;
                float critValue = 0;

                if (leftAttacks)
                {
                    strValue = strength1Value;
                    constValue = constitution2Value;
                    dexValue = dexterity1Value;
                    intValue = intelligence1Value;
                    critValue = critChance1;
                }
                else
                {
                    strValue = strength2Value;
                    constValue = constitution1Value;
                    dexValue = dexterity2Value;
                    intValue = intelligence2Value;
                    critValue = critChance2;
                }

                int strRoll = Random.Range(1, strValue) * 2;
                int dexRoll = dexValue + Random.Range(0, 50);
                int intRoll = intValue + Random.Range(0, 50);
                float critRoll = critValue + Random.Range(0f, 50f);

                if (dexRoll >= 50) // Double hit (max)
                {
                    Debug.Log("Chance to Double Hit!");

                    if (intRoll >= 50) // Double hit
                    {
                        Debug.Log("It is a Double Hit!");

                        if (critRoll >= 25) // Crit
                        {
                            Debug.Log("Double Hit with a Crit!");

                            constValue -= (int)(strRoll * 1.5f) * 2;

                            Debug.Log("DAMAGE: " + (int)(strRoll * 1.5f) * 2);
                        }
                        else // No crit
                        {
                            Debug.Log("Double Hit with No Crit!");

                            constValue -= (int)(strRoll) * 2;

                            Debug.Log("DAMAGE: " + (int)(strRoll) * 2);
                        }
                    }
                    else if (intRoll >= 25) // Single hit
                    {
                        Debug.Log("It is a Single Hit!");

                        if (critRoll >= 25) // Crit
                        {
                            Debug.Log("Single Hit with a Crit!");

                            constValue -= (int)(strRoll * 1.5f);

                            Debug.Log("DAMAGE: " + (int)(strRoll * 1.5f));
                        }
                        else // No crit
                        {
                            Debug.Log("Single Hit with No Crit!");

                            constValue -= (int)(strRoll);

                            Debug.Log("DAMAGE: " + (int)(strRoll));
                        }
                    }
                    else // No hit
                    {
                        Debug.Log("Miss!");
                    }
                }
                else // Single hit (max)
                {
                    Debug.Log("Chance to Single Hit!");

                    if (intRoll >= 25) // Single hit
                    {
                        Debug.Log("It is a Single Hit!");

                        if (critRoll >= 25) // Crit
                        {
                            Debug.Log("Single Hit with a Crit!");

                            constValue -= (int)(strRoll * 1.5f);

                            Debug.Log("DAMAGE: " + (int)(strRoll * 1.5f));
                        }
                        else // No crit
                        {
                            Debug.Log("Single Hit with No Crit!");

                            constValue -= (int)(strRoll);

                            Debug.Log("DAMAGE: " + (int)(strRoll));
                        }
                    }
                    else // No hit
                    {
                        Debug.Log("Miss!");
                    }
                }

                if (leftAttacks)
                {
                    constitution2Value = constValue;
                }
                else
                {
                    constitution1Value = constValue;
                }

                if (leftAttacks && constitution2Value > 0)
                {
                    leftAttacks = false;
                    step -= 1;
                }
                else if (!leftAttacks && constitution1Value > 0)
                {
                    leftAttacks = true;
                    step -= 1;
                }
                else
                {
                    if (leftAttacks && constitution2Value <= 0)
                    {
                        Debug.Log("Left Wins!");

                        leftWins += 1;
                        gameEnded = true;
                    }
                    else if (!leftAttacks && constitution1Value <= 0)
                    {
                        Debug.Log("Right Wins!");

                        rightWins += 1;
                        gameEnded = true;
                    }
                }
            }

            SetUItoValues();
            play = false;
        }
    }
}
