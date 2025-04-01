using System;
using System.Collections.Generic;
using UnityEngine;

namespace BIS.Data
{
    [CreateAssetMenu(menuName = "BIS/SO/Data/Dialogue")]
    public class DialogueSO : ScriptableObject
    {
        public Action DialogueFinishEvent;
        [SerializeField] private List<DialogueData> _dialogueLines; public List<DialogueData> DialogueLines { get { return _dialogueLines; } }
    }
}
