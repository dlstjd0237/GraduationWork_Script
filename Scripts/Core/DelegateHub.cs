using UnityEngine;

namespace BIS.Core
{
    public class DelegateHub {}
    public delegate void ValueChangeEvent(float currentVlaue, float maxValue, float minValue);
    public delegate void SoundValueChangeEvent(float currentVlaue);
}
