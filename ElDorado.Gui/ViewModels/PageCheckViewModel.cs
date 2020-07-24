using ElDorado.Console.Refreshes;
using System;
using System.Linq;

namespace ElDorado.Gui.ViewModels
{
    public class PageCheckViewModel
    {
        private PageCheckResult _pageCheckResult;

        public string PageTitle => _pageCheckResult.PageTitle;

        public string PageUrl => _pageCheckResult.PageUrl; 

        public PageCheckViewModel(PageCheckResult pr)
        {
            _pageCheckResult = pr;
        }
    }
}