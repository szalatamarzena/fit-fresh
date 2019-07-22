using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcBlog.Data;
using MvcBlog.Models;
using System.Net.Mail;
using System.Text;

namespace MvcBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly MvcBlogContext _context;

        public HomeController(MvcBlogContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Blog()
        {
            return View(await _context.Posts.OrderByDescending(post => post.PostDate).ToListAsync());
        }

        public async Task<IActionResult> Post(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _context.Posts.FirstOrDefaultAsync(p => p.ID == id);

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult SendMail(ContactUsViewModel mailData)
        {
            if (ModelState.IsValid)
            {
                MailMessage mail = new MailMessage("obrona18@gmail.com", "obrona18@gmail.com");
                SmtpClient client = new SmtpClient();
                client.EnableSsl = true;
                client.Port = 25;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.UseDefaultCredentials = false;
                client.Host = "smtp.gmail.com";
                client.Credentials = new System.Net.NetworkCredential("obrona18@gmail.com", "18obrona19");
                mail.BodyEncoding = UTF8Encoding.UTF8;
                mail.Subject = "FitFood Message";
                mail.Body = $"Email: {mailData.Email}\n\n{mailData.Content}";
                client.Send(mail);

                return RedirectToAction("Index");
            }

            // If we got this far, something failed; redisplay form.
            return View("Index", mailData);
        }
    }
    
}
