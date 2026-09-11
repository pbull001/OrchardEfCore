using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrchardCoreEFCoreData.Controllers
{
    public sealed class HomeController : Controller
    {
        private readonly ExampleDbContext _context;

        public HomeController(ExampleDbContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            var events = _context.Events;
            return View(events);
        }
        public async Task<IActionResult> Seed()
        {
            var now = DateTime.UtcNow;

            _context.Events!.AddRange(
                new Event
                {
            EventDateTime = now.AddMinutes(-40),
                    IngestedDateTime = now.AddMinutes(-39),
                    DeviceReference = "DEV-001",
                    DeviceType = "Sensor",
                    EventTypeCode = "START",
                    LoopCount = 1,
                    Source = "GateA",
                    Destination = "Hub1"
                },
                new Event
                {
        EventDateTime = now.AddMinutes(-30),
                    IngestedDateTime = now.AddMinutes(-29),
                    DeviceReference = "DEV-002",
                    DeviceType = "Sensor",
                    EventTypeCode = "STOP",
                    LoopCount = 2,
                    Source = "GateB",
                    Destination = "Hub1"
                },
                new Event
                {
        EventDateTime = now.AddMinutes(-20),
                    IngestedDateTime = now.AddMinutes(-19),
                    DeviceReference = "DEV-003",
                    DeviceType = "Camera",
                    EventTypeCode = "ALERT",
                    LoopCount = 3,
                    Source = "GateC",
                    Destination = "Hub2"
                },
                new Event
                {
        EventDateTime = now.AddMinutes(-10),
                    IngestedDateTime = now.AddMinutes(-9),
                    DeviceReference = "DEV-004",
                    DeviceType = "Camera",
                    EventTypeCode = "HEARTBEAT",
                    LoopCount = 4,
                    Source = "GateD",
                    Destination = "Hub2"
                },
                new Event
                {
        EventDateTime = now,
                    IngestedDateTime = now,
                    DeviceReference = "DEV-005",
                    DeviceType = "Sensor",
                    EventTypeCode = "ERROR",
                    LoopCount = 5,
                    Source = "GateE",
                    Destination = "Hub3"
                }
            );

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
