namespace ReservationAPI.Middlewares.Communication;

public class Messaging : IMessaging
{
    public void SendMessage(string message)
    {
        Console.WriteLine($"Message sent: {message}");
    }
}