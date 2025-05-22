namespace AuctionHouseAPI.Shared.Exceptions
{ 
    [Serializable]
	public class DuplicateEntityException : Exception
	{
		public DuplicateEntityException() { }
		public DuplicateEntityException(string message) : base(message) { }
		public DuplicateEntityException(string message, Exception inner) : base(message, inner) { }
		protected DuplicateEntityException(
		  System.Runtime.Serialization.SerializationInfo info,
			#pragma warning disable SYSLIB0051
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
