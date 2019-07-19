using Manatee.Trello;
using Manatee.Trello.ManateeJson;
using Manatee.Trello.WebApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElDorado.Trello
{
    public abstract class CalendarService
    {
        public const string PlannedPostsListName = "Planned Posts";
        
        protected ICalendarBoard Board { get; set; }
        
        private TrelloAuthorization Auth => TrelloAuthorization.Default;

        public virtual void Initialize(string filePath)
        {
            var credentialStore = new CredentialStore(File.ReadAllText(filePath));
            Initialize(credentialStore);
        }

        public virtual void Initialize(CredentialStore credentialStore)
        {
            var serializer = new ManateeSerializer();
            TrelloConfiguration.Serializer = serializer;
            TrelloConfiguration.Deserializer = serializer;
            TrelloConfiguration.JsonFactory = new ManateeFactory();
            TrelloConfiguration.RestClientProvider = new WebApiClientProvider();

            Auth.AppKey = credentialStore["TrelloAppKey"];
            Auth.UserToken = credentialStore["TrelloUserToken"];
        }

        public virtual void DeleteCard(string trelloId)
        {
            var card = Board.AllCards.FirstOrDefault(c => c.Id == trelloId);
            if (card != null)
                card.Delete();
        }

        public virtual bool DoesCardExistWithTitle(string blogPostTitle)
        {
            return Board.AllCards.Any(c => c.Name.TrelloCardLooselyMatches(blogPostTitle));
        }

    }
}
