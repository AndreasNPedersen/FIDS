using FlyRejser.Data;
using Microsoft.EntityFrameworkCore;

namespace FlyRejser.Service.Queries
{
    public class QueryTravels
    {
        private FlyRejserContext _context;
        public QueryTravels(FlyRejserContext context) {
            _context = context;
        }
        public IQueryable<Travel> CurrentDepartureTravels()
        {
            return _context.Travel.Where(x => x.DepartureDate.Day == DateTime.Now.Day);
        }

        public IQueryable<Travel> CurrentArrivalTravels()
        {
            return _context.Travel.Where(x => x.ArrivalDate.Day == DateTime.Now.Day);
        }


    }
}
