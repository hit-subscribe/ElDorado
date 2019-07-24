using ElDorado.Domain;
using ElDorado.Gui.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElDorado.Gui.Controllers
{
    public class ReportsController : Controller
    {
        private readonly BlogContext _context;
        public DateTime Today { get; set; } = DateTime.Now;

        public ReportsController(BlogContext context)
        {
            _context = context;
        }

        public ActionResult PostHustling()
        {
            var allBlogPosts = _context.BlogPosts.ToList();
            var allAuthors = _context.Authors.ToList();
            var matchingPosts = allBlogPosts.Where(p => ShouldPostAppearInPostHustlingReport(p));

            var pairingsByDate = matchingPosts.Select(mp => new PostAuthorPairing(mp, allAuthors)).OrderBy(p => p.DraftDate);

            return View(new PostHustlingViewModel(pairingsByDate));
        }

        public ActionResult AuthorTimeliness()
        {
            var authors = _context.Authors.OrderBy(a => a.FirstName).ToList();
            return View(authors.Select(a => new AuthorTimelinessRecord(a)));
        }

        public ActionResult AccountsPayable(DateTime? userPickedDate = null)
        {
            var year = userPickedDate?.Year ?? 0;
            var month = userPickedDate?.Month ?? 0;

            var accountsPayableAuthorsForTheMonth = _context.Authors.ToList().Where(a => a.HasCompletedWorkInMonth(year, month)); //ToList here matters because without it, Linq tries to convert "HasCompletedWorkInMonth" to a SQL construct
            var accountsPayableEditorsForTheMonth = _context.Editors.ToList().Where(e => e.HasCompletedWorkInMonth(year, month));

            var viewModel = new AccountsPayableViewModel()
            {
                AuthorLedgers = accountsPayableAuthorsForTheMonth.Select(a => BuildAuthorLedger(a, year, month)).OrderBy(a => a.Name),
                EditorLedgers = accountsPayableEditorsForTheMonth.Select(e => BuildEditorLedger(e, year, month)).OrderBy(e => e.Name)
            };
            return View(viewModel);
        }

        private static PersonLedgerViewModel BuildEditorLedger(Editor e, int year, int month)
        {
            return new PersonLedgerViewModel()
            {
                Name = $"{e.FirstName} {e.LastName}",
                LineItems = e.BlogPosts.Where(bp => bp.DraftCompleteDate.MatchesYearAndMonth(year, month)).Select(bp => new LedgerLineItemViewModel(bp) { Cost = bp.EditorPay })
            };
        }

        private static PersonLedgerViewModel BuildAuthorLedger(Author a, int year, int month)
        {
            return new PersonLedgerViewModel()
            {
                Name = $"{a.FirstName} {a.LastName}",
                LineItems = a.BlogPosts.Where(bp => bp.DraftCompleteDate.MatchesYearAndMonth(year, month)).Select(bp => new LedgerLineItemViewModel(bp) { Cost = bp.AuthorPay }).Concat(
                                    a.PostRefreshes.Where(pr => pr.SubmittedDate.MatchesYearAndMonth(year, month)).Select(pr => new LedgerLineItemViewModel(pr) { Cost = pr.AuthorPay }))
            };
        }

        private bool ShouldPostAppearInPostHustlingReport(BlogPost post)
        {
            return post.Author == null && post.DraftDate > Today && post.IsApproved;
        }


    }
}