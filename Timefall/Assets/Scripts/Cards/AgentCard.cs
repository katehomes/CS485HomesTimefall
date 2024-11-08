using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentCard : Card
{
    public AgentCardData agentCardData;

    private void Awake() {
        agentCardData = (AgentCardData) agentCardData;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
