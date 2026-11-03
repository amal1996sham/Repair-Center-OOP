using System;
 
namespace RepairCenterSolution.Models;
 
public sealed class Refrigerator : Appliance
{
    private int _capacityLiters;
 
    public int CapacityLiters
    {
        get => _capacityLiters;
        set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
            _capacityLiters = value;
        }
    }
 
    public Refrigerator(string serialNumber, string brand, string model,
        int capacityLiters, DateTime receivedDate,
        DateTime? expectedDeliveryDate = null)
        : base(serialNumber, brand, model, receivedDate, expectedDeliveryDate)
    {
        CapacityLiters = capacityLiters;
    }
 
    public override decimal CalculateRepairCost() => 35m + CapacityLiters * 0.08m;
}
