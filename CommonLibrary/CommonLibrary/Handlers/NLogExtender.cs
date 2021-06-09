using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Handlers
{
    public class NLogExtender
    {
        private Logger _logger;
        private string _postfix = null;
        private string _suffix = null;
        private bool _timeStamped = true;

        public NLogExtender(Logger logger, string postfix = null, string suffix = null, bool timeStamped = true)
        {
            _logger = logger;

            if (postfix != null)
                _postfix = postfix;

            if (suffix != null)
                _suffix = suffix;

            _timeStamped = timeStamped;
        }

        public void Trace(string message)
        {
            _logger.Trace(ObtainMessage(message));
        }

        public void Debug(string message)
        {
            _logger.Debug(ObtainMessage(message));
        }

        public void Warn(string message)
        {
            _logger.Warn(ObtainMessage(message));
        }

        public void Error(string message)
        {
            _logger.Error(ObtainMessage(message));
        }

        public string ObtainMessage(string message)
        {
            string retMessage = message;

            if (_suffix != null)
                retMessage = $"{_suffix}:{retMessage}";

            if (_postfix != null)
                retMessage = $"{retMessage}:{_postfix}";

            if (_timeStamped)
                retMessage = $"{retMessage}:{DateTimeOffset.Now}";

            return retMessage;
        }
    }
}
