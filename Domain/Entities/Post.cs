using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Post
    {
        private string Title;
        private string Content;

        public Post() { }

        public Post(string title, string content)
        {
            Title = title;
            Content = content;
        }

        public string GetTitle()
        {
            return Title;
        }

        public string GetContent()
        {
            return Content;
        }

        public void SetTitle(string title)
        {
            Title = title;
        }

        public void SetContent(string content)
        {
            Content = content;
        }
    }

}
