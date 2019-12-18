using UnityEngine;
using UnityEngine.UI;

namespace HeathenEngineering
{
    public class Console : MonoBehaviour
    {
        public int maxLines = 200;

        public Text text;
        public ScrollRect scrollRect;

        void OnEnable()
        {
            Application.logMessageReceived += HandleLog;
        }

        void OnDisable()
        {
            Application.logMessageReceived -= HandleLog;
        }

        void HandleLog(string logString, string stackTrace, LogType type)
        {
            Color color;

            switch (type)
            {
                case LogType.Error:
                case LogType.Exception:
                    color = Color.red;
                    break;
                case LogType.Warning:
                    color = Color.yellow;
                    break;
                default:
                    color = Color.white;
                    break;
            }

            text.text += "\n<color=#" + ColorUtility.ToHtmlStringRGB(color) + ">" + logString + "</color>";
            Canvas.ForceUpdateCanvases();
            if (text.cachedTextGenerator.lineCount > maxLines)
            {
                var firstLine = text.cachedTextGenerator.lines[text.cachedTextGenerator.lineCount - maxLines];
                text.text = text.text.Substring(firstLine.startCharIdx);
            }
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0f;
        }
    }
}