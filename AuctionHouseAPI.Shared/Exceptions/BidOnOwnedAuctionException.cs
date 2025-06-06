[Serializable]
public class BidOnOwnedAuctionException : Exception
{
	public BidOnOwnedAuctionException() { }
	public BidOnOwnedAuctionException(string message) : base(message) { }
	public BidOnOwnedAuctionException(string message, Exception inner) : base(message, inner) { }
	protected BidOnOwnedAuctionException(
	  System.Runtime.Serialization.SerializationInfo info,
#pragma warning disable SYSLIB0051
	  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}