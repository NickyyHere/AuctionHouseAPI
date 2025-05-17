using AuctionHouseAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuctionHouseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuctionController : Controller
    {
        private List<Auction> Auctions { get; set; } = new();
        public AuctionController() 
        {
            Auctions.Add(new Auction
            {
                Id = 0,
                OwnerId = 0,
                Item = new AuctionItem
                {
                    Name = "Name",
                    Description = "Description",
                    AuctionId = 0,
                    CategoryId = 0,
                    CustomTags = new List<string> 
                    {
                        "Tag1",
                        "Tag2"
                    }
                },
                Options = new AuctionOptions
                {
                    AuctionId = 0,
                    AllowBuyItNow = false,
                    StartDateTime = DateTime.Now,
                    FinishDateTime = DateTime.Now.AddDays(2),
                    IsActive = true,
                    IsIncreamentalOnLastMinuteBid = true,
                    MinimumOutbid = 20,
                    MinutesToIncrement = 2,
                    StartingPrice = 100.99M,
                }
            });
            Auctions.Add(new Auction
            {
                Id = 1,
                OwnerId = 1,
                Item = new AuctionItem
                {
                    Name = "Name2",
                    Description = "Description",
                    AuctionId = 1,
                    CategoryId = 1,
                    CustomTags = new List<string>
                    {
                        "Tag1",
                        "Tag2"
                    }
                },
                Options = new AuctionOptions
                {
                    AuctionId = 1,
                    AllowBuyItNow = true,
                    BuyItNowPrice = 300,
                    StartDateTime = DateTime.Now,
                    FinishDateTime = DateTime.Now.AddDays(2),
                    IsActive = true,
                    IsIncreamentalOnLastMinuteBid = false,
                    MinimumOutbid = 5,
                    StartingPrice = 20M,
                }
            });
        }
        /// <summary>
        /// Create new auction
        /// </summary>
        /// <remarks>
        /// This endpoint takes auction in a form of JSON and parses it to Auction object.
        /// 
        /// Then saves the Auction object in the static list
        /// 
        /// Then returns status code 201
        /// </remarks>
        /// <param name="auction"></param>
        /// <returns>Status code</returns>
        /// <response code="201">Successfuly created</response>
        [HttpPost]
        public ActionResult CreateAuction([FromBody] Auction auction)
        {
            Auctions.Add(auction);
            return Created();
        }
        /// <summary>
        /// Edit auction by id and new values
        /// </summary>
        /// <remarks>
        /// This endpoint takes id from the route and JSON of auction and parses it to Auction object
        /// 
        /// Then finds the auction in the list by the id
        /// 
        /// Then checks if auction with such id exists
        /// 
        /// Then replaces the existing object with the one parsed from JSON
        /// </remarks>
        /// <param name="editedAuction"></param>
        /// <param name="id"></param>
        /// <returns>Status code</returns>
        /// <response code="404">Auction not found</response>
        /// <response code="204">Successfuly edited an auction</response>
        [HttpPut("{id}")]
        public ActionResult EditAuction(int id, [FromBody] Auction editedAuction)
        {
            var auction = Auctions.FirstOrDefault(a => a.Id == id);
            if (auction == null)
            {
                return NotFound();
            }
            auction = editedAuction;
            return NoContent();
        }
        /// <summary>
        /// Delete auction by its id
        /// </summary>
        /// <remarks>
        /// This endpoint takes id from route
        /// 
        /// Then finds the auction in the list by the id
        /// 
        /// Then checks if auction with such id exists
        /// 
        /// Then deletes the auction from the list
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>Status code</returns>
        /// <response code="404">Auction not found</response>
        /// <response code="204">Successfuly deleted an auction</response>
        [HttpDelete]
        public ActionResult DeleteAuction(int id)
        {
            var auction = Auctions.FirstOrDefault(a => a.Id == id);
            if (auction == null)
            {
                return NotFound();
            }
            Auctions.Remove(auction);
            return NoContent();
        }
        /// <summary>
        /// Get all auctions
        /// </summary>
        /// <remarks>
        /// This endpoint returns the list of auctions in its body
        /// </remarks>
        /// <returns>List of auctions</returns>
        /// <response code="200">Successfuly fetched auctions</response>
        [HttpGet]
        public ActionResult<List<Auction>> GetAuctions()
        {
            return Ok(Auctions);
        }
        /// <summary>
        /// Get auction by its id
        /// </summary>
        /// <remarks>
        /// This endpoint takes id from route
        /// 
        /// Then finds the auction in the list by the id
        /// 
        /// Then checks if auction with such id exists
        /// 
        /// Then returns the auction
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>One auction</returns>
        /// <response code="404">Auction not found</response>
        /// <response code="200">Successfuly fetched an auction</response>
        [HttpGet("{id}")]
        public ActionResult GetAuction(int id)
        {
            var auction = Auctions.FirstOrDefault(a => a.Id == id);
            if (auction == null)
            {
                return NotFound();
            }
            return Ok(auction);
        }
        /// <summary>
        /// Get all auction items
        /// </summary>
        /// <remarks>
        /// This endpoint returns the list of all items ever auctioned
        /// </remarks>
        /// <returns>List of items</returns>
        /// <response code="200">Successfuly fetched auction items</response>
        [HttpGet("item")]
        public ActionResult<List<AuctionItem>> GetAuctionItems()
        {
            var auctionItems = Auctions.Select(a => a.Item).ToList();
            return Ok(auctionItems);
        }
        /// <summary>
        /// Get auction item by auction id
        /// </summary>
        /// <remarks>
        /// This endpoint takes id from route
        /// 
        /// Then finds the auction in the list by the id
        /// 
        /// Then checks if auction with such id exists
        /// 
        /// Then returns the auction item
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>One Auction item</returns>
        /// <response code="404">Auction item not found</response>
        /// <response code="200">Successfuly fetched an auction item</response>
        [HttpGet("{id}/item")]
        public ActionResult<AuctionItem> GetAuctionItem(int id)
        {
            var item = Auctions.Select(a => a.Item).FirstOrDefault(i  => i.AuctionId == id);
            if(item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }
        /// <summary>
        /// Edit auction item
        /// </summary>
        /// <remarks>
        /// This endpoint takes id from route and JSON of auction item and parses it to AuctionItem object
        /// 
        /// Then finds the auction in the list by the id
        /// 
        /// Then checks if auction witch such id exists
        /// 
        /// Then replaces the AuctionItem of the Auction with parsed AuctionItem
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="editedAuctionItem"></param>
        /// <returns>Status code</returns>
        /// <response code="404">Auction not found</response>
        /// <response code="200">Successfuly edited an auction item</response>
        [HttpPut("{id}/item")]
        public ActionResult EditAuctionItem(int id,[FromBody] AuctionItem editedAuctionItem)
        {
            var auction = Auctions.FirstOrDefault(a => a.Id == id);
            if (auction == null)
            {
                return NotFound();
            }
            auction.Item = editedAuctionItem;
            return NoContent();
        }
        /// <summary>
        /// Get auction options by auction id
        /// </summary>
        /// <remarks>
        /// This endpoint takes id from route
        /// 
        /// Then finds options of an auction with id
        /// 
        /// Then checks if auction with such id exists
        /// 
        /// Then returns the AuctionOptions
        /// </remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="404">Auction options not found</response>
        /// <response code="200">Successfuly fetched auction options</response>
        [HttpGet("{id}/options")]
        public ActionResult<AuctionOptions> GetAuctionOptions(int id)
        {
            var options = Auctions.Select(a => a.Options).FirstOrDefault(o => o.AuctionId == id);
            if(options == null)
            {
                return NotFound();
            }
            return Ok(options);
        }
        /// <summary>
        /// Edit auction options
        /// </summary>
        /// <remarks>
        /// This endpoint takes id from route and JSON of AuctionOptions and parses it to AuctionOptions object
        /// 
        /// Then finds auction in the list by the id
        /// 
        /// Then checks if auction with such id exists
        /// 
        /// Then replaces existing AuctionOptions object with parsed AuctionOptions
        /// </remarks>
        /// <param name="id"></param>
        /// <param name="editedAuctionOptions"></param>
        /// <returns>Status code</returns>
        /// <response code="404">Auction not found</response>
        /// <response code="204">Successfuly edited auction options</response>
        [HttpPut("{id}/options")]
        public ActionResult EditAuctionOptions(int id, [FromBody] AuctionOptions editedAuctionOptions)
        {
            var auction = Auctions.FirstOrDefault(a => a.Id == id);
            if (auction == null)
            {
                return NotFound();
            }
            auction.Options = editedAuctionOptions;
            return NoContent();
        }
        
    }
}
