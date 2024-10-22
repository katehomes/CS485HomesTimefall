using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Essence Card", menuName = "EssenceCard")]
public class EssenceCard : Card
{

    public void Awake()
    {
        cardType = CardType.ESSENCE;
    }
}
