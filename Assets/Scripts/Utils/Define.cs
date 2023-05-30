using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum Layer
    {
        Monster = 6,
        Ground,
        Block
    }

    public enum Scene
    {
        Unknown,
        Login,
        Lobby,
        Game
    }

    public enum Sound
    {
        Bgm, Sfx, MaxCount
    }

    public enum UIEvent
    {
        Click
    }

    public enum CameraMode
    {
        QuarterView
    }

    public enum MouseEvent
    {
        Press, PointerDown, PointerUp, Click
    }

    public enum WorldObject
    {
        Unknown, ChickGroup, FoxGroup, NeutralGroup
    }

    public enum CharacterState
    {
        Idle, Moving, Eat, Die
    }

    public enum ChickenType
    {
        None, Egg, Chick, Chicken
    }

    public enum FeedMakeCount
    {
        AM = 15, PM = 30
    }

    public const int TableLabel = 1;
}