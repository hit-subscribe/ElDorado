using ElDorado.Console.Trello;
using ElDorado.Console.Wordpress;
using ElDorado.Domain;
using ElDorado.Gui.ViewModels;
using System;
using System.Linq;
using System.Web.Mvc;

namespace ElDorado.Gui.Controllers
{
    public class BlogPostsController : Controller
    {
        private readonly BlogContext _blogContext;
        private readonly WritingCalendarService _writingCalendarService;
        private readonly WordpressService _wordpressService;

        public string MapPath { get; set; }

        public BlogPostsController(BlogContext blogContext, WritingCalendarService writingCalendarService, WordpressService wordpressService)
        {
            _wordpressService = wordpressService;
            _writingCalendarService = writingCalendarService;
            _blogContext = blogContext;
        }

        public ActionResult Index(int blogId = 0, int authorId = 0, DateTime? draftDate = null, bool includeAll = false)
        {
            var postsFromEvaluatedDatabaseQuery = _blogContext.BlogPosts.ToList(); //I like the readability from these filtering methods we're using, and the cost tradeoff here just doesn't matter

            var filteredPosts = postsFromEvaluatedDatabaseQuery.Where(bp => bp.BlogId.DefaultMatches(blogId) && bp.AuthorId.DefaultMatches(authorId) && bp.DraftDate.DefaultMatches(draftDate));
            var currentPosts = filteredPosts.Where(bp => includeAll || (!bp.IsHitSubscribeFinished && bp.IsApproved));
            var orderedPosts = currentPosts.OrderBy(p => p.DraftDate);

            return View(new BlogPostIndexViewModel(orderedPosts, _blogContext));
        }

        public ActionResult Create(int blogId = 0)
        {
            return View(new BlogPostEditViewModel(new BlogPost() { BlogId = blogId }, _blogContext));
        }

        [HttpPost]
        public ActionResult Create(BlogPost post, string createNew = null)
        {
            #region Move to a service
            _blogContext.BlogPosts.Add(post);
            _blogContext.SaveChanges();
            _blogContext.UpdateBlogPostDependencies(post);

            InitializeTrelloService();
            _writingCalendarService.AddCard(post);

            post.CalculateAuthorPay();

            SyncToWordpress(post);

            _blogContext.SaveChanges();
            #endregion

            if (ModelState.IsValid)
                return RedirectToAppropriatePage(post, createNew);
            else
                return Create(post.BlogId);
        }

        public ActionResult Edit(int postId)
        {
            return View(GetViewModelForId(postId));
        }


        [HttpPost]
        public ActionResult Edit(BlogPostEditViewModel blogPostViewModel)
        {
            #region Move to a service
            _blogContext.BlogPosts.Attach(blogPostViewModel.Post);
            _blogContext.SetModified(blogPostViewModel.Post);
            _blogContext.UpdateBlogPostDependencies(blogPostViewModel.Post);
            blogPostViewModel.SetPay();

            if(blogPostViewModel.Post.WordpressId != 0)
               SyncToWordpress(blogPostViewModel.Post);

            InitializeTrelloService();
            _writingCalendarService.EditCard(blogPostViewModel.Post);

            _blogContext.SaveChanges();
            #endregion

            if (ModelState.IsValid)
                return RedirectToAction("Edit", new { postId = blogPostViewModel.Post.Id });
            else
                return Edit(blogPostViewModel.Post.Id);
        }

        public ActionResult Delete(int postId)
        {
            #region Move to a Service
            var post = _blogContext.BlogPosts.First(p => p.Id == postId);
            string trelloId = post.TrelloId;

            _blogContext.BlogPosts.Remove(post);
            _blogContext.SaveChanges();

            InitializeTrelloService();
            _writingCalendarService.DeleteCard(trelloId);

            AuthorizeWordpress();
            _wordpressService.DeleteFromWordpress(post);
            #endregion

            return RedirectToAction("Index");
        }

        public ActionResult Review(int postId)
        {
            var post = _blogContext.BlogPosts.First(p => p.Id == postId);

            AuthorizeWordpress();
            post.Content = _wordpressService.GetBlogPostById(post.WordpressId).Content;

            var viewModel = new PostReviewViewModel(post, new RenderedPostContents(post.Content));

            _blogContext.SaveChanges();
            return View(viewModel);
        }

        private ActionResult RedirectToAppropriatePage(BlogPost post, string createNew)
        {
            if (createNew == "Create and Add Another")
                return RedirectToAction("Create", new { blogId = post.BlogId });

            return RedirectToAction("Edit", new { postId = post.Id });
        }

        private void SyncToWordpress(BlogPost post)
        {
            try
            {
                AuthorizeWordpress();
                _wordpressService.SyncToWordpress(post);
            }
            catch(MissingAuthorException)
            {
                ModelState.AddModelError("Post.AuthorId", "Author not in Wordpress.");
            }
        }

        private BlogPostEditViewModel GetViewModelForId(int id)
        {
            var blogPost = _blogContext.BlogPosts.First(bp => bp.Id == id);
            return new BlogPostEditViewModel(blogPost, _blogContext);
        }

        private void InitializeTrelloService()
        {
            _writingCalendarService.Initialize(MapPath ?? Server.MapPath(@"~/App_Data/trello.cred"));
        }

        private void AuthorizeWordpress() => _wordpressService.AuthorizeUser(MapPath ?? Server.MapPath(@"~/App_Data/wordpress.cred"));
    }
}