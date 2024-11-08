using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssenceCard : Card
{
    // public new EssenceCardData data;
    public EssenceCardData essenceCardData;

    private void Awake() {
        essenceCardData = (EssenceCardData) data;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CanTargetSpace(BoardSpace boardSpaces)
    {
        //TODO: add functionality
        return essenceCardData.CanTargetSpace(boardSpaces, targets);
    }

    public List<BoardSpace> GetTargatableSpaces(List<BoardSpace> board)
    {
        //TODO: add functionality
        return essenceCardData.GetTargatableSpaces(board, targets);
    }

    public void SelectTarget(BoardSpace boardSpace)
    {
        essenceCardData.essenceAction.SelectTarget(boardSpace, targets);
    }
}
