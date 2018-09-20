using ElDorado.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElDorado.Gui.ViewModels
{
    public class PostHustlingViewModel
    {
        public IEnumerable<PostAuthorPairing> UnclaimedPosts { get; private set; } = Enumerable.Empty<PostAuthorPairing>();

        public PostHustlingViewModel(IEnumerable<PostAuthorPairing> blogPosts)
        {
            UnclaimedPosts = blogPosts;
        }

    }
}