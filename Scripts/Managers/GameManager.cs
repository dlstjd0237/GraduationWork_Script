using BIS.Data;
using System.Collections.Generic;

namespace BIS.Manager
{
    public class GameManager
    {
        public List<EnemyPartySO> EnemyPartySOs { get; set; }

        private bool _isTutorialComplete;

        public bool IsTutorialComplete
        {
            get => _isTutorialComplete;
            set => _isTutorialComplete = value;
        }

        public GameManager()
        {
        }

        public bool IsTutorialCompleted() => _isTutorialComplete == false ? true : false;





    }
}
