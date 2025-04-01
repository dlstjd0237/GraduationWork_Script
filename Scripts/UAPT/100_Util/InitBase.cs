using UnityEngine;

namespace BIS.Initializes 
{
    public class InitBase : MonoBehaviour
    {
        [HideInInspector] public bool _init = false;
        public virtual bool Init()
        {
            if (_init)
                return false;

            _init = true;

            return true;
        }

        private void Awake()
        {
            Init();
        }
    }

}
