using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Otus.Teaching.Pcf.GivingToCustomer.Core.Domain
{
    public class BaseEntity
    {
        [BsonId]
        public Guid Id { get; set; }
    }
}