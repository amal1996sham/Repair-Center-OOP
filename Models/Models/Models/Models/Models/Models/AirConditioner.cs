using System;
 
namespace RepairCenterSolution.Models;
 
public sealed class AirConditioner : Appliance
{
    private int _coolingCapacityBtu;
 
    public int CoolingCapacityBtu
    {
        get => _coolingCapacityBtu;
        set
        {
            if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
            _coolingCapacityBtu = value;
        }
    }
 
    public AirConditioner(string serialNumber, string brand, string model,
        int coolingCapacityBtu, DateTime receivedDate,
        DateTime? expectedDeliveryDate = null)
        : base(serialNumber, brand, model, receivedDate, expectedDeliveryDate)
    {
        CoolingCapacityBtu = coolingCapacityBtu;
    }
 
    public override decimal CalculateRepairCost() => 40m + coolingCapacityBtuCost();
 
    private decimal coolingCapacityBtuCost() => CoolingCapacityBtu / 1000m * 4m;
}
