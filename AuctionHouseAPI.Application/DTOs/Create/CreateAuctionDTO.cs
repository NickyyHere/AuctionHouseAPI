﻿using System.ComponentModel.DataAnnotations;

namespace AuctionHouseAPI.Application.DTOs.Create
{
#pragma warning disable
    public class CreateAuctionDTO
    {
        [Required]
        public CreateAuctionItemDTO Item { get; set; }
        [Required]
        public CreateAuctionOptionsDTO Options { get; set; }
    }
}
