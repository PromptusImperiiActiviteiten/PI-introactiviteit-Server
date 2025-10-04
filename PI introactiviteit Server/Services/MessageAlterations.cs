using System.Text.RegularExpressions;

namespace PI_introactiviteit_Server.Services
{
    public class MessageAlterations
    {
        public static string RemoveProtocolFromMessage(string message)
        {
            string messageRegexString = @"^1\d{2}:";

            string clientName = RemoveByRegexString(message,messageRegexString);

            return clientName;
        }

        public static string GetTargetedClientNameFrom102Message(string message) {
            string messageEndRegexString = @";.*$";
            string protocolRegexString = @"^102:";

            string messageWithoutProtocol = RemoveByRegexString(message, protocolRegexString);
            string whisperClientName = RemoveByRegexString(messageWithoutProtocol, messageEndRegexString);

            return whisperClientName;
        }

        public static string RemoveClientNameAndProtocolFrom102Message(string message)
        {
            string messageEndRegexString = @"^.*;";

            string whisperClientName = RemoveByRegexString(message, messageEndRegexString);

            return whisperClientName;
        }


        public static string RemoveMessageFromProtocol(string message) {
            string protocolRegexString = @":.*$";

            string messageProtocol = RemoveByRegexString(message, protocolRegexString);
            
            return messageProtocol;
        }

        private static string RemoveByRegexString(string message, string regexString) {
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

        public static MessageProtocol GetProtocolFromMessage(string incommingClientMessage)
        {
            int intMessageProtocolCode;
            string stringMessageProtocolCode;

            stringMessageProtocolCode = MessageAlterations.RemoveMessageFromProtocol(incommingClientMessage);
            int.TryParse(stringMessageProtocolCode, out intMessageProtocolCode);
            MessageProtocol incommingMessageProtocol = (MessageProtocol)intMessageProtocolCode;

            return incommingMessageProtocol;
        }

    }
}
