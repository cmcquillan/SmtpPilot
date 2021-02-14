using SmtpPilot.Server.Internal;
using System;

namespace SmtpPilot.Server.Conversation
{
    public class SmtpReply : ConversationElement
    {
        public static SmtpReply OK
        {
            get
            {
                return new SmtpReply(SmtpReplyCode.Code250, Constants.OKText);
            }
        }

        public static SmtpReply ServerClosing
        {
            get
            {
                return new SmtpReply(SmtpReplyCode.Code221, Constants.QuitText);
            }
        }

        public static SmtpReply BeginData
        {
            get
            {
                return new SmtpReply(SmtpReplyCode.Code354, Constants.BeginDataText);
            }
        }

        private readonly SmtpReplyCode _code;
        private string _text;

        public SmtpReply(SmtpReplyCode code)
            : this(code, null) { }

        public SmtpReply(SmtpReplyCode code, string text)
        {
            _code = code;
            _text = text;
        }

        public SmtpReplyCode Code { get { return _code; } }
        public string Text { get { return _text; } set { _text = value; } }

        public override string ToString()
        {
            return FullText + Constants.CarriageReturnLineFeed;
        }

        public override string Preamble
        {
            get { return ((int)Code).ToString(); }
        }

        public override string FullText
        {
            get { return String.Format("{0} {1}", Preamble, Text); }
        }

        public bool IsError
        {
            get
            {
                return (int)Code > (int)SmtpReplyCode.Code354;
            }
        }
    }

    public enum SmtpReplyCode : int
    {
        Code500 = 500,
        Code501 = 501,
        Code502 = 502,
        Code503 = 503,
        Code504 = 504,
        Code201 = 201,
        Code214 = 215,
        Code220 = 220,
        Code221 = 221,
        Code421 = 421,
        Code250 = 250,
        Code251 = 251,
        Code450 = 450,
        Code550 = 550,
        Code451 = 451,
        Code551 = 551,
        Code452 = 452,
        Code552 = 552,
        Code553 = 553,
        Code354 = 354,
        Code554 = 554,
    }
}
