namespace ReservationAPI.Middlewares.Security;

public interface ICryptographer
{
    public string Encrypt(string data);
    public string Decrypt(string data);
}