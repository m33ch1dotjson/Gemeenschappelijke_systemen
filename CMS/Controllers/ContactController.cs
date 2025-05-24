using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Controllers
{
    public class ContactController : Controller
    {
        private readonly IPostRepository _postRepository;

        public ContactController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task<IActionResult> Index()
        {
            var post = await _postRepository.GetByTitleAsync("Contact");
            return View(post);
        }
    }
}
