using System;
using UnityEngine.UIElements;

namespace BIS.UI.Data
{
    public struct ProgressBarData
    {
        //==== Data Info ====
        public float maxAmount;
        public float minAmount;
        public float currentAmount;
        public ProgressBar progressBar;
        //===================

        public ProgressBarData(float max, float min, float cur, ProgressBar bar)
        {
            maxAmount = max;
            minAmount = min;
            currentAmount = cur;
            progressBar = bar;
        }
    }
}
