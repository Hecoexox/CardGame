using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card")]
public class CardData : ScriptableObject
{
    [Header("Basic Info")]
    public int id;
    public string cardName;
    public Sprite artwork;
    public bool isUnlocked;

    [Header("Stats")]
    public int attack;
    public int health;
    public CardLevel level;
}

public enum CardLevel
{
    Apex_Predator,
    Alpha_Predator,
    Meso_Predator,
    Micro_Predator,
    Mega_Herbivore,
    Meso_Herbivore,
    Micro_Herbivore
}
