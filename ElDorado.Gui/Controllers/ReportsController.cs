using ElDorado.Domain;
using ElDorado.Gui.ViewModels;
using System;
using System.Collections.Generic;
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
            var authors = _context.Authors.ToList();
            return View(authors.Select(a => new AuthorTimelinessRecord(a)));
        }

        private bool ShouldPostAppearInPostHustlingReport(BlogPost post)
        {
            return post.Author == null && post.DraftDate > Today && post.IsApproved;
        }
    }
}