

using System.Text.Json.Nodes;



namespace OrchardCoreEFCoreData
{
    
    public class Event : IEvent
    {
        public Event() {}
        public long EventId { get; set; }
        public DateTime EventDateTime { get; set; }
        public DateTime? IngestedDateTime { get; set; }
        public string? DeviceReference { get; set; }
        public string? DeviceType { get; set; }
        public string? EventTypeCode { get; set; }
        public int? LoopCount { get; set; }
        public string? Source { get; set; }
        public string? Destination { get; set; }
        
        //[NotMapped]
        public JsonNode? Data { get; set; }
        

        
    }
}
