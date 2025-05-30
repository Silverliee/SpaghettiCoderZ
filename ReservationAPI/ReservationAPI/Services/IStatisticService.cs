using ReservationAPI.Models.DTO;

namespace ReservationAPI.Services;

public interface IStatisticService
{
    Task<ParkingGlobalStatisticResponse> GetParkingGlobalStatisticAsync();
    Task<ParkingGlobalStatisticResponse> GetParkingGlobalStatisticByDateAsync(DateTime startDate, DateTime endDate);
    
    Task<UserStatisticResponse> GetUserStatisticAsync(int userId);
    Task<UserStatisticResponse> GetUserStatisticByDateAsync(int userId, DateTime startDate, DateTime endDate);
}