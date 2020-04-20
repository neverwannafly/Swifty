using System.Collections.Generic;
using Newtonsoft.Json;

namespace swifty {
    public class Logger {
        private IDictionary<string, List<string>> _logger {get;}
        public Logger() {
            _logger = new Dictionary<string, List<string>>();
        }
        public void AddLog(string key, string log) {
            if (!_logger.ContainsKey(key))
                // _logger.Remove(key);
                _logger.Add(key, new List<string>());
            _logger[key].Add(log);
        }
        public void PrintLog() {
            var json = JsonConvert.SerializeObject(_logger, Formatting.Indented);
            System.IO.File.WriteAllText("result.json", json);
        }
    }
}