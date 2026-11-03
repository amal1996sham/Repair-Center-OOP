using System;
using System.Collections.Generic;
using System.Linq;
using RepairCenterSolution.Models;
 
namespace RepairCenterSolution.Services;
 
public sealed class RepairCenter
{
    private readonly List<Appliance> _appliances = new();
 
    public IReadOnlyList<Appliance> Appliances => _appliances.AsReadOnly();
 
    public void AddAppliance(Appliance appliance)
    {
        if (appliance is null) throw new ArgumentNullException(nameof(appliance));
        if (_appliances.Any(a => a.SerialNumber == appliance.SerialNumber))
            throw new InvalidOperationException("Serial number already exists.");
 
        appliance.StatusChanged += OnApplianceStatusChanged;
        _appliances.Add(appliance);
    }
 
    public Appliance? FindBySerialNumber(string serialNumber)
    {
        return _appliances.FirstOrDefault(
            a => a.SerialNumber.Equals(serialNumber,
                StringComparison.OrdinalIgnoreCase));
    }
 
    public bool ChangeStatus(string serialNumber, RepairStatus newStatus)
    {
        Appliance? appliance = FindBySerialNumber(serialNumber);
        if (appliance is null) return false;
 
        appliance.UpdateStatus(newStatus);
        return true;
    }
 
    public decimal CalculateAllRepairCosts()
    {
        return _appliances.Sum(a => a.GetTotalRepairCost());
    }
 
    public int CountByStatus(RepairStatus status)
    {
        return _appliances.Count(a => a.Status == status);
    }
 
    public IReadOnlyList<Appliance> GetOverdueAppliances(DateTime today)
    {
        return _appliances.Where(a => a.IsOverdue(today)).ToList().AsReadOnly();
    }
 
    private void OnApplianceStatusChanged(
        object? sender, ApplianceStatusChangedEventArgs e)
    {
        Console.WriteLine($"[Notification] {e.Message}");
    }
}
