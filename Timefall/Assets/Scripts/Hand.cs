using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour
{
    public static string LOCATION = "HAND";

    public RectTransform  handPanel;

    public List<Card> cardsInHand;

    [Header("Managers")]
    public BattleManager battleManager;
    public BoardManager boardManager;

    public TurnManager turnManager;
    public Deck timelineDeck;
    public DeckDisplay playerDeckDisplay;

    [Header("Buttons")]
    public Button drawPlayerButton;
    public Button shufflePlayerButton;
    public Button drawTimelineButton;
    public Button shuffleTimelineButton;

    [Header("Card Display Prefabs")]
    public GameObject agentCardDisplay;
    public GameObject essenceCardDisplay;
    public GameObject eventCardDisplay;

    [Header("Expanded Card View")]
    public Transform expandedHoverTransform;

    public Transform expandedStaticTransform;

    public List<CardDisplay> staticCards = new List<CardDisplay>();

    // Start is called before the first frame update
    void Start()
    {
        turnManager = FindObjectOfType<TurnManager>();
        battleManager = FindObjectOfType<BattleManager>();

		drawPlayerButton.onClick.AddListener(DrawFromPlayerDeck);

		shufflePlayerButton.onClick.AddListener(ShufflePlayerDeck);

		drawTimelineButton.onClick.AddListener(DrawFromTimelineDeck);

		shuffleTimelineButton.onClick.AddListener(ShuffleTimelineDeck);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DrawFromPlayerDeck()
    {
        DrawFromDeck(playerDeckDisplay.deck);
    }



    void ShufflePlayerDeck()
    {
        ShuffleDeck(playerDeckDisplay.deck);
    }

    void DrawFromDeck(Deck targetDeck){
        Card card = targetDeck.Draw();

        if(card == null){ return;}

        CardType cardType = card.cardType;

        // Debug.Log (card.ToString());

        GameObject obj = null;

        switch(cardType) 
        {
            case CardType.AGENT:
                obj = Instantiate(agentCardDisplay, new Vector3(0, 0, 0), Quaternion.identity);
                AgentCardDisplay agentDC = obj.GetComponent<AgentCardDisplay>();

                if(agentDC == null){return;} 
                agentDC.SetCard(card);
                agentDC.InstantiateInHand(transform);

                break;
            case CardType.ESSENCE:
                obj = Instantiate(essenceCardDisplay, new Vector3(0, 0, 0), Quaternion.identity);
                EssenceCardDisplay essenceDC = obj.GetComponent<EssenceCardDisplay>();

                if(essenceDC == null){return;} 
                essenceDC.SetCard(card);
                essenceDC.InstantiateInHand(transform);
                break;
            case CardType.EVENT:
                obj = Instantiate(eventCardDisplay, new Vector3(0, 0, 0), Quaternion.identity);
                EventCardDisplay eventDC = obj.GetComponent<EventCardDisplay>();

                if(eventDC == null){return;} 
                eventDC.SetCard(card);
                eventDC.InstantiateInHand(transform);
                break;
            default:
            //Error handling
                Debug.Log ("Invalid Card Type: " + cardType);
                return;
        }

        cardsInHand.Add(card);

        // obj.transform.SetParent(transform);
        //obj.transform.localScale =  new Vector3(0.55f, 0.55f, 0.55f);
        
	}

    void ShuffleDeck(Deck targetDeck){
        targetDeck.Shuffle();
	}

    public CardDisplay ExpandCardView(Card card, bool hoverClear)
    {
        if(card == null){ return null;}

        CardType cardType = card.cardType;

        // Debug.Log (card.ToString());

        GameObject obj = null;

        CardDisplay displayToReturn = null;

        Transform expandedViewTransform = hoverClear ? expandedHoverTransform :  expandedStaticTransform;

        switch(cardType) 
        {
            case CardType.AGENT:
                obj = Instantiate(agentCardDisplay, new Vector3(0, 0, 0), Quaternion.identity);
                AgentCardDisplay agentDC = obj.GetComponent<AgentCardDisplay>();

                if(agentDC == null){return null;} 
                agentDC.SetCard(card);
                agentDC.Place(expandedViewTransform, "EXPAND");

                displayToReturn = agentDC;

                break;
            case CardType.ESSENCE:
                obj = Instantiate(essenceCardDisplay, new Vector3(0, 0, 0), Quaternion.identity);
                EssenceCardDisplay essenceDC = obj.GetComponent<EssenceCardDisplay>();

                if(essenceDC == null){return null;} 
                essenceDC.SetCard(card);
                essenceDC.Place(expandedViewTransform, "EXPAND");

                displayToReturn = essenceDC;

                break;
            case CardType.EVENT:
                obj = Instantiate(eventCardDisplay, new Vector3(0, 0, 0), Quaternion.identity);
                EventCardDisplay eventDC = obj.GetComponent<EventCardDisplay>();

                if(eventDC == null){return null;} 
                eventDC.SetCard(card);
                eventDC.Place(expandedViewTransform, "EXPAND");

                displayToReturn = eventDC;

                break;
            default:
            //Error handling
                Debug.Log ("Invalid Card Type: " + cardType);
                return null;
        }

        obj.transform.SetParent(expandedViewTransform, false);

        if(!hoverClear)
        {
            staticCards.Add(displayToReturn);
        }

        return displayToReturn;
    }

    public void CloseExpandCardView()
    {
        while (expandedHoverTransform.childCount > 0) {
            
            DestroyImmediate(expandedHoverTransform.GetChild(0).gameObject);
        }
    }

        void ShuffleTimelineDeck()
    {
        ShuffleDeck(timelineDeck);
    }

    public void DrawFromTimelineDeck()
    {
        //Debug.Log("Drawing from timeline deck!");
        Card card = timelineDeck.Draw();

        if(card == null){ return;}
        //Debug.Log("card exists!");

        CardDisplay display = ExpandCardView(card, false);

        display.playState = CardPlayState.IntialTimelineDraw;

        // display.OnPointerClick.AddListener(PlayTimelineCard(display));
    }

    public void PlayTimelineCard(CardDisplay display)
    {
        if(display == null){ return;}

        boardManager.PlaceTimelineEventForTurn(display);
        turnManager.SetVictoryPointUI();
    }

    public void AutoPlayTimelineCard()
    {
        Debug.Log("AutoPlayTimelineCard");
        if(staticCards.Count == 1)
        {
            Debug.Log("count==1");
            PlayTimelineCard(staticCards[0]);
            staticCards.RemoveAt(0);
        }
    }

    public bool CanPlayCard(Card card)
    {
        return turnManager.CanPlayCard(card);
    }

    public void DrawStartOfTurnHand()
    {

        int handSize = 5; //TODO: get player hand size
        for (int i = 0; i < handSize; i++)
        {
            DrawFromPlayerDeck();
        }
    }

    public void ShuffleHandBackIntoDeck()
    {
        playerDeckDisplay.deck.ShuffleHandBackIn(cardsInHand);

        cardsInHand.Clear();

        while (this.transform.childCount > 0) {
            DestroyImmediate(this.transform.GetChild(0).gameObject);
        }


    }

    public void SetPlayerDeck(Faction faction, Deck deck)
    {
        playerDeckDisplay.SetPlayerDeck(faction, deck);
    }

    public void BeginDragCard(CardDisplay cardDisplay)
    {
        Debug.Log("Hand: beginDragCard");
        Card card = cardDisplay.displayCard;

        //check if enough essence to play card
        bool haveEnoughEssence = CanPlayCard(card);

        Debug.Log("haveEnoughEssence = " + haveEnoughEssence);

        if(!haveEnoughEssence) {return;}

        Debug.Log("Hand: beginDragCard enough essence");

        //set card possibilities
        battleManager.SetCardPossibilities(card);
    }

    public void EndDragCard()
    {
        battleManager.ClearPossibilities();
    }

}
