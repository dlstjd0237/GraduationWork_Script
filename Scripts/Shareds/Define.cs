using UnityEngine;

namespace BIS.Shared
{
    public static class Define
    {
        public const float UIDuration = 0.5f;

        public const string SRestrictionMessage = "현재 재화가 부족합니다. 재화를 모아서 다시 시도해 주세요.";

        public static readonly Color CBronze = new Color(0.6729559f, 0.3641879f, 0);
        public static readonly Color CSilver = new Color(0.627451f, 0.654902f, 0.6509804f);
        public static readonly Color CGold = new Color(0.7294118f, 0.572549f, 0);
        public enum EUIEventType
        {
            DOWN,
            MOVE,
            ENTER,
            EXIT,
            CLICK,
            FLOAT
        }

    }
    public enum EMainTabbar
    {
        MISSION,
        SKILL,
        APPEARANCE,
        PLAYGROUND,
        PROGRESSION
    }
    public enum EModelType
    {
        Int,
        Float,
        Bool
    }

    public enum EUIEvent    
    {
        Click,
        PointerDown,
        PointerUp,
        PointerMove,
        Drag,
        PointerEnter,
        PointerExit
    }
    public enum EScene
    {
        Unknown,
        TitleScene,
        GameScene,
        Battlefield
    }

    public enum ERarity
    {
        Bronze,
        Silver,
        Gold
    }

    public enum ESoundType
    {
        Master,
        VFX,
        BGM
    }
}

