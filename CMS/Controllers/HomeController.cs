using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CMS.Models;
using Domain.Interfaces;
using Infrastructure.Data;

namespace CMS.Controllers;

public class HomeController : Controller
{
    private readonly IPostRepository _postRepository;

    public HomeController(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<IActionResult> Index()
    {
        var post = await _postRepository.GetByTitleAsync("Home");
        return View(post);
    }

    public async Task<IActionResult> Privacy()
    {
        var post = await _postRepository.GetByTitleAsync("Privacy");
        return View(post);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
