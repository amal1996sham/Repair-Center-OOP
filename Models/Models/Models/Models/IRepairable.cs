namespace RepairCenterSolution.Models;
 
public interface IRepairable
{
    string SerialNumber { get; }
    string Model { get; }
    RepairStatus Status { get; }
    decimal CalculateRepairCost();
    void UpdateStatus(RepairStatus newStatus);
}
