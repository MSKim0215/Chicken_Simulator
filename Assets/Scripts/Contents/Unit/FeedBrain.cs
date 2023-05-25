using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedBrain : CharacterBrain
{
    public FeedDNA DNA { private set; get; }        // À¯ÀüÀÚ DNA

    private void Start() { }

    public override void Init()
    {
        DNA = new FeedDNA();
        DNA.SetStatus(GetComponent<FeedStat>());
    }

    public void ClearDNA() => DNA = null;

    private void OnEnable()
    {
        Init();
    }
}