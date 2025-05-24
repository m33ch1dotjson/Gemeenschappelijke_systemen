using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;

namespace CMS.Controllers
{
    public class EditorController : Controller
    {
        private readonly IPostRepository _postRepository;

        public EditorController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> EditPost(string title)
        {
            var post = await _postRepository.GetByTitleAsync(title);

            if (post == null)
            {
                post = new Post();
                post.SetTitle(title);

                _postRepository.AddAsync(post);
            }

            return View(post);
        }

        [HttpPost]

        public async Task<IActionResult> SavePost(string title, string content)
        {
            var post = await _postRepository.GetByTitleAsync(title);

            if (post == null)
            {
                return View("Error");
            }

            post.SetContent(content);
            await _postRepository.UpdateContent(post);

            return RedirectToAction(nameof(Index));
        }
    }
}
