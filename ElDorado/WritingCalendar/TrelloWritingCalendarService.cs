using ElDorado.Domain;
using Manatee.Trello;
using Manatee.Trello.ManateeJson;
using Manatee.Trello.WebApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ElDorado.WritingCalendar
{
    public class TrelloWritingCalendarService
    {
        public const string PlannedPostTrelloListName = "Planned Posts";
        public const string TrelloBoardId = "AhqnpUJD";


        private TrelloAuthorization Auth => TrelloAuthorization.Default;

        private Lazy<Board> LazyWritingCalendar = new Lazy<Board>(() => new Board(TrelloBoardId));

        private Board WritingCalendar => LazyWritingCalendar.Value;
        

        private CardCollection PlannedPostCards => WritingCalendar.Lists.First(l => l.Name == PlannedPostTrelloListName).Cards;
        private IList<Card> BoardCards => WritingCalendar.Cards.ToList();

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
        public virtual void AddCard(BlogPost postToAdd)
        {
            var card = PlannedPostCards.Add(name: postToAdd.AuthorTitle, dueDate: postToAdd.DraftDate.SafeAddHours(12), 
                members: GetMemberWithUserName(postToAdd.AuthorTrelloUserName), labels: GetLabelsForCompany(postToAdd.BlogCompanyName));
            card.SetKeyword(postToAdd.Keyword);

            postToAdd.TrelloId = card.Id;
        }

        public virtual bool DoesCardExist(string blogPostTitle)
        {
            return BoardCards.ToList().Any(c => c.Name.TrelloCardLooselyMatches(blogPostTitle));
        }

        public virtual void EditCard(BlogPost post)
        {
            var card = WritingCalendar.Cards.FirstOrDefault(c => c.Id == post.TrelloId);
            if (card == null)
                return;

            card.Name = post.AuthorTitle;
            card.SetKeyword(post.Keyword);
            if (card.List.Name == "Planned Posts")
            {
                card.DueDate = post.DraftDate.SafeAddHours(12);
                card.UpdateLabels(GetLabelsForCompany(post.BlogCompanyName));
                card.UpdateMembers(GetMemberWithUserName(post.AuthorTrelloUserName));
            }
            
        }
        public virtual void DeleteCard(string trelloId)
        {
            var card = WritingCalendar.Cards.FirstOrDefault(c => c.Id == trelloId);
            if(card != null)
                card.Delete();
        }

        private IEnumerable<Label> GetLabelsForCompany(string companyName)
        {
            return WritingCalendar.Labels.Where(l => l.Name == companyName);
        }

        private IEnumerable<Member> GetMemberWithUserName(string trelloUserName)
        {
            return WritingCalendar.Members.Where(m => m.UserName == trelloUserName);
        }
    }
}
