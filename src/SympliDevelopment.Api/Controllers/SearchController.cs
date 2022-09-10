using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using SympliDevelopment.Api.Models;

namespace SympliDevelopment.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchController : ControllerBase
    {

        [HttpGet("keywords")]
        public async Task<IActionResult> GetResult([FromQuery] string keywords)
        {
            // please implement this method to return the result correctly.
            // the method receives an input keywords and then return the ranking result
            if (keywords is null)
            {
                return BadRequest("no search value is provided");
            }
            var url = "https://www.google.com/search?q=" + keywords + "&num=100&start=0";
            var websiteUrl = "https://www.sympli.com.au/";
            HtmlWeb web = new HtmlWeb();
            web.UserAgent = "user-agent=Mozilla/5.0" +
                " (Windows NT 10.0; Win64; *64)" +
                " AppleWebKit/537.36 (KHTML, like Gecko)" +
                " Chrome/74.0.3729.169 Safari/537.36";

            var htmlDoc = web.Load(url);
            var xPath = "//div[@class='yuRUbf']";

            HtmlNodeCollection nodes = htmlDoc.DocumentNode.SelectNodes(xPath);

            var searchDeatils = new List<SearchDeatiils>();
            var count = 0;
            foreach (var node in nodes)
            {
                var searchDeatil = new SearchDeatiils();

                var link = node.Descendants("a").FirstOrDefault().Attributes["href"].Value;
                searchDeatil.Url = node.Descendants("a").FirstOrDefault().Attributes["href"].Value;
                searchDeatil.Title = node.Descendants("h3").FirstOrDefault().InnerText;
                if (searchDeatil.Url.Contains(websiteUrl))
                {
                    searchDeatils.Add(searchDeatil);
                    count++;
                }
                
            }
            var response = new Response
            {
                SearchDeatiils = searchDeatils.ToArray(),
                Count = count,

            };
            return Ok(response);
        }

        
    }
}