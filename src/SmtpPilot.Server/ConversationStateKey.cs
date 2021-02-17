using SmtpPilot.Server.States;
using System;
using System.Collections.Generic;

namespace SmtpPilot.Server
{
    internal struct ConversationStateKey : IEquatable<ConversationStateKey>
    {
        public ConversationStateKey(Type stateType)
        {
            StateType = stateType;
        }

        internal Type StateType { get; }

        public override bool Equals(object obj)
        {
            return obj is ConversationStateKey key && Equals(key);
        }

        public bool Equals(ConversationStateKey other)
        {
            return EqualityComparer<Type>.Default.Equals(StateType, other.StateType);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StateType);
        }

        public static bool operator ==(ConversationStateKey left, ConversationStateKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ConversationStateKey left, ConversationStateKey right)
        {
            return !(left == right);
        }
    }
}
