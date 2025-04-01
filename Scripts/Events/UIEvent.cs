using BIS.Data;
using Main.Runtime.Core.Events;
using BIS.Shared.Interface;
using System.Collections.Generic;

namespace BIS.Events
{
    public static class UIEvent
    {
        public static readonly ShopInteractEvent ShopInteractEvent = new ShopInteractEvent();
        public static readonly ShopItemChoice ShopItemChoice = new ShopItemChoice();
        public static readonly OptionEvent OptionEvent = new OptionEvent();
        public static readonly LoadingUIEvent LoadingUIEvent = new LoadingUIEvent();
        public static readonly InventoryEvent InventoryEvent = new InventoryEvent();
        public static readonly AchievementUIEvent AchievementUIEvent = new AchievementUIEvent();
        public static readonly EnemyPriviewChoiceEvent EnemyPrevieChoiceEvent = new EnemyPriviewChoiceEvent();
        public static readonly EnemyPriviewUIEvent EnemyPreviewUIEvent = new EnemyPriviewUIEvent();
        public static readonly ThemePriviewUIEvent ThemePriviewUIEvent = new ThemePriviewUIEvent();
        public static readonly ThemPriviewChoiceEvent ThemPriviewChoiceEvent = new ThemPriviewChoiceEvent();
        public static readonly UnitChoiceUIEvent UnitChoiceUIEvent = new UnitChoiceUIEvent();

    }

    public class ShopInteractEvent : GameEvent
    {
        public bool isOpen;
    }

    public class ShopItemChoice : GameEvent
    {
        public ISynergyChoiceable shopItemUI;
    }

    public class OptionEvent : GameEvent
    {
        public bool isOpen;
    }

    public class InventoryEvent : GameEvent
    {
        public bool isOpen;
    }

    public class LoadingUIEvent : GameEvent
    {
        public bool isComplete;
    }

    public class EnemyPriviewUIEvent : GameEvent
    {
        public bool isOpen;
    }
    public class EnemyPriviewChoiceEvent : GameEvent
    {
        public EnemyPartySO enemyPartySO;
    }
    public class ThemePriviewUIEvent : GameEvent
    {
        public bool isOpen;
    }
    public class ThemPriviewChoiceEvent : GameEvent
    {
        public ThemeSO themeSO;
    }
    public class AchievementUIEvent : GameEvent
    {
        public AchievementSO choiceAchieve;
        public IAchievementChoiceable isClick;
    }

    public class UnitChoiceUIEvent : GameEvent
    {
        public UnitSO choiceUnit;
        public IUnitChoiceable isClick;

    }


}
