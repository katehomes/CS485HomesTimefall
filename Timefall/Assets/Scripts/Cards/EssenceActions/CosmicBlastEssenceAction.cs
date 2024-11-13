using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New COSMIC BLAST Essence Action", menuName = "Essence Action/COSMIC BLAST")]
[System.Serializable]
public class CosmicBlastEssenceAction : EssenceAction
{
    public Texture COSMIC_BLAST_TEX;

    public Texture2D CURSOR_COSMIC_BLAST_TEX;

    public override bool CanBePlayed(List<BoardSpace> potentialBoardTargets, List<CardDisplay> potentialHandTargets)
    {
        return potentialBoardTargets.Count >= 1; 
    }

    public override bool CanTargetSpace(BoardSpace boardSpace, List<BoardSpace> boardTargets)
    {   
        //must have an agent & agent must not be sheilded
        if(!boardSpace.hasAgent || boardSpace.agentCard.shielded) { return false ;}

        return true;
    }

    public override bool CanTargetHandDisplay(CardDisplay handDisplay, List<CardDisplay> handTargets)
    { 
        return false;
    }

    public override List<BoardSpace> GetTargatableSpaces(List<BoardSpace> spacesToTest, List<BoardSpace> boardTargets)
    {
        List<BoardSpace> targetableSpaces = new List<BoardSpace>();

        if(boardTargets.Count == 1){ return targetableSpaces;}

        foreach (BoardSpace boardSpace in spacesToTest)
        {
            if(!CanTargetSpace(boardSpace, boardTargets)) { continue;}

            targetableSpaces.Add(boardSpace);
        }

        return targetableSpaces;
    }

    public override List<CardDisplay> GetTargatableHandDisplays(List<CardDisplay> handDisplays, List<CardDisplay> handTargets)
    {
        return new List<CardDisplay>();
    }

    public override Texture GetSelectionTexture(List<BoardSpace> boardTargets, List<CardDisplay> handTargets)
    {
        if(boardTargets.Count == 0)
        {
            return COSMIC_BLAST_TEX;
        }

        return null;
    }

    public Texture2D GetCursorTexture(List<BoardSpace> boardTargets)
    {
        if(boardTargets.Count == 0)
        {
            return CURSOR_COSMIC_BLAST_TEX;
        }

        return null;
    }

    public override void SelectTarget(BoardSpace boardSpace, List<BoardSpace> boardTargets, List<CardDisplay> handTargets)
    {
        if(boardTargets.Count == 0)
        {
            boardTargets.Add(boardSpace);

            Cursor.SetCursor(GetCursorTexture(boardTargets), Vector2.zero, CursorMode.Auto);

            Texture selectionTexture = GetSelectionTexture(boardTargets, handTargets);
            boardSpace.SelectAsTarget(selectionTexture);
        }
        
        Hand.Instance.UpdatePossibilities();

        if(boardTargets.Count == 1)
        {
            CosmicBlast(boardTargets);
        }
    }

    public override void SelectTarget(CardDisplay handTarget, List<BoardSpace> boardTargets, List<CardDisplay> handTargets)
    {
        return;
    }

    public override void StartAction(List<BoardSpace> boardTargets, List<CardDisplay> handTargets)
    {
        Cursor.SetCursor(GetCursorTexture(boardTargets), Vector2.zero, CursorMode.Auto);
    }

    public override void EndAction(List<BoardSpace> boardTargets, List<CardDisplay> handTargets)
    {
        //set handstate
        Hand hand = Hand.Instance;
        hand.SetHandState(HandState.ACTION_END);
        
        //reset cursor
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        //reset boardTargets
        foreach (var targetedSpace in boardTargets)
        {
            targetedSpace.DeselectAsTarget();
        }

        hand.RemoveCardAfterPlaying();
    }

    private void CosmicBlast(List<BoardSpace> boardTargets)
    {
        BoardSpace target = boardTargets[0];

        AgentCard agentToDiscard = target.agentCard;
        
        target.RemoveAgentCard();

        //send agent to its factions discard pile
        BattleManager.Instance.DiscardToDeck(agentToDiscard, agentToDiscard.GetFaction());
        
        EndAction(boardTargets, null);
    }
}