using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalalogAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CalalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private static readonly string CollectionId = "CalalogDB";
        // GET: api/Books
        [HttpGet]
        [Route("api/Books/Get")]
        public async Task<IEnumerable<Book>> Get()
        {
            var result = await DocumentDBRepository<Book>.GetItemsAsync(CollectionId);
            return result;


        }
    }
}
