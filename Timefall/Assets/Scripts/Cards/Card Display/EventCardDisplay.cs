using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EventCardDisplay : CardDisplay
{
    public int pos = 0;
    public Image borderImage;

    [Header("Victory Points")]
    public TMP_Text stewardText;
    public Image stewardImage;
    public TMP_Text seekerText;
    public Image seekerImage;
    public TMP_Text sovereignText;
    public Image sovereignImage;
    public TMP_Text weaverText;
    public Image weaverImage;

    // Start is called before the first frame update
    void Start()
    {
        //ResetDisplay((EventCardData) displayCard);
    }

    private void ResetDisplay(EventCardData eventData) 
    {
        //base card
        nameText.text = eventData.cardName;
        descText.text = eventData.description;

        image.texture = eventData.image;

        
        //victory points
        SetVictoryPoints(eventData.victoryPoints);
    
        SetFactionColors(GetFactionColor(eventData.faction));
        
    }

    public void SetCard(EventCard eventCard)
    {
        displayCard = eventCard;
        ResetDisplay(eventCard.eventCardData);
    }

    // public void SetCard(Card eventCard)
    // {
    //     SetCard((EventCard) eventCard);
    // }


    void SetFactionColors(Color color)
    {
        borderImage.color = color;
    }

    void SetVictoryPoints(int[] vpArr)
    {

        setVictoryPointUI(stewardText, stewardImage, vpArr[0]);
        setVictoryPointUI(seekerText, seekerImage, vpArr[1]);
        setVictoryPointUI(sovereignText, sovereignImage, vpArr[2]);
        setVictoryPointUI(weaverText, weaverImage, vpArr[3]);
    }

    string GetVPText(int vp)
    {
        string symbol = ""; 
        if(vp > 0) { symbol = "+";}
        else if (vp < 0) { symbol = "-";}
        else {return "";}

        return string.Format("{0}{1}", symbol, Mathf.Abs(vp));
    }

    void setVictoryPointUI(TMP_Text vpTMP, Image vpImage, int vp)
    {
        string vpText = GetVPText(vp);
        if (vpText.Equals(""))
        { //black out VP for no points add/removed
            vpTMP.enabled = false;
            vpImage.enabled = false;
        } else 
        { //set VP text
            vpTMP.enabled = true;
            vpTMP.text = vpText;
            vpImage.enabled = true;
        }
    }

}