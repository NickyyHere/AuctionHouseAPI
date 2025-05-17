using AuctionHouseAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuctionHouseAPI.Controllers
{
    [Route("api/[controller]")]
    public class AuctionController : Controller
    {
        private List<Auction> Auctions { get; set; } = new();
        public AuctionController() { }
        /// <summary>
        /// Create new auction
        /// </summary>
        /// <param name="auction"></param>
        /// <returns>Status code</returns>
        [HttpPost]
        public ActionResult CreateAuction([FromBody] Auction auction)
        {
            Auctions.Add(auction);
            return Created();
        }
        /// <summary>
        /// Edit auction by id and new values
        /// </summary>
        /// <param name="editedAuction"></param>
        /// <param name="id"></param>
        /// <returns>Status code</returns>
        [HttpPut("{id}")]
        public ActionResult EditAuction(int id, [FromBody] Auction editedAuction)
        {
            var auction = Auctions.FirstOrDefault(a => a.Id == id);
            if (auction == null)
            {
                return NotFound();
            }
            auction.Options = editedAuction.Options;
            auction.Item = editedAuction.Item;
            return NoContent();
        }
        /// <summary>
        /// Delete auction by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status code</returns>
        [HttpDelete]
        public ActionResult DeleteAuction(int id)
        {
            var auction = Auctions.FirstOrDefault(a => a.Id == id);
            if (auction == null)
            {
                return NotFound();
            }
            return NoContent();
        }
        /// <summary>
        /// Get all auctions
        /// </summary>
        /// <returns>List of auctions</returns>
        [HttpGet]
        public ActionResult<List<Auction>> GetAuctions()
        {
            return Ok(Auctions);
        }
        /// <summary>
        /// Get auction by its id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>One auction</returns>
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
        /// <returns>List of items</returns>
        [HttpGet("item")]
        public ActionResult<List<AuctionItem>> GetAuctionItems()
        {
            var auctionItems = Auctions.Select(a => a.Item).ToList();
            return Ok(auctionItems);
        }
        /// <summary>
        /// Get auction item by auction id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>One Auction item</returns>
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
        /// <param name="id"></param>
        /// <param name="editedAuctionItem"></param>
        /// <returns>Status code</returns>
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
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// <param name="id"></param>
        /// <param name="editedAuctionOptions"></param>
        /// <returns>Status code</returns>
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
