using System.ComponentModel.DataAnnotations;

namespace WifiAPIExam.Models;

public class WifiShipIdModel
{
    [Key]
    public int ShipId { get; set; }
}

