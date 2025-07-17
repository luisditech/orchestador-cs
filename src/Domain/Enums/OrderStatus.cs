using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace TropicFeel.Domain.Enums;
[JsonConverter(typeof(JsonStringEnumConverter))]
[Description("Enum que representa el estado de la orden")]
public enum OrderStatusEnum
{
    
    [Description("New Order")]
    ORDER,

    [Description("Processing")]
    ORDER_ACK,

    [Description("Item(s) on Back Order")]
    BACKORDER,

    [Description("Dispatched")]
    DISPATCH,

    [Description("Cancellation Request")]
    CANCEL,

    [Description("Cancellation Acknowledgment")]
    CANCEL_ACK,

    [Description("New Return")]
    RMA,

    [Description("Return Processing")]
    RMA_ACK,

    [Description("Return Approved")]
    RMA_REC,

    [Description("Return Exception")]
    RMA_EXC,

    [Description("Return Rejected")]
    RMA_REJ


}
public class EnumAtrib
{
    public static string? GetEnumDescription(Enum? value)
    {
        if (value == null)
            return null;

        var field = value.GetType().GetField(value.ToString() ?? string.Empty);

        if (field == null)
            return value.ToString();

        var attribute = (DescriptionAttribute?)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));

        return attribute == null ? value.ToString() : attribute.Description;
    }
}

