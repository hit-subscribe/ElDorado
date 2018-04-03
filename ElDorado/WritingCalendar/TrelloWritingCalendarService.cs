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

        private Lazy<Board> WritingCalender = new Lazy<Board>(() => new Board(TrelloBoardId));        
        

        private CardCollection PlannedPostCards => WritingCalender.Value.Lists.First(l => l.Name == PlannedPostTrelloListName).Cards;
        private IList<Card> BoardCards => WritingCalender.Value.Cards.ToList();

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
            //This isn't going to stay like this -- implement some extension methods and test 'em
            return BoardCards.ToList().Any(c => c.Name.Trim().ToLower().Contains(cardTitle.Trim().ToLower().Replace("  ", " ")));
        }

    }
}
