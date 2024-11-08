using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EssenceCardDisplay : CardDisplay
{

    void ResetDisplay(EssenceCard essenceCard) 
    {
        //base card
        nameText.text = essenceCard.cardName;
        descText.text = essenceCard.description;

        image.texture = essenceCard.image;
    
        // SetFactionColors(GetFactionColor(eventCard.faction));
        
    }

    void SetCard(EssenceCard essenceCard)
    {
        displayCard = essenceCard;
        ResetDisplay(essenceCard);
    }

    public void SetCard(Card essenceCard)
    {
        SetCard((EssenceCard) essenceCard);
    }

}
