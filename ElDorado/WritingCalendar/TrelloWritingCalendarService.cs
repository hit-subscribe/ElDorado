using Manatee.Trello;
using Manatee.Trello.ManateeJson;
using Manatee.Trello.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElDorado.WritingCalendar
{
    public class TrelloWritingCalendarService
    {
        public const string PlannedPostTrelloListName = "Planned Posts";
        public const string TrelloBoardId = "AhqnpUJD";


        private TrelloAuthorization Auth => TrelloAuthorization.Default;

        private CardCollection PlannedPostCards => new Board(TrelloBoardId).Lists.First(l => l.Name == PlannedPostTrelloListName).Cards;
        private IList<Card> BoardCards => new Board(TrelloBoardId).Cards.ToList();

        public void Initialize(CredentialStore credentialStore)
        {
            var serializer = new ManateeSerializer();
            TrelloConfiguration.Serializer = serializer;
            TrelloConfiguration.Deserializer = serializer;
            TrelloConfiguration.JsonFactory = new ManateeFactory();
            TrelloConfiguration.RestClientProvider = new WebApiClientProvider();

            Auth.AppKey = credentialStore["TrelloAppKey"];
            Auth.UserToken = credentialStore["TrelloUserToken"];
        }
        public virtual void AddCard(string cardName)
        {
            PlannedPostCards.Add(cardName);
        }
        public virtual bool DoesCardExist(string cardTitle)
        {
            var cards = BoardCards.ToList();
            var isMatch =  cards.Any(c => c.Name == cardTitle);
            return isMatch;
        }

    }
}
