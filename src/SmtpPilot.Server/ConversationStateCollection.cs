using SmtpPilot.Server.States;
using System.Collections.Generic;
using System.Linq;

namespace SmtpPilot.Server
{
    internal class ConversationStateCollection
    {
        private readonly Dictionary<ConversationStateKey, IConversationState> _stateDictionary;

        public ConversationStateCollection(IEnumerable<IConversationState> states)
        {
            _stateDictionary = states
                .ToDictionary(p => new ConversationStateKey(p.GetType()));
        }

        public IConversationState Get<T>() => _stateDictionary[new ConversationStateKey(typeof(T))];

        public IConversationState Get(ConversationStateKey key) => _stateDictionary[key];
    }
}
