using ElDorado.Domain;
using Manatee.Trello;
using Manatee.Trello.ManateeJson;
using Manatee.Trello.WebApi;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace ElDorado.Trello
{
    public class TrelloWritingCalendarService
    {
        private TrelloAuthorization Auth => TrelloAuthorization.Default;

        private IWritingCalendarBoard _board;

        public TrelloWritingCalendarService(IWritingCalendarBoard board = null)
        {
            _board = board ?? new WritingCalendarBoard();
        }

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
            var card = _board.AddPlannedPostCard(name: postToAdd.AuthorTitle, dueDate: postToAdd.DraftDate.SafeAddHours(12), trelloUserName: postToAdd.AuthorTrelloUserName, companyName: postToAdd.BlogCompanyName);
            card.SetKeyword(postToAdd.Keyword);
            card.IsArchived = !postToAdd.IsApproved;

            postToAdd.TrelloId = card.Id;
        }

        public virtual bool DoesCardExistWithTitle(string blogPostTitle)
        {
            return _board.AllCards.Any(c => c.Name.TrelloCardLooselyMatches(blogPostTitle));
        }

        public virtual void EditCard(BlogPost postToEdit)
        {
            var card = _board.AllCards.FirstOrDefault(c => c.Id == postToEdit.TrelloId);
            if (card == null)
                return;

            card.Name = postToEdit.AuthorTitle;
            card.IsArchived = !postToEdit.IsApproved;
            card.SetKeyword(postToEdit.Keyword);
            if (card.ListName == "Planned Posts")
            {
                card.DueDate = postToEdit.DraftDate.SafeAddHours(12);
                card.UpdateLabels(_board.GetLabelsForCompany(postToEdit.BlogCompanyName));
                card.UpdateMembers(_board.GetMemberWithUserName(postToEdit.AuthorTrelloUserName));
            }
            
        }
        public virtual void DeleteCard(string trelloId)
        {
            var card = _board.AllCards.FirstOrDefault(c => c.Id == trelloId);
            if(card != null)
                card.Delete();
        }
    }
}
