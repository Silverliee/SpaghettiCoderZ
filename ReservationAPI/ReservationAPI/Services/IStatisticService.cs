using ReservationAPI.Models.DTO;
using ReservationAPI.Models.DTO.Statistics;

namespace ReservationAPI.Services;

public interface IStatisticService
{
    Task<ParkingGlobalStatisticResponse> GetParkingGlobalStatisticAsync();
    Task<UserStatisticResponse> GetUserStatisticAsync(int userId);
    Task<UserStatisticResponse> GetUserStatisticByDateAsync(int userId, DateTime startDate, DateTime endDate);
}