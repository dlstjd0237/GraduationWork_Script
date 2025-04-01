using System.Collections;
using TMPro;
using UnityEngine;
using BIS.Data;
using System;
using BIS.Manager;
using PJH.Players;

namespace BIS.UI.Popup
{
    public class DialoguePopupUI : PopupUI
    {
        [SerializeField] private PlayerInputSO _inputSO;
        private DialogueSO _currentData;
        public event Action DialogueFinishEvent;

        private enum Texts
        {
            NameText,
            ContentText
        }

        public void ShowText(DialogueSO data, bool isFinishMove = false, bool isDontMove = true)
        {
            _currentData = data;
            if (isDontMove == true)
                _inputSO.EnablePlayerInput(false);

            BindTexts(typeof(Texts));

            StartCoroutine(ShowTextCoroutine(_currentData, isFinishMove));
        }

        private IEnumerator ShowTextCoroutine(DialogueSO data, bool isFinishMove = false)
        {
            TMP_Text text = GetText((int)Texts.ContentText);
            int dataLineLength = data.DialogueLines.Count;

            var wait = new WaitForSeconds(0.015f);

            for (int i = 0; i < dataLineLength; ++i)
            {
                GetText((int)Texts.NameText).text = data.DialogueLines[i].speaker;

                string line = data.DialogueLines[i].contents;
                text.text = "";

                for (int j = 0; j < line.Length; ++j)
                {
                    text.text += line[j];
                    yield return wait;

                    if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
                    {
                        text.text = line;
                        break; // Move Next Line
                    }
                }

                yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space));
            }

            data.DialogueFinishEvent?.Invoke();
            this.DialogueFinishEvent?.Invoke();
            this.DialogueFinishEvent = null;
            if (isFinishMove == true)
                _inputSO.EnablePlayerInput(true);
            Managers.UI.ClosePopupUI(this);
        }
    }
}