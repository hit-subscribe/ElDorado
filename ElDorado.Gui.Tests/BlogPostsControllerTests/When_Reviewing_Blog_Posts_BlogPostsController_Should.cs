using ElDorado.Domain;
using ElDorado.Gui.Controllers;
using ElDorado.Gui.ViewModels;
using ElDorado.Trello;
using ElDorado.Wordpress;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.JustMock;
using Telerik.JustMock.EntityFramework;
using Telerik.JustMock.Helpers;

namespace ElDorado.Gui.Tests.BlogPostsControllerTests
{
    [TestClass]
    public class When_Reviewing_Blog_Posts_BlogPostsController_Should
    {
        private const string PostTitle = "5 Tips to Help You Suck at Math";
        private const int PostId = 12;
        private const int WordpressId = 43;

        private static string Content = $@"<p>Imagine an office. Hundreds of employees fill the office. One of the main duties of these employees is to make copies using one of the many copy machines throughout the office. The air smells of toner and the carpet is worn to threads in front of each copy station.</p>{Environment.NewLine}<p>Jerry needs one hundred copies of the annual sales report. Samantha needs seventy copies of the updated mission statement. Bradley needs ninety copies of the ""missing dog"" poster for the furry office mascot.</p>{Environment.NewLine}<p>Now imagine that nobody in the office knows that the machines can make more than one copy at a time.</p>{Environment.NewLine}<p>The costs for employees standing around the copiers would be through the roof. The company might well sink; all for want of a little more training with the copy machines.</p>{Environment.NewLine}<p>Clearly, the situation would be much better if everyone using the tools available to them were aware of and comfortable using them to their fullest extent.</p>{Environment.NewLine}<p>As long as you ignore the fact that a copy machine isn't really ""digital,"" this leads us to a good definition for the term at hand:</p>{Environment.NewLine}<p><strong>Digital adoption is leveraging an existing tool to its fullest extent.</strong></p>{Environment.NewLine}<p>Software is developed intentionally; every facet built for a purpose. If a piece of software is not used to its fullest potential, the user may be losing out on valuable experiences, optimizations, simplifications, and more.</p>{Environment.NewLine}<p>There's tremendous value to be found by organizations leveraging digital adoption as part of their business goals. Read on to discover what digital adoption really means and how you and your organization can benefit from it.</p>{Environment.NewLine}<h2>Double Dip With Digital Adoption</h2>{Environment.NewLine}<p>There are two primary applications of digital adoption: internal and external.</p>{Environment.NewLine}<p>Internally, organizations may leverage digital adoption to minimize dependencies and streamline processes. This can result in increased efficiency, reduced complexity, less stress in the workplace, and more effective onboarding.</p>{Environment.NewLine}<p>Those same organizations can leverage digital adoption externally through their products' end users. Confusing and complicated software can impact how well a product is received by the market and how long it's used, among other things.</p>{Environment.NewLine}<p>To get maximum benefits, organizations should strive for both.</p>{Environment.NewLine}<p>Now that you have a decent idea of what digital adoption is, let's dig into why you should care.</p>{Environment.NewLine}<h3>Digital Adoption Within Your Organization</h3>{Environment.NewLine}<p>Digital tools are commonplace in every modern business. Think about things like customer relationship management, project management, time logging, team communication, and content creation. All these common processes require tools.</p>{Environment.NewLine}<p>The ultimate goal for any business paying for these tools is to earn a return on their investment. That ROI can come in the form of improved efficiency, increased sales, decreased operational expenses, and a number of other improvements.</p>{Environment.NewLine}<p>It would make sense, then, that any tools adopted by the company be leveraged to their fullest extent to maximize such benefits. Take the case of the copier story at the beginning of this article, for instance. It wasn't enough for the company to simply purchase the copiers. Indeed, the employees needed proper training to use them effectively to save time and, ultimately, paid work hours.</p>{Environment.NewLine}<p>There are a number of excellent reasons to focus on digital adoption—that is, best leveraging your tools—within your organization.</p>{Environment.NewLine}<h4>Digital Adoption and Onboarding, a Beautiful Pair</h4>{Environment.NewLine}<p>Employee retention is an important metric for most companies. Hiring is expensive and time-consuming. As it turns out, new hires are <a href=""https://www.shrm.org/resourcesandtools/hr-topics/talent-acquisition/pages/dont-underestimate-the-importance-of-effective-onboarding.aspx"">much more likely to stay</a> with a company when provided with an effective onboarding process. Digital adoption can decrease some mental burden for new hires, allowing them to assimilate information more readily.</p>{Environment.NewLine}<p>Employee morale is another important subject for organizations. Stress is hardly welcome in the workplace. Poor onboarding processes, as well as tedious day-to-day tasks, can have adverse effects on employees. Reducing the number of tools new hires are expected to use during the onboarding process, through digital adoption, makes for a much less stressful experience.</p>{Environment.NewLine}<p>On the subject of onboarding, another common problem that can be mitigated by focusing on digital adoption is that of ""information overload."" New hires are often tasked with figuring out how to use all the various digital tools within an organization in a short period of time. Traditional training, such as group demos, webinars, and pre-recorded videos, are dense with information, much of which is often forgotten. Embracing digital adoption can help remedy these issues. Digital adoption typically includes ""contextual information"" such that help with tools or processes is provided only at the point that it's required; new hires shouldn't have to take notes on a once-a-year process that they won't run into for eleven more months.</p>{Environment.NewLine}<p>By fully leveraging digital onboarding and training tools, employees will feel much more comfortable with the technologies they're using. Plus, they'll be less stressed. You'll see productivity improve as information retention improves.</p>{Environment.NewLine}<h3>Digital Adoption by Your Customers</h3>{Environment.NewLine}<p>Your customers use your product to meet a desire or need. Just as digital adoption within an organization can help employees, its benefits can extend to end users to more effectively meet their needs.</p>{Environment.NewLine}<p>There are a few key points to consider when thinking about implementing a digital adoption strategy for your customers.</p>{Environment.NewLine}<p>For the most part, users expect instant gratification, with respect to technology. Things are just supposed to work. If they don't, or if they take too long to figure out, people will gladly abandon a product. Such experiences aren't exactly the hallmark of a successful product. You can increase customer retention by reducing the learning curve for your product through a digital adoption strategy. This strategy will look different from organization to organization, but typically will include things like contextual guidance (help when needed, no earlier), feature surfacing (reminding users of little-known features), and tool chain simplification (reducing the number of tools used for any given process).</p>{Environment.NewLine}<p>Further, if you prioritize the process of feature discovery within your product, it can drastically reduce the number of support requests from your users. Often, your customers will first use your product because they believe it offers desirable features. It's one problem if they then quickly abandon the product because they can't figure out how to use it, but you might also face another problem as a result of frustrated, confused users: an inundation of your customer support channels with requests for help. This is an unnecessary waste of both your time and theirs.</p>{Environment.NewLine}<p>You shouldn't forget about digital adoption for existing users of your products, either. Digital tools are constantly being updated, but humans are creatures of habit. Neglecting to provide adequate support for design or functionality changes to your products can leave even seasoned users unhappy.</p>{Environment.NewLine}<h2>Digital Adoption in the Future</h2>{Environment.NewLine}<p>A focus on digital adoption will only become more important as time goes on and the digital landscape becomes more crowded. Providing a more holistic approach to usability and guidance for digital product users will be one of the differentiating factors for successful products.</p>{Environment.NewLine}<p>Offering a rich feature set, beautiful interface, broad array of third-party integrations, or supreme performance is no longer enough. Today's digital consumers and workforce expect a complete, enjoyable experience from their tools, and the companies that embrace this fact will come out ahead.</p>{Environment.NewLine}<p>Digital adoption doesn't have to be a huge leap, by the way. It's unlikely that you can assess, as a whole, an organization's level of adoption of an entire suite of tools. However, you can consider tools individually and decide whether they're being used to their fullest extent. And corrections can be made when it's most convenient.</p>{Environment.NewLine}<p>On the customer side of things, organizations can make efforts to more regularly publish or broadcast notifications about lesser used areas of their products. For new users, companies may opt to leverage tools that can create a guided tour of the product's complete set of features. A step further would be to implement contextual help for users such that they can quickly learn how to leverage a product's features when it makes sense for them to do so.</p>{Environment.NewLine}<p>Why not explore what digital adoption might look like for you and your organization? While it's a fairly simple idea, the upsides can be tremendous. There could very well be less complexity, less stress, more efficiency, and more profitability in your future.</p>{Environment.NewLine}";

