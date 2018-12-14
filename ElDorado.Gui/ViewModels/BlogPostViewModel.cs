using ElDorado.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ElDorado.Gui.ViewModels
{
    public class BlogPostViewModel
    {
        public IEnumerable<SelectListItem> Blogs { get; private set; } = Enumerable.Empty<SelectListItem>();

        public IEnumerable<SelectListItem> Authors { get; protected set; } = Enumerable.Empty<SelectListItem>();

        public BlogPostViewModel(BlogContext context)
        {
            if (context != null)
            {
                Blogs = context.Blogs.OrderBy(b => b.CompanyName).ToList().Select(b => new SelectListItem() { Text = b.CompanyName, Value = b.Id.ToString() });
                Authors = BuildAuthorsList(context, a => a.IsInOurSystems);
            }
        }

        protected IEnumerable<SelectListItem> BuildAuthorsList(BlogContext context, Func<Author, bool> selectionCriteria)
        {
            if (context == null)
                return Enumerable.Empty<SelectListItem>();

            var matchingAuthors = context.Authors.Where(selectionCriteria).OrderBy(a => a.FirstName).ToList();
            return matchingAuthors.Select(a => new SelectListItem() { Text = $"{a.FirstName} {a.LastName}", Value = a.Id.ToString() });
        }
    }
}