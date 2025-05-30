using System.Text;

namespace ReservationAPI.Middlewares.Security;

public class Cryptographer : ICryptographer
{
    public string Encrypt(string data)
    {
        var encrypter = Encoding.UTF8.GetBytes(data);
        return System.Convert.ToBase64String(encrypter);
    }

    public string Decrypt(string data)
    {
        var decrypter = System.Convert.FromBase64String(data);
        return Encoding.UTF8.GetString(decrypter);
    }
}