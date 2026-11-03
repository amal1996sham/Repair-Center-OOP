using System;
using System.Text.RegularExpressions;
 
namespace RepairCenterSolution.Services;
 
public static class ValidationUtility
{
    public static bool IsValidSerialNumber(string? serialNumber)
    {
        return !string.IsNullOrWhiteSpace(serialNumber)
            && Regex.IsMatch(serialNumber.Trim(), @"^[A-Za-z0-9]{5,20}$");
    }
 
    public static bool IsValidDate(DateTime date)
    {
        return date.Date <= DateTime.Today.AddYears(1).Date
            && date.Year >= 2000;
    }
 
    public static bool IsValidCost(decimal cost) => cost >= 0;
}
