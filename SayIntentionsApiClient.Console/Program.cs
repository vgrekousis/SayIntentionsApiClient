using SayIntentions;

SayIntentionsApiClient apiClient = new SayIntentionsApiClient("");
var r = await apiClient.GetParking();

if (r.Error != null)
{
    Console.WriteLine(r.Error.Error);
}
else
{
    if (r.Parking == null)
    {
        Console.WriteLine("No gate assignment is set by SI");
    }
    else
    {
        Console.WriteLine("Gate " + r.Parking.Id + " is expected as per assignment");
    }
}

var rch = await apiClient.GetCommsHistory();
//Console.WriteLine(rch.Comm_History.Count + " entries in comm history");

var wx = await apiClient.GetWeather("CYUL");
Console.WriteLine(wx.Airports[0].Atis);

