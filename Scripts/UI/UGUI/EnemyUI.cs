using UnityEngine;

namespace BIS.UI
{
    public class EnemyUI : UIBase
    {
        private void LateUpdate()
        {
            transform.LookAt(Camera.main.transform);
            transform.Rotate(0f, 180f, 0f);
        }
    }
}
