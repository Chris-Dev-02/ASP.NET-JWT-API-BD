using JWT_API_BD.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JWT_API_BD.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly BasicUserAuthContext _basicUserAuthContext;
        public NewsController(BasicUserAuthContext basicUserAuthContext)
        {
            _basicUserAuthContext = basicUserAuthContext;
        }

        [HttpGet]
        [Route("list")]
        public IActionResult GetListOfNews()
        {
            List<News> news = new List<News>();
            try
            {
                news = _basicUserAuthContext.News.ToList();
                return StatusCode(StatusCodes.Status200OK, new { message = "OK", Response = news });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("{idNews:long}")]
        public IActionResult GetNews(long idNews)
        {
            News news = _basicUserAuthContext.News.Find(idNews);

            if(news == null)
            {
                return BadRequest("News not found");
            }
            try
            {
                news = _basicUserAuthContext.News.Include(c => c.PublishedByNavigation).Where(p => p.IdNews == idNews).FirstOrDefault();
                return StatusCode(StatusCodes.Status200OK, new { message = "OK", Response = news });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost]
        [Route("")]
        public IActionResult AddNews([FromBody] News newNews)
        {
            try
            {
                _basicUserAuthContext.News.Add(newNews);
                _basicUserAuthContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { message = "Ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut]
        [Route("{idNews:long}")]
        public IActionResult UpdateNews(long idNews, [FromBody] News news)
        {
            News foundNews = _basicUserAuthContext.News.Find(idNews);
            if (foundNews == null)
            {
                return BadRequest("Product not found");
            }
            try
            {
                foundNews.PublishedBy = news.PublishedBy is null ? foundNews.PublishedBy : news.PublishedBy;
                foundNews.Title = news.Title is null ? foundNews.Title : news.Title;
                foundNews.Description = news.Description is null ? foundNews.Description : news.Description;
                foundNews.Date = news.Description is null ? foundNews.Date : news.Date;

                _basicUserAuthContext.News.Update(foundNews);
                _basicUserAuthContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { message = "Ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpDelete]
        [Route("{idNews:long}")]
        public IActionResult DeleteNews(long idNews)
        {
            News news = _basicUserAuthContext.News.Find(idNews);
            if (news == null)
            {
                return BadRequest("Product not found");
            }
            try
            {
                _basicUserAuthContext.News.Remove(news);
                _basicUserAuthContext.SaveChanges();
                return StatusCode(StatusCodes.Status200OK, new { message = "Ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = ex.Message });
            }
        }
    }
}
