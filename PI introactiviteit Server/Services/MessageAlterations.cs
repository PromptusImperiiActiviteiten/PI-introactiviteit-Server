using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using System.Windows.Forms;

namespace PI_introactiviteit_Server.Services
{
    public class MessageAlterations
    {
        public static string IsolateMessageFromProtocol(string message)
        {
            string messageRegexString = @"^1\d{2}:";

            string clientName = IsolateByRegexString(message,messageRegexString);

            return clientName;
        }

        public static string IsolateClientNameFrom103Message(string message) {
            string messageEndRegexString = @";.*$";
            string protocolRegexString = @"^103:";

            string messageWithoutProtocol = IsolateByRegexString(message, protocolRegexString);
            string whisperClientName = IsolateByRegexString(messageWithoutProtocol, messageEndRegexString);

            return whisperClientName;
        }

        public static string IsolateProtocolFromMessage(string message) {
            string protocolRegexString = @":.*$";

            string messageProtocol = IsolateByRegexString(message, protocolRegexString);
            
            return messageProtocol;
        }

        private static string IsolateByRegexString(string message, string regexString) {
            Regex regex = new Regex(regexString);

            if (!HandleRegexCheck(message, regex)) return null;

            string messageProtocol = Regex.Replace(message, regexString, "");
            return messageProtocol;
        }

        public static Boolean HandleRegexCheck(string message, Regex regex)
        {
            if (regex.IsMatch(message))
            {
                return true;
            }
            return false;
        }
    }
}