        private BlogContext Context { get; } = EntityFrameworkMock.Create<BlogContext>();

        private WordpressService WordpressService { get; set; } = Mock.Create<WordpressService>();


        private BlogPost Post { get; set; } = new BlogPost()
        {
            Id = PostId,
            Title = PostTitle,
            WordpressId = WordpressId, 
            Content = Content
        };

        private BlogPostsController Target { get; set; }

        [TestInitialize]
        public void BeforeEachTest()
        {
            WordpressService.Arrange(ws => ws.AuthorizeUser(Arg.AnyString));
            WordpressService.Arrange(wps => wps.GetBlogPostById(Post.WordpressId)).Returns(Post);

            Context.BlogPosts.Add(Post);

            Target = new BlogPostsController(Context, Mock.Create<TrelloWritingCalendarService>(), WordpressService) { MapPath = "Doesn't matter" };
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Authorize_Wordpress()
        {
            WordpressService.Arrange(ws => ws.AuthorizeUser(Arg.AnyString));
            WordpressService.Arrange(wps => wps.GetBlogPostById(Post.WordpressId)).Returns(Post);

            Target.Review(Post.Id);

            WordpressService.Assert(ws => ws.AuthorizeUser(Arg.AnyString));
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_With_Word_Count_Equal_To_Post_Content_Word_Count()
        {
            var viewModel = Target.Review(Post.Id).GetResult<PostReviewViewModel>();

            viewModel.WordCount.ShouldBe(1372);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Return_A_ViewModel_With_Title_Set_To_BlogPost_Title()
        {
            var viewModel = Target.Review(Post.Id).GetResult<PostReviewViewModel>();

            viewModel.Title.ShouldBe(Post.Title);
        }

        [TestMethod, Owner("ebd"), TestCategory("Proven"), TestCategory("Unit")]
        public void Save_To_Context_When_Retrieving_Wordpress_Contents()
        {
            Context.Arrange(ctx => ctx.SaveChanges());

            Target.Review(Post.Id);

            Context.Assert(ctx => ctx.SaveChanges());
        }
    }
}
