using BIS.UI.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace BIS.UIToolkit
{
    public class ButtonModelView : UIBase<Button>
    {
        private Button _button;
        public Vector2 defualtPos;
        public ButtonModelView(ButtonData data)
        {
            _button = data.button;
            SetUpData();
        }


        protected override void SetUpData()
        {
            _ui = _button;
            defualtPos = new Vector2(_button.resolvedStyle.left, _button.resolvedStyle.top);
            base.SetUpData();
        }



    }
}
