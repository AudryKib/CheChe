using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.RequestHelpers;
using System;

namespace SearchService.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController: ControllerBase
    {
        public SearchController()
        {
 
        }

        [HttpGet]
        public async Task<ActionResult<List<Item>>> SearchItems([FromQuery] SearchParams searchParams)
        {
            var query = DB.PagedSearch<Item, Item>();


            if (!string.IsNullOrEmpty(searchParams.SearchTerm))
            {
                query.Match(Search.Full,searchParams.SearchTerm).SortByTextScore();
            }

            query = searchParams.OrderBy switch
            {
                "make" => query.Sort(x => x.Ascending(a => a.Make)),
                "new" => query.Sort(x => x.Descending(a => a.CreatedAt)),
                _ => query.Sort(x => x.Ascending(a => a.AuctionEnd)),
            };

            query = searchParams.FilterBy switch
            {
                "active" => query.Match(x => x.Where(a => a.AuctionEnd > DateTime.Now && a.Status == "active")),
                "endingSoon" => query.Match(x => x.Where(a => a.AuctionEnd < DateTime.Now.AddHours(6) && a.AuctionEnd > DateTime.Now)),
                _ => query.Match(x => x.AuctionEnd > DateTime.Now)
            };

            if (!string.IsNullOrEmpty(searchParams.Seller))
            {
                query.Match(x => x.Where(a => a.Seller == searchParams.Seller));
            }

            if (!string.IsNullOrEmpty(searchParams.Winner))
            {
                query.Match(x => x.Where(a => a.Seller == searchParams.Winner));
            }

            query.PageNumber(searchParams.PageNumber);
            query.PageSize(searchParams.PageSize);

            var result = await query.ExecuteAsync();

            return Ok( new {
                    results= result.Results,
                    pageCount = result.PageCount,
                    totalCount = result.TotalCount,
            });
        }
    }
}

