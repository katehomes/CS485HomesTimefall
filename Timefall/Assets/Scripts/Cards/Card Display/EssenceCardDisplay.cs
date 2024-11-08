using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EssenceCardDisplay : CardDisplay
{

    private void ResetDisplay(EssenceCardData essenceCard) 
    {
        //base card
        nameText.text = essenceCard.cardName;
        descText.text = essenceCard.description;

        image.texture = essenceCard.image;
    
        // SetFactionColors(GetFactionColor(eventCard.faction));
        
    }

    public void SetCard(EssenceCard essenceCard)
    {
        displayCard = essenceCard;
        ResetDisplay(essenceCard.essenceCardData);
    }

    // public void SetCard(Card essenceCard)
    // {
    //     SetCard((EssenceCard) essenceCard);
    // }

}