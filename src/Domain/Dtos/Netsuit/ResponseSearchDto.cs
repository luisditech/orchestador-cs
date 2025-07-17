using Newtonsoft.Json;

namespace TropicFeel.Domain.Dtos.Netsuit;

public class ResponseSearchDto
{
    public List<List<BodyItemSearch>> Body { get; set; } = new List<List<BodyItemSearch>>();
}

public class BodyItemSearch
{
    [JsonProperty("UPC Code")]
    public required string UPCCode { get; set; }
    
    [JsonProperty("Internal ID")]
    public required string InternalID { get; set; } 
    
    [JsonProperty("ITEM NUMBER")]
    public required string ModelNumber { get; set; } 
}
