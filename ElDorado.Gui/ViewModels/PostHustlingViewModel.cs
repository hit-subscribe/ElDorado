using ElDorado.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElDorado.Gui.ViewModels
{
    public class PostHustlingViewModel
    {
        public IEnumerable<BlogPost> UnclaimedPosts { get; private set; } = Enumerable.Empty<BlogPost>();

        public PostHustlingViewModel(IEnumerable<BlogPost> blogPosts)
        {
            UnclaimedPosts = blogPosts;
        }

    }
}