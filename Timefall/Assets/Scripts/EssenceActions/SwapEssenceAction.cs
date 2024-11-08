using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Swap Essence Action", menuName = "Swap Essence Action")]
[System.Serializable]
public class SwapEssenceAction : EssenceAction
{
    public Texture CARD_TAP_UP_TEX;
    public Texture CARD_TAP_DOWN_TEX;

    public Texture2D CURSOR_CARD_TAP_UP_TEX;
    public Texture2D CURSOR_CARD_TAP_DOWN_TEX;

    public override bool CanTargetSpace(BoardSpace boardSpace, List<BoardSpace> targets)
    {
        Debug.Log("SwapEA: CanTargetSpace");

        EventCard eventCard = (EventCard) boardSpace.eventCard;
        AgentCard agentCard = boardSpace.agentCard;
        
        //must have an event & not have an agent & not already targeted
        if(eventCard == null || agentCard != null || boardSpace.isBeingTargeted) { return false ;}

        return true;
    }

    /* [SWAP]
     * 1. Targetable Space Criteria:
     *      - Has an Event
     *      - Doesnt have an agent
     * 2. Atleast 2 Targetable Spaces
     *      - if not, return empty list
    */
    public override List<BoardSpace> GetTargatableSpaces(List<BoardSpace> spacesToTest, List<BoardSpace> targets)
    {
        Debug.Log("SwapEA: GetTargatableSpaces");

        List<BoardSpace> targetableSpaces = new List<BoardSpace>();

        if(targets.Count == 2){ Debug.Log("SwapEA: 2 targets already");return targetableSpaces;}

        foreach (BoardSpace boardSpace in spacesToTest)
        {
            if(!CanTargetSpace(boardSpace, targets)) { continue;}

            targetableSpaces.Add(boardSpace);
        }

        //Must have atleast 2 plable spaces on board to swap
        if(targetableSpaces.Count < 2)
        {
            Debug.Log("SwapEA: not enough targets");
            targetableSpaces.Clear();
        }

        Debug.Log(string.Format("SwapEA: PS Count [{0}]", targetableSpaces.Count));

        return targetableSpaces;
    }

    public override Texture GetSelectionTexture(List<BoardSpace> targets)
    {
        Debug.Log("GetSelectionTexture:");
        if(targets.Count == 0)
        {
            return CARD_TAP_UP_TEX;
        } else if (targets.Count == 1) {
            return CARD_TAP_DOWN_TEX;
        }

        Debug.Log("GetSelectionTexture: returning null");

        return null;
    }

    public Texture2D GetCursorTexture(List<BoardSpace> targets)
    {
        Debug.Log("GetCursorTexture:");
        if(targets.Count == 0)
        {
            return CURSOR_CARD_TAP_UP_TEX;
        } else if (targets.Count == 1) {
            return CURSOR_CARD_TAP_DOWN_TEX;
        }

        Debug.Log("GetCursorTexture: returning null");

        return null;
    }

    public override void SelectTarget(BoardSpace boardSpace, List<BoardSpace> targets)
    {
        if(targets.Count < 2)
        {
            Texture selectionTexture = GetSelectionTexture(targets);
            boardSpace.SelectAsTarget(selectionTexture);
            targets.Add(boardSpace);

            Cursor.SetCursor(GetCursorTexture(targets), Vector2.zero, CursorMode.Auto);
        }
    }
}
