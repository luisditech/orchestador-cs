using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TropicFeel.Domain.Dtos.JLP;
 public class ProductResponseJlpDto
{
    public int count { get; set; }
    public string? next { get; set; }
    public string? previous { get; set; }
    public required List<Result> results { get; set; }
}
public class Result
{
    public string? url { get; set; }
    public string? part_number { get; set; }
    public string? category { get; set; }
    public string? client { get; set; }
    public string? supplier { get; set; }
    public required Data data { get; set; }
    public DateTime? back_in_stock_date { get; set; }
}

public class Data
{
    public string? image { get; set; }
    public string? boName { get; set; }
    public string? d2cInd { get; set; }
    public string? prodId { get; set; }
    public string? sorInd { get; set; }
    public string? dissNum { get; set; }
    public string? image_2 { get; set; }
    public string? image_3 { get; set; }
    public string? image_4 { get; set; }
    public string? image_5 { get; set; }
    public string? image_6 { get; set; }
    public string? image_7 { get; set; }
    public string? image_8 { get; set; }
    public string? image_9 { get; set; }
    public string? modelNo { get; set; }
    public string? dissName { get; set; }
    public string? image_10 { get; set; }
    public string? brandName { get; set; }
    public string? low_stock { get; set; }
    public string? distSource { get; set; }
    public string? free_stock { get; set; }
    public string? sellingUOM { get; set; }
    public string? tradedCode { get; set; }
    public string? webDispInd { get; set; }
    public string? branchTpInd { get; set; }
    public string? onlineTpInd { get; set; }
    public string? supplierNum { get; set; }
    public string? customerDesc { get; set; }
    public string? prevProdCode { get; set; }
    public string? supplierName { get; set; }
    public string? distributorID { get; set; }
    public string? noOfPeopleDel { get; set; }
    public string? prodAvailType { get; set; }
    public DateTime prodCodeSdate { get; set; }
    public DateTime dispIndChgDate { get; set; }
    public string? fulfilmentType { get; set; }
    public string? orderDescClean { get; set; }
    public string? webDispIndName { get; set; }
    public string? distributorName { get; set; }
    public DateTime stock_updated_at { get; set; }
    public string? supplierShortName { get; set; }
    public string? availability_status { get; set; }
    public string? product_direct_status { get; set; }
    public string? availability_feed_type { get; set; }
    public string? _SKIP_INV_STATUS_UPDATE { get; set; }
    public string? supplier_product_status { get; set; }
    public string? previous_available_units { get; set; }
    public string? ring_fenced_units_for_jl { get; set; }
    public string? previous_inventory_status { get; set; }
    public string? max_lead_time_to_delivery_in_days { get; set; }
    public string? current_lead_time_to_delivery_in_days { get; set; }
    public string? previous_lead_time_to_delivery_in_days { get; set; }
    public string? standard_lead_time_to_delivery_in_days { get; set; }
    public DateTime start_date_for_current_lead_time_to_delivery { get; set; }
}
