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
using Gallery.Web.Models;

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

        

        public async Task<IActionResult> Index()
        {
            return View(await _context.Images.ToListAsync());
        }

        

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

        
        public IActionResult Create()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ImageViewModel image)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;                      //sets path to wwwroot folder
                string fileName = Path.GetFileNameWithoutExtension(image.ImageFile.FileName);
                string extension = Path.GetExtension(image.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;   //creating unique name for the new image
                image.ImageName = fileName;                                             //setting ImageName prop to be used for displaying image
                string path = Path.Combine(wwwRootPath + "/Image/", fileName);

                var newImageObject = new Image()
                {
                    ImageName = image.ImageName,
                    Title = image.Title,
                    Details = image.Details,
                    ImageFile = image.ImageFile
                };

                using(var fileStream = new FileStream(path, FileMode.Create))           //creating/uploading the new image in the set path
                {
                    await image.ImageFile.CopyToAsync(fileStream);
                }

                _context.Add(newImageObject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(image);
        }

        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var image = await _context.Images.FindAsync(id);
            var imageViewModel = new ImageViewModel() {
                ImageId=image.ImageId,
                Title=image.Title,
                Details=image.Details,
                ImageName=image.ImageName,
                ImageFile=image.ImageFile
            };
            if (image == null)
            {
                return NotFound();
            }
            return View(imageViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ImageViewModel image)
        {
            if (id != image.ImageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(image.ImageFile.FileName);
                string extension = Path.GetExtension(image.ImageFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                var newImageName = fileName;                        //used to set the new ImageName after deletint the old one
                var newPath = Path.Combine(wwwRootPath + "/Image/", fileName);

                if (newPath != null)
                {
                    using (var fileStream = new FileStream(newPath, FileMode.Create))
                    {
                        await image.ImageFile.CopyToAsync(fileStream);
                    }

                    var oldImageObject = await _context.Images.FindAsync(id);      
                    var oldPath = Path.Combine(_hostEnvironment.WebRootPath, "Image", oldImageObject.ImageName);
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    };
                    

                    oldImageObject.ImageName = newImageName;
                    oldImageObject.Details = image.Details;
                    oldImageObject.Title = image.Title;

                    try
                    {
                        _context.Images.Update(oldImageObject);
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
                }

                return RedirectToAction(nameof(Index));
            }
            return View(image);
        }

        

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

        

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var image = await _context.Images.FindAsync(id);

            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "Image", image.ImageName);   
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
