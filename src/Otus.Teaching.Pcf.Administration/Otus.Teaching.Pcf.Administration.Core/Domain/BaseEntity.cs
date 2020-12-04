using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Otus.Teaching.Pcf.Administration.Core.Domain
{
    public class BaseEntity
    {
        [BsonId]
        public Guid Id { get; set; }
    }
}