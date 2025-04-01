using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using BIS.Shared.Interface;
using Main.Runtime.Manager;
using Main.Runtime.Agents;
using PJH.Players;

namespace BIS.UI
{
    public class PostureProgressUI : UIBase
    {
        [SerializeField] private float _fillDuration = 0.25f;
        private readonly int _shaderPropertyID = Shader.PropertyToID("_InnerOutlineFade");
        private Material _iconMat;

        private IEffectable effectable;

        private enum Images
        {
            left_fill_contain,
            right_fill_contain,
            fill_icon_contain,
            fill_max_icon_efect_contain
        }

        public override bool Init()
        {
            if (base.Init() == false)
                return false;

            BindImages(typeof(Images));

            _iconMat = GetImage((int)Images.fill_icon_contain).material;
            effectable = GetImage((int)Images.fill_max_icon_efect_contain).GetComponent<IEffectable>();


            return true;
        }

        public virtual void SetUpProgress(float current, float max)
        {
            float endValue = current / max;
            Image leftFill = GetImage((int)Images.left_fill_contain);
            Image rightFill = GetImage((int)Images.right_fill_contain);

            Color endColor = leftFill.color;
            endColor.g = 1 - endValue;

            leftFill.DOColor(endColor, _fillDuration);
            rightFill.DOColor(endColor, _fillDuration);

            leftFill.DOFillAmount(endValue, _fillDuration);
            rightFill.DOFillAmount(endValue, _fillDuration);


            Effect(endValue);
        }

        protected void Effect(float progress)
        {
            if (1 <= progress)
            {
                _iconMat.DOFloat(1, _shaderPropertyID, 0.2f);
                effectable.PlayEffect();
            }
            else
            {
                _iconMat.DOFloat(progress, _shaderPropertyID, 0.2f);
            }
        }

        private void OnDestroy()
        {
            _iconMat.DOFloat(0, _shaderPropertyID, 0);
        }
    }
}