using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CMS.Controllers
{
    public class FacilitiesController : Controller
    {

        private readonly IPostRepository _postRepository;

        public FacilitiesController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        public async Task<IActionResult> Index()
        {
            var post = await _postRepository.GetByTitleAsync("Faciliteiten");
            return View(post);
        }
    }
}
