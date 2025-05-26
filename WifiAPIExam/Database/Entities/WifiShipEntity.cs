using System.ComponentModel.DataAnnotations;

namespace WifiAPIExam.Database.Entities;

public class WifiShipEntity
{
    [Key]
    public int ShipId { get; set; }
}

