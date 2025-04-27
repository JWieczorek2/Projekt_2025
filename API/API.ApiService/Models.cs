namespace API.ApiService.Models;

public class RezerwacjaRequest
{
    public int NumerSali { get; set; }
    public string Login { get; set; } = string.Empty;
    public DateTime DataOd { get; set; }
    public DateTime DataDo { get; set; }
}

public class EdytujRezerwacjeRequest
{
    public DateTime NowaDataOd { get; set; }
    public DateTime NowaDataDo { get; set; }
}
