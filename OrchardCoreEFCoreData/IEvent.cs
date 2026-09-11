using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace OrchardCoreEFCoreData
{
    public interface IEvent
    {
        long EventId { get; set; }
        public DateTime EventDateTime { get; set; }
        public string? DeviceReference { get; set; }
        public string? DeviceType { get; set; }
        public string? EventTypeCode { get; set; }
        public int? LoopCount { get; set; }

        //public JsonNode? Data { get; }

        // This property will hold the serialized JObject as a string in the database
        //public string? JsonData { get;}
        
    }
}
