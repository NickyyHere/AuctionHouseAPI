namespace AuctionHouseAPI.Shared.Exceptions
{
        [Serializable]
        public class EntityDoesNotExistException : Exception
        {
            public EntityDoesNotExistException() { }
            public EntityDoesNotExistException(string message) : base(message) { }
            public EntityDoesNotExistException(string message, Exception inner) : base(message, inner) { }
            protected EntityDoesNotExistException(
              System.Runtime.Serialization.SerializationInfo info,
#pragma warning disable
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }
}
