using System;
using RepairCenterSolution.Models;
using RepairCenterSolution.Services;
 
namespace RepairCenterSolution;
 
internal static class Program
{
    private static void Main()
    {
        var center = new RepairCenter();
 
        var refrigerator = new Refrigerator(
            "FRG10001", "Samsung", "RT38", 380,
            DateTime.Today, DateTime.Today.AddDays(4));
 
        var washingMachine = new WashingMachine(
            "WASH2001", "LG", "F4V", 9,
            DateTime.Today, DateTime.Today.AddDays(3));
 
        var airConditioner = new AirConditioner(
            "AIR30001", "Gree", "Pular", 12000,
            DateTime.Today, DateTime.Today.AddDays(2));
 
        refrigerator.AdditionalCost = 10m;
        washingMachine.AdditionalCost = 5m;
 
        center.AddAppliance(refrigerator);
        center.AddAppliance(washingMachine);
        center.AddAppliance(airConditioner);
 
        Console.WriteLine("=== All appliances ===");
        foreach (Appliance appliance in center.Appliances)
            Console.WriteLine(appliance);
 
        Console.WriteLine($"Total repair cost: {center.CalculateAllRepairCosts():0.00}");
        Console.WriteLine($"Under maintenance: " +
            center.CountByStatus(RepairStatus.UnderMaintenance));
 
        center.ChangeStatus("FRG10001", RepairStatus.UnderMaintenance);
        center.ChangeStatus("FRG10001", RepairStatus.Repaired);
 
        Console.WriteLine("=== Overdue appliances ===");
        foreach (Appliance appliance in center.GetOverdueAppliances(
                     DateTime.Today.AddDays(5)))
            Console.WriteLine(appliance.SerialNumber);
    }
}
