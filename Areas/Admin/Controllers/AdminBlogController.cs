using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcBlog.Data;
using MvcBlog.Models;

namespace MvcBlog.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AdminBlogController : Controller
    {
        private readonly MvcBlogContext _context;

        public AdminBlogController(MvcBlogContext context)
        {
            _context = context;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Posts.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .FirstOrDefaultAsync(m => m.ID == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostCreateViewModel post)
        {
            if (ModelState.IsValid)
            {
                Post postModel = new Post();
                postModel.Title = post.Title;
                postModel.Content = post.Content;
                postModel.PostDate = DateTime.Now;
                if (post.Image.FileName != null)
                {
                    postModel.ImageMimeType = post.Image.ContentType;
                    if (post.Image.Length > 0)
                    //Convert Image to byte and save to database
                    {
                        byte[] imageBytes = null;
                        using (var fileStream = post.Image.OpenReadStream())
                        using (var memoryStream = new MemoryStream())
                        {
                            fileStream.CopyTo(memoryStream);
                            imageBytes = memoryStream.ToArray();
                        }
                        postModel.Image = imageBytes;
                    }
                }

                _context.Add(postModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            
            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostCreateViewModel post)
        {
            if (id != post.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var postModel = await _context.Posts.FirstOrDefaultAsync(m => m.ID == id);
                if (postModel == null)
                {
                    return NotFound();
                }

                postModel.Title = post.Title;
                postModel.Content = post.Content;
                postModel.PostDate = DateTime.Now;
                if (post.Image != null)
                {
                    postModel.ImageMimeType = post.Image.ContentType;
                    if (post.Image.Length > 0)
                    //Convert Image to byte and save to database
                    {
                        byte[] imageBytes = null;
                        using (var fileStream = post.Image.OpenReadStream())
                        using (var memoryStream = new MemoryStream())
                        {
                            fileStream.CopyTo(memoryStream);
                            imageBytes = memoryStream.ToArray();
                        }
                        postModel.Image = imageBytes;
                    }
                }

                try
                {
                    _context.Update(postModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostExists(postModel.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts
                .FirstOrDefaultAsync(m => m.ID == id);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.ID == id);
        }
    }
}

    

