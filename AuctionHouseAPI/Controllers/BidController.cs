using AuctionHouseAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuctionHouseAPI.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class BidController : Controller
    {
        private List<Bid> Bids = new();
        public BidController()
        {
            Bids.Add(new Bid
            {
                Id = 0,
                Amount = 100.01M,
                AuctionId = 0,
                UserId = 0,
                PlacedDateTime = DateTime.Now
            });
            Bids.Add(new Bid
            {
                Id = 1,
                Amount = 121.99M,
                AuctionId = 0,
                UserId = 1,
                PlacedDateTime = DateTime.Now.AddMinutes(1)
            });
            Bids.Add(new Bid
            {
                Id = 2,
                Amount = 150M,
                AuctionId = 0,
                UserId = 0,
                PlacedDateTime = DateTime.Now.AddMinutes(5)
            });
        }
        /// <summary>
        /// Places bid on specific auction for a user
        /// </summary>
        /// <param name="aid"></param>
        /// <param name="uid"></param>
        /// <param name="amount"></param>
        /// <returns>Status code</returns>
        [HttpPost("auction/{aid}/user/{uid}")]
        public ActionResult PlaceBid(int aid, int uid, [FromBody] decimal amount)
        {
            // no database yet so no checking if user or auction exists
            // not to mention authentication
            Bid bid = new Bid
            {
                UserId = uid,
                Amount = amount,
                AuctionId = aid,
                PlacedDateTime = DateTime.Now
            };
            return Created();
        }
        /// <summary>
        /// Get all bids for a specific auction
        /// </summary>
        /// <param name="aid"></param>
        /// <returns>List of bids</returns>
        [HttpGet("auction/{aid}")]
        public ActionResult<List<Bid>> GetAuctionBids(int aid)
        {
            var bids = Bids.Where(b => b.AuctionId == aid).ToList();
            return Ok(bids);
        }
        /// <summary>
        /// Get all bids for a specific user
        /// </summary>
        /// <param name="uid"></param>
        /// <returns>List of bids</returns>
        [HttpGet("user/{uid}")]
        public ActionResult<List<Bid>> GetUserBids(int uid)
        {
            var bids = Bids.Where(b => b.UserId == uid).ToList();
            return Ok(bids);
        }
        /// <summary>
        /// Get all user bids for specific auction
        /// </summary>
        /// <param name="aid"></param>
        /// <param name="uid"></param>
        /// <returns>List of bids</returns>
        [HttpGet("auction/{aid}/user/{uid}")]
        public ActionResult<List<Bid>> GetUserAuctionBids(int aid, int uid)
        {
            var bids = Bids.Where(b => b.UserId == uid && b.AuctionId == aid);
            return Ok(bids);
        }
        /// <summary>
        /// Remove all user bids for specific auction
        /// </summary>
        /// <param name="aid"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpDelete("auction/{aid}/user/{uid}")]
        public ActionResult WithdrawFromAuction(int aid, int uid)
        {
            var bids = Bids.Where(b => b.UserId == uid && b.AuctionId == aid).ToList();
            foreach(var b in bids)
            {
                Bids.Remove(b);
            }
            return NoContent();
        }
        /// <summary>
        /// Change bid amount
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        /// <returns>Status code</returns>
        [HttpPut("{id}")]
        public ActionResult EditBid(int id, [FromBody] decimal amount)
        {
            var bid = Bids.FirstOrDefault(b => b.Id == id);
            if(bid == null)
            {
                return NotFound();
            }
            bid.Amount = amount;
            return NoContent();
        }
    }
}
