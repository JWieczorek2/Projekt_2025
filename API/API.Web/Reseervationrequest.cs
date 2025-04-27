public class ReservationRequest
{
    public int NumerSali { get; set; }
    public string Login { get; set; } = string.Empty;
    public DateTime DataOd { get; set; }
    public DateTime DataDo { get; set; }
}
