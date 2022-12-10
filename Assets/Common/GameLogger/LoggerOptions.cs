using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace SeedWork.GameLogs
{
    [Serializable]
    [CreateAssetMenu(fileName = "LoggerNameOptions", menuName = "GameLogger/Options")]
    public sealed class LoggerOptions : ScriptableObject // ToDo: Rename to GameLogOptions or LogOptions, LogWindowOptions, or just Options(bc namespace) 
    {
        [SerializeField] bool _show = false;
        [SerializeField] bool _inheritNumberOfLogsToShow = true;
        [Range(0.0f, GameLogger.MAX_NUMBER_LOGS_STORED)]
        [SerializeField] int _numberOfLogsToShow = 6;

        [Header("Font")]
        [SerializeField] bool _inheritFontColor = true;
        [SerializeField] Color _fontColor = Color.white;
        [SerializeField] bool _inheritFontSize = true;
        [SerializeField] int _fontSize = 32;

        public bool Show => _show;
        public bool InheritNumberOfLogsToShow => _inheritNumberOfLogsToShow;
        public int NumberOfLogsToShow => _numberOfLogsToShow;
        public bool InheritFontColor => _inheritFontColor;
        public Color FontColor => _fontColor;
        public bool InheritFontSize => _inheritFontSize;
        public int FontSize => _fontSize;
    }
}
