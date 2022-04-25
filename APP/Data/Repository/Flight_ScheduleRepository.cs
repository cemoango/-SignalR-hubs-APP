

using APP.Business.Interface;
using APP.Business.Models;
using APP.Data.Context;
using AIR.ViewModels;
using Microsoft.EntityFrameworkCore;
using APP.ViewModels;

namespace APP.Data.Repository
{
    public class Flight_ScheduleRepository : Repository<Flight_Schedule>, IFlight_ScheduleRepository
    {
        public Flight_ScheduleRepository(APPDBContext context) : base(context) { }

        public async Task<IEnumerable<Flight_Schedule>> GetAllFlight_Schedule()
        {

            return await Db.flight_Schedule.AsNoTracking()
                .Include(p => p.airlineOBJ)
                .Include(p => p.flightOBJ)
                .Include(p => p.airportOBJ)
                //.Include(p => p.to_airportOBJ)
                .OrderBy(p => p.time).ToListAsync();
        }
        public List<Flight_ScheduleViewModels> GetAllFlight_ScheduleJS()
        {

            List<Flight_ScheduleViewModels> flight_ScheduleViewModels = new();

            var lst = Db.flight_Schedule.AsNoTracking()
                .Include(p => p.airlineOBJ)
                .Include(p => p.flightOBJ)
                .Include(p => p.airportOBJ)
                .Include(p => p.flightOBJ)
                .OrderBy(p => p.time).ToList();

            foreach (var flight_Schedule in lst)
            {
                flight_ScheduleViewModels.Add(new Flight_ScheduleViewModels {
                    airline = flight_Schedule.airlineOBJ.Name,
                    flight = flight_Schedule.flightOBJ.cod,
                    airport= flight_Schedule.airportOBJ.Name,
                    Id =    flight_Schedule.Id,
                    time = flight_Schedule.time,
                    status = flight_Schedule.status.ToString(),
                    to_fromStatus= flight_Schedule.to_fromStatus

                });
            }
            return flight_ScheduleViewModels;
        }
    }
}