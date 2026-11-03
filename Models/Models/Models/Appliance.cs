using System;
using RepairCenterSolution.Services;
 
namespace RepairCenterSolution.Models;
 
// Delegate used to notify the system when appliance status changes.
public delegate void ApplianceStatusChangedHandler(
    object? sender, ApplianceStatusChangedEventArgs e);
 
public sealed class ApplianceStatusChangedEventArgs : EventArgs
{
    public string SerialNumber { get; }
    public RepairStatus OldStatus { get; }
    public RepairStatus NewStatus { get; }
    public string Message { get; }
 
    public ApplianceStatusChangedEventArgs(
        string serialNumber, RepairStatus oldStatus,
        RepairStatus newStatus, string message)
    {
        SerialNumber = serialNumber;
        OldStatus = oldStatus;
        NewStatus = newStatus;
        Message = message;
    }
}
 
public abstract class Appliance : IRepairable
{
    private readonly string _serialNumber;
    private string _brand;
    private string _model;
    private RepairStatus _status;
    private readonly DateTime _receivedDate;
    private DateTime? _expectedDeliveryDate;
    private decimal _additionalCost;
 
    public string SerialNumber => _serialNumber; // read-only after creation
 
    public string Brand
    {
        get => _brand;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Brand cannot be empty.");
            _brand = value.Trim();
        }
    }
 
    public string Model
    {
        get => _model;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Model cannot be empty.");
            _model = value.Trim();
        }
    }
 
    public RepairStatus Status => _status;
    public DateTime ReceivedDate => _receivedDate;
 
    public DateTime? ExpectedDeliveryDate
    {
        get => _expectedDeliveryDate;
        set
        {
            if (value.HasValue && value.Value.Date < _receivedDate.Date)
                throw new ArgumentException("Delivery date cannot be before received date.");
            _expectedDeliveryDate = value;
        }
    }
 
    public decimal AdditionalCost
    {
        get => _additionalCost;
        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            _additionalCost = value;
        }
    }
 
    public event ApplianceStatusChangedHandler? StatusChanged;
 
    protected Appliance(string serialNumber, string brand, string model,
        DateTime receivedDate, DateTime? expectedDeliveryDate = null)
    {
        if (!ValidationUtility.IsValidSerialNumber(serialNumber))
            throw new ArgumentException("Serial number must be 5-20 letters or digits.");
        if (!ValidationUtility.IsValidDate(receivedDate))
            throw new ArgumentException("Received date is invalid.");
        _serialNumber = serialNumber.Trim();
        Brand = brand;
        Model = model;
        _receivedDate = receivedDate.Date;
        ExpectedDeliveryDate = expectedDeliveryDate;
        _status = RepairStatus.Received;
    }
 
    // Each derived appliance defines its own base repair cost.
    public abstract decimal CalculateRepairCost();
 
    public void UpdateStatus(RepairStatus newStatus)
    {
        RepairStatus oldStatus = _status;
        if (oldStatus == newStatus)
            return;
 
        _status = newStatus;
        string message = newStatus switch
        {
            RepairStatus.Repaired => $"Repair completed for {SerialNumber}.",
            RepairStatus.Delayed => $"Delivery delayed for {SerialNumber}.",
            _ => $"Status changed from {oldStatus} to {newStatus}."
        };
 
        StatusChanged?.Invoke(this,
            new ApplianceStatusChangedEventArgs(
                SerialNumber, oldStatus, newStatus, message));
    }
 
    public decimal GetTotalRepairCost()
    {
        return CalculateRepairCost() + AdditionalCost;
    }
 
    public bool IsOverdue(DateTime today)
    {
        return ExpectedDeliveryDate.HasValue
            && today.Date > ExpectedDeliveryDate.Value.Date
            && Status != RepairStatus.Delivered
            && Status != RepairStatus.Repaired;
    }
 
    public override string ToString()
    {
        return $"{SerialNumber} | {Brand} {Model} | {Status} | " +
               $"Cost: {GetTotalRepairCost():0.00}";
    }
}
