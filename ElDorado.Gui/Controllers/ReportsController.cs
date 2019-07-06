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

            var authors = _context.Authors.ToList();
            var accountsPayableAuthorsForTheMonth = authors.Where(a => a.BlogPosts.Any(bp => bp.DraftCompleteDate.MatchesYearAndMonth(year, month)));

            var editors = _context.Editors.ToList();
            var accountsPayableEditorsForTheMonth = editors.Where(e => e.BlogPosts.Any(bp => bp.DraftCompleteDate.MatchesYearAndMonth(year, month)));

            var viewModel = new AccountsPayableViewModel()
            {
                AuthorLedgers = accountsPayableAuthorsForTheMonth.Select(a =>
                new PersonLedgerViewModel()
                {
                    Name = $"{a.FirstName} {a.LastName}",
                    Posts = a.BlogPosts.Where(bp => bp.DraftCompleteDate.MatchesYearAndMonth(year, month)).Select(bp => new PostLineItemViewModel(bp) { Cost = bp.AuthorPay })
                }).OrderBy(a => a.Name),

                EditorLedgers = accountsPayableEditorsForTheMonth.Select(e =>
                new PersonLedgerViewModel()
                {
                    Name = $"{e.FirstName} {e.LastName}",
                    Posts = e.BlogPosts.Where(bp => bp.DraftCompleteDate.MatchesYearAndMonth(year, month)).Select(bp => new PostLineItemViewModel(bp) { Cost = bp.EditorPay })
                }).OrderBy(e => e.Name)
            };
            return View(viewModel);
        }

        private bool ShouldPostAppearInPostHustlingReport(BlogPost post)
        {
            return post.Author == null && post.DraftDate > Today && post.IsApproved;
        }
    }
}