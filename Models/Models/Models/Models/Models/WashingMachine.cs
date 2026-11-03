using System;
 
namespace RepairCenterSolution.Models;
 
public sealed class WashingMachine : Appliance
{
    private int _capacityKg;
 
    public int CapacityKg
    {
        get => _capacityKg;
        set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
            _capacityKg = value;
        }
    }
 
    public WashingMachine(string serialNumber, string brand, string model,
        int capacityKg, DateTime receivedDate,
        DateTime? expectedDeliveryDate = null)
        : base(serialNumber, brand, model, receivedDate, expectedDeliveryDate)
    {
        CapacityKg = capacityKg;
    }
 
    public override decimal CalculateRepairCost() => 25m + CapacityKg * 2.5m;
}
