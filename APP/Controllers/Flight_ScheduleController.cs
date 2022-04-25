using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using APP.Business.Models;
using APP.Data.Context;
using APP.Business.Interface;
using AutoMapper;
using APP.ViewModels;

namespace APP.Controllers
{
    public class Flight_ScheduleController : Controller
    {
        private readonly APPDBContext _context;
        private readonly IFlight_ScheduleRepository _flight_ScheduleRepository;
        private readonly IMapper _mapper;

        public Flight_ScheduleController(APPDBContext context, IFlight_ScheduleRepository flight_ScheduleRepository, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _flight_ScheduleRepository = flight_ScheduleRepository;

        }

        // GET: Flight_Schedule
        public async Task<IActionResult> Index()
        {
            var result = _mapper.Map<IEnumerable<Flight_ScheduleViewModels>>(
                await _flight_ScheduleRepository.GetAllFlight_Schedule());
            return View(result);
        }

        // GET: Flight_Schedule/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight_Schedule = await _context.flight_Schedule
                .Include(f => f.airlineOBJ)
                .Include(f => f.airportOBJ)
                .Include(f => f.flightOBJ)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flight_Schedule == null)
            {
                return NotFound();
            }

            return View(flight_Schedule);
        }

        // GET: Flight_Schedule/Create
        public IActionResult Create()
        {
            ViewData["airlineID"] = new SelectList(_context.airline, "Id", "Name");
            ViewData["airportID"] = new SelectList(_context.airport, "Id", "Name");
            ViewData["flightID"] = new SelectList(_context.flight, "Id", "Name");
            return View();
        }

        // GET: Flight_Schedule/Permission
        [HttpGet, ActionName("Permission")]
        public async Task<IActionResult> Permission(Guid id)
        {
            var flight_Schedule = await _flight_ScheduleRepository.GetById(id);
            if (flight_Schedule == null)
            {
                return NotFound();
            }

            flight_Schedule.status = true;

            await _flight_ScheduleRepository.Update(flight_Schedule);

            return RedirectToAction("Index");
        }
        // GET: Flight_Schedule/Create
        public async Task<IActionResult> Chegada()
        {
            var result = _mapper.Map<IEnumerable<Flight_ScheduleViewModels>>(
            await _flight_ScheduleRepository.GetAllFlight_Schedule());
            return PartialView("Chegada", new { result, to_from = 1 });

        }

        // POST: Flight_Schedule/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("flightID,airportID,time,airlineID,status,to_fromStatus,Id")] Flight_Schedule flight_Schedule)
        {
            if (ModelState.IsValid)
            {
                flight_Schedule.Id = Guid.NewGuid();
                _context.Add(flight_Schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["airlineID"] = new SelectList(_context.airline, "Id", "Id", flight_Schedule.airlineID);
            ViewData["airportID"] = new SelectList(_context.airport, "Id", "Id", flight_Schedule.airportID);
            ViewData["flightID"] = new SelectList(_context.flight, "Id", "Id", flight_Schedule.flightID);
            return View(flight_Schedule);
        }

        // GET: Flight_Schedule/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight_Schedule = await _context.flight_Schedule.FindAsync(id);
            if (flight_Schedule == null)
            {
                return NotFound();
            }
            ViewData["airlineID"] = new SelectList(_context.airline, "Id", "Id", flight_Schedule.airlineID);
            ViewData["airportID"] = new SelectList(_context.airport, "Id", "Id", flight_Schedule.airportID);
            ViewData["flightID"] = new SelectList(_context.flight, "Id", "Id", flight_Schedule.flightID);
            return View(flight_Schedule);
        }

        // POST: Flight_Schedule/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("flightID,airportID,time,airlineID,status,to_fromStatus,Id")] Flight_Schedule flight_Schedule)
        {
            if (id != flight_Schedule.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(flight_Schedule);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Flight_ScheduleExists(flight_Schedule.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["airlineID"] = new SelectList(_context.airline, "Id", "Id", flight_Schedule.airlineID);
            ViewData["airportID"] = new SelectList(_context.airport, "Id", "Id", flight_Schedule.airportID);
            ViewData["flightID"] = new SelectList(_context.flight, "Id", "Id", flight_Schedule.flightID);
            return View(flight_Schedule);
        }

        // GET: Flight_Schedule/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight_Schedule = await _context.flight_Schedule
                .Include(f => f.airlineOBJ)
                .Include(f => f.airportOBJ)
                .Include(f => f.flightOBJ)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (flight_Schedule == null)
            {
                return NotFound();
            }

            return View(flight_Schedule);
        }

        // POST: Flight_Schedule/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var flight_Schedule = await _context.flight_Schedule.FindAsync(id);
            _context.flight_Schedule.Remove(flight_Schedule);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Flight_ScheduleExists(Guid id)
        {
            return _context.flight_Schedule.Any(e => e.Id == id);
        }
        private async Task<Flight_ScheduleViewModels> InicializeValues(Flight_ScheduleViewModels flight_ScheduleViewModels)
        {
            //flight_ScheduleViewModels. = _mapper.Map<IEnumerable<AirlineViewModels>>(await _airlineRepository.GetAll());
            //flight_ScheduleViewModels.airports = _mapper.Map<IEnumerable<AirportViewModels>>(await _airportRepository.GetAll());
            //flight_ScheduleViewModels.flights = _mapper.Map<IEnumerable<FlightViewModels>>(await _flightRepository.GetAll());

            return flight_ScheduleViewModels;
        }

        //private async Task<Flight_ScheduleViewModels> InicializeArline(Flight_ScheduleViewModels flight_ScheduleViewModels)
        //{
        //    flight_ScheduleViewModels.airlines = _mapper.Map<IEnumerable<AirlineViewModels>>(await _airlineRepository.GetAll());
        //    return flight_ScheduleViewModels;
        //}

        //private async Task<Flight_ScheduleViewModels> InicializeAirport(Flight_ScheduleViewModels flight_ScheduleViewModels)
        //{
        //    flight_ScheduleViewModels.airports = _mapper.Map<IEnumerable<AirportViewModels>>(await _airportRepository.GetAll());
        //    return flight_ScheduleViewModels;
        //}

        //private async Task<Flight_ScheduleViewModels> InicializeFlight(Flight_ScheduleViewModels flight_Schedule)
        //{
        //    flight_Schedule.flights = _mapper.Map<IEnumerable<FlightViewModels>>(await _flightRepository.GetAll());
        //    return flight_Schedule;
        //}
    }
}
