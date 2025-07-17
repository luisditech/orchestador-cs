using System.ComponentModel;

namespace TropicFeel.Domain.Enums;

public enum StatusOrderSale
{
    [Description("Enviado a JLP")]
    SentToJlp = 1,
    [Description("Enviado a Netsuite")]
    SentToNetsuite = 2,
    [Description("Enviado a Sprint")]
    SentToSprint = 3,
    [Description("Error en el envío de la orden")]
    Error = 4,
    [Description("Devolución")]
    Return = 5,
    [Description("Tracking enviado a Netsuite")]
    ProcessTrackingNS = 6,
    [Description("Tracking enviado a JLP")]
    ProcessTrackingJlp = 7,
    [Description("Fulfillment completado")]
    PreviouslyCreated = 8,
    [Description("Orden Despachada")]
    Dispatch = 9,
    [Description("Orden cancelada")]
    Cancel = 10,
    [Description("Facturado")]
    Invoiced = 11,
    [Description("Tracking vacio")]
    TrackingEmpty = 12,
    [Description("Stock actualizado")]
    UpdateStockJlp = 13,
}
