using SeedWork.GameLogs;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

public class InGameLogger : MonoBehaviour
{
    static readonly List<GameLogger> _loggers = new();
    static Font _consolaFont;

    [SerializeField] int _fontSize = 32;
    [SerializeField] int _paddingRight = 10;  
    [SerializeField] int _paddingLeft = 10;
    [Range(1, GameLogger.MAX_NUMBER_LOGS_STORED)]
    [SerializeField] int _numberOfLogsToShowPerLogger = 5;
    [SerializeField] Color _textColor = Color.white;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
    [RuntimeInitializeOnLoadMethod]
    public static void Initialize()
    {
        var go = Instantiate(Resources.Load<GameObject>("InGameLogger"));
        _consolaFont = Resources.Load<Font>("CONSOLA");
        go.name = "InGameLogger";
        DontDestroyOnLoad(go);
    }
#endif

    void OnGUI()
    {
        try
        {
            GUIStyle defaultStyles = new GUIStyle(GUI.skin.box); // need default, to get the box Texture as background
            defaultStyles.wordWrap = false;
            defaultStyles.font = _consolaFont;
            defaultStyles.fontSize = _fontSize;
            defaultStyles.alignment = TextAnchor.MiddleLeft;
            defaultStyles.imagePosition = ImagePosition.TextOnly;
            defaultStyles.padding.top = 5;
            defaultStyles.padding.right = _paddingRight;
            defaultStyles.padding.bottom = 10;
            defaultStyles.padding.left = _paddingLeft;
            defaultStyles.normal.textColor = _textColor;

            Vector2 loggerScreenPosition = Vector2.zero;

            int biggestLogHeight = 0;
            for (int x = 0; x < _loggers.Count; x++)
            {
                GameLogger logger = _loggers[x];

                if (logger.Count == 0 || (logger.HasSpecificOptions && !logger.ShowLogWindow))
                    continue;

                StringBuilder logs = new StringBuilder($"{logger.Name}");
                for (int y = logger.Count - 1; y > -1; y--)
                {
                    int numberOfLogsAdded = logger.Count - (y + 1);
                    int numberOfLogsToShow = logger.HasSpecificOptions && !logger.Options.InheritNumberOfLogsToShow
                        ? logger.Options.NumberOfLogsToShow
                        : _numberOfLogsToShowPerLogger;

                    if (numberOfLogsAdded == numberOfLogsToShow)
                        break;

                    string log = logger.GetLog(y);
                    logs.Append($"\n{log}");
                }

                GUIStyle loggerStyles = GetLoggerStyles(logger, defaultStyles);
                GUIContent logContent = new GUIContent(logs.ToString(), logger.GameObject.name);
                
                Vector2 logSize = loggerStyles.CalcSize(logContent);

                if (biggestLogHeight < logSize.y)
                    biggestLogHeight = (int)logSize.y;

                Rect loggerRect = new Rect(loggerScreenPosition.x, loggerScreenPosition.y, logSize.x, logSize.y);

                float remainingScreenWidth = Screen.width - (loggerScreenPosition.x + logSize.x);
                bool remaining_screen_width_is_not_enough = remainingScreenWidth < 0.0f;
                if (remaining_screen_width_is_not_enough)
                {
                    loggerScreenPosition.x = 0.0f;
                    loggerScreenPosition.y += biggestLogHeight;
                    biggestLogHeight = 0;
                    loggerRect.x = loggerScreenPosition.x;
                    loggerRect.y = loggerScreenPosition.y;
                }

                loggerScreenPosition.x += logSize.x;
                
                GUI.Box(loggerRect, logContent, loggerStyles);
            }
        }
        catch (System.Exception ex)
        {
            this.LogError(ex.Message + ex.StackTrace);
        }
    }

    private GUIStyle GetLoggerStyles(GameLogger logger, GUIStyle defaultStyles)
    {
        if (!logger.HasSpecificOptions)
            return defaultStyles;

        var loggerStyles = new GUIStyle(defaultStyles);

        if (!logger.Options.InheritFontColor)
            loggerStyles.normal.textColor = logger.Options.FontColor;

        if (!logger.Options.InheritFontSize)
            loggerStyles.fontSize = logger.Options.FontSize;

        return loggerStyles;
    }

    [Conditional("UNITY_EDITOR")]
    public static void Subscribe(GameLogger gameLogger)
    {
        Assert.IsNotNull(gameLogger, $"Cannot add empty {nameof(GameLogger)} to {nameof(InGameLogger)}.");
        Assert.IsFalse(
            _loggers
            .Select(logger => logger.Name)
            .Contains(gameLogger.Name), $"{nameof(GameLogger)} with the name '{gameLogger.Name}' has already been added.");

        _loggers.Add(gameLogger);
    }

    [Conditional("UNITY_EDITOR")]
    public static void Unsubscribe(GameLogger gameLogger)
    {
        Assert.IsTrue(
            _loggers
            .Contains(gameLogger), $"Cannot unregister {nameof(GameLogger)}, because it is not registered in {nameof(InGameLogger)}.");

        _loggers.Remove(gameLogger);
    }
}
