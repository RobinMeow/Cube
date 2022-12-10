using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace SeedWork.GameLogs
{
    [Serializable]
    public sealed class GameLogger
    {
        public const int MAX_NUMBER_LOGS_STORED = 20;

        byte _logCount = 0;
        public int Count => _logCount;

        readonly string _name = String.Empty;
        public string Name => _name;

        LoggerOptions _options;
        public LoggerOptions Options => _options;
        public bool ShowLogWindow => _options.Show;

        public bool HasSpecificOptions => _options != null;

        readonly IList<string> logs = new List<string>();
        static readonly ICollection<string> _uniqieNames = new List<string>();

        readonly GameObject _gameObject;
        public GameObject GameObject => _gameObject;
        readonly Func<float> _getTimeStamp;

        public GameLogger(string name, GameObject gameObject)
            : this(name, gameObject, null, null)
        {
        }

        public GameLogger(string name, GameObject gameObject, LoggerOptions options) 
            : this(name, gameObject, options, null) 
        {
        }

        public GameLogger(string name, GameObject gameObject, LoggerOptions options, Func<float> getTimeStamp)
        {
            Assert.IsFalse(String.IsNullOrWhiteSpace(name), $"{nameof(GameLogger)} requires a name. ");
            Assert.IsNotNull(gameObject, $"{nameof(gameObject)} passed into {nameof(GameLogger)} '{name}' may not be null.");

            _options = options;
            _gameObject = gameObject;
            _getTimeStamp = getTimeStamp;
            _name = name;
        }

        [Conditional("UNITY_EDITOR")]
        public void Log(string log)
        {
            if (_getTimeStamp != null)
                logs.Add($"[{_getTimeStamp():000.000}] {log}");
            else
                logs.Add(log);

            _logCount++;

            void DeleteOldestEntries()
            {
                if (_logCount == MAX_NUMBER_LOGS_STORED)
                {
                    logs.RemoveAt(0);
                    --_logCount;
                }
            }
            DeleteOldestEntries();
        }

        public string GetLog(int index)
        {
            return logs[index];
        }

        [Conditional("UNITY_EDITOR")]
        public void Subscribe()
        {
            InGameLogger.Subscribe(this);
        }

        [Conditional("UNITY_EDITOR")]
        public void Unsubscribe()
        {
            InGameLogger.Unsubscribe(this);
        }
    }
}
