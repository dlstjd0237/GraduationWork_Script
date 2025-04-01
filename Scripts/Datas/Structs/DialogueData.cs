using System;
using UnityEngine;

namespace BIS.Data
{
    [Serializable]
    public struct DialogueData
    {
        public string speaker;
        [TextArea(4, 3)] public string contents;
    }
}
