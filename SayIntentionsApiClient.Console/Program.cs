using SayIntentions;
using SayIntentionsApiClient.Configuration;


var apiClient = SayIntentionsApiClient.SayIntentionsApiClient.CreateFromConfiguration();


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
//Console.WriteLine(rch.Comm_History.Count + " entries in comm history"); // <--- Always null, either something wrong with the Client or SayIntention returns - To be validated.

var wx = await apiClient.GetWeather("CYUL");
Console.WriteLine(wx.Airports[0].Atis);

