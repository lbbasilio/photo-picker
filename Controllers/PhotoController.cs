using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using PhotoPicker.Entities;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using PhotoPicker.Infrastructure.Database;
using PhotoPicker.Infrastructure.Http;

namespace PhotoPicker.Controllers
{
    [Authorize]
    [Route("photo")]
    public class PhotoController : BaseController
    {
        private readonly DataContext _dataContext;
        public PhotoController(DataContext dataContext) { _dataContext = dataContext; }

        [HttpGet]
        public async Task<IActionResult> ListPhotos()
        {
            try
            {
                var id = long.Parse(GetClaim("Id"));
                var role = Enum.Parse<UserRole>(GetClaim(ClaimTypes.Role));

                if (role == UserRole.User)
                {
                    var photos = await _dataContext.Set<Photo>()
                                                   .AsNoTracking()
                                                   .Where(x => x.SelectedUserId == id || (x.SelectedUserId == null && x.RecognizedUserId == id))
                                                   .ToArrayAsync();

                    return Ok(photos);
                }
                else
                {
                    var photos = await _dataContext.Set<Photo>()
                                                   .AsNoTracking()
                                                   .Where(x => x.UploaderId == id)
                                                   .ToArrayAsync();

                    return Ok(photos);
                }
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [Authorize(Policy = "PhotographerOnly")]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadPhoto(ICollection<IFormFile> files)
        {
            try
            {
                var uploaderId = long.Parse(HttpContext.User.Claims.Where(x => x.Type == "Id").First().Value);
                var uploadDate = DateTime.Now;
                
                var photos = new List<Photo>();
                foreach (var file in files)
                {
                    using (var ms = new MemoryStream())
                    {
                        await file.CopyToAsync(ms);
                        var photo = new Photo
                        {
                            UploaderId = uploaderId,
                            UploadDate = uploadDate,
                            State = PhotoState.ProcessingPending
                        };
                    }
                }

                await _dataContext.AddRangeAsync(photos);
                await _dataContext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
