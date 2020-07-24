using ElDorado.Console.Trello;
using ElDorado.Domain;
using Manatee.Trello;
using Manatee.Trello.ManateeJson;
using Manatee.Trello.WebApi;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace ElDorado.Console.Trello
{
    public class WritingCalendarService : CalendarService
    {
        public WritingCalendarService(ICalendarBoard board = null)
        {
            Board = board ?? new CalendarBoard("AhqnpUJD");
        }

        public virtual void AddCard(BlogPost postToAdd)
        {
            var card = Board.AddPlannedPostCard(name: postToAdd.AuthorTitle, dueDate: postToAdd.DraftDate.SafeToMidnightEastern(), trelloUserNames: postToAdd.AuthorTrelloUserName, companyName: postToAdd.BlogCompanyName);
            card.BuildDescriptionFromBlogPost(postToAdd);
            card.IsArchived = !postToAdd.IsApproved;

            postToAdd.TrelloId = card.Id;
        }

        public virtual void EditCard(BlogPost postToEdit)
        {
            var card = Board.AllCards.FirstOrDefault(c => c.Id == postToEdit?.TrelloId);
            if (card == null)
                return;

            card.Name = postToEdit.AuthorTitle;
            card.BuildDescriptionFromBlogPost(postToEdit);
            card.IsArchived = !postToEdit.IsApproved;

            UpdatePlannedPostPropertiesIfApplicable(postToEdit, card);

        }

        private void UpdatePlannedPostPropertiesIfApplicable(BlogPost postToEdit, ITrelloCard card)
        {
            if (card.ListName == PlannedPostsListName)
            {
                card.DueDate = postToEdit.DraftDate.SafeToMidnightEastern();
                card.UpdateLabels(Board.GetLabelsForCompany(postToEdit.BlogCompanyName));
                card.UpdateMembers(Board.GetMembersWithUserNames(postToEdit.AuthorTrelloUserName, postToEdit.EditorTrelloUserName));
            }
        }
    }
}
