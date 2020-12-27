using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gallery.Data;
using Gallery.Domains;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Gallery.Web.Controllers
{
    public class ImageController : Controller
    {
        private readonly Context _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ImageController(Context context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Image
        public async Task<IActionResult> Index()
        {
            return View(await _context.Images.ToListAsync());
        }

        // GET: Image/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images
                .FirstOrDefaultAsync(m => m.ImageId == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // GET: Image/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Image/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImageId,Title,ImageFile,Details")] Image image)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;         //sets path to wwwroot folder
                string fileName = Path.GetFileNameWithoutExtension(image.ImageFile.FileName);
                string extension = Path.GetExtension(image.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;   //creating unique name for the new image
                image.ImageName = fileName;
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);

                using(var fileStream = new FileStream(path, FileMode.Create))
                {
                    await image.ImageFile.CopyToAsync(fileStream);
                }

                _context.Add(image);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(image);
        }

        // GET: Image/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images.FindAsync(id);
            if (image == null)
            {
                return NotFound();
            }
            return View(image);
        }

        // POST: Image/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ImageId,Title,ImageFile,Details,ImageName")] Image image)
        {
            if (id != image.ImageId)
            {
                return NotFound();
            }

            

            if (ModelState.IsValid)
            {
                var oldImage = await _context.Images.FindAsync(id);      //bandom
                var oldPath = Path.Combine(_hostEnvironment.WebRootPath, "Image", oldImage.ImageName);
                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                };
                oldImage = null;


                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(image.ImageFile.FileName);
                string extension = Path.GetExtension(image.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                image.ImageName = fileName;
                string newPath = Path.Combine(wwwRootPath + "/Image/", fileName);

                    //System.IO.File.Create(newPath);
                    using (var fileStream = new FileStream(newPath, FileMode.Create))
                    {
                        await image.ImageFile.CopyToAsync(fileStream);
                    }


                try
                {
                    _context.Update(image);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageExists(image.ImageId))
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
            return View(image);
        }

        // GET: Image/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images
                .FirstOrDefaultAsync(m => m.ImageId == id);
            if (image == null)
            {
                return NotFound();
            }

            return View(image);
        }

        // POST: Image/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var image = await _context.Images.FindAsync(id);

            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Image", image.ImageName);   //making path to the current object image by using it's ImageName prop
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            };

            _context.Images.Remove(image);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImageExists(int id)
        {
            return _context.Images.Any(e => e.ImageId == id);
        }
    }
}
