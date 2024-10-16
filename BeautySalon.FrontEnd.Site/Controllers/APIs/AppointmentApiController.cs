using BeautySalon.FrontEnd.Site.Models;
using BeautySalon.FrontEnd.Site.Models.DTO;
using BeautySalon.FrontEnd.Site.Models.EFModels;
using BeautySalon.FrontEnd.Site.Models.Services;
using BeautySalon.FrontEnd.Site.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Unity.Injection;

namespace BeautySalon.FrontEnd.Site.Controllers.APIs
{
    [RoutePrefix("api/AppointmentApi")]
    public class AppointmentApiController : ApiController
    {
        [HttpGet]
        [Route("GetAppointmentDetail")]
        public IHttpActionResult GetAppointmentDetail()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest("帳號未授權");
            }

            try
            {
                var db = new AppDbContext();
                int memberId = User.Identity.GetUserId<int>();

                var appointmentDetails = db.Appointments
                                          .AsNoTracking()
                                          .Include(a => a.Service)
                                          .Include(a => a.Employee)
                                          .Where(a => a.MemberID == memberId)
                                          .OrderByDescending(a => a.AppointmentStart)
                                          .Select(a => new
                                          {
                                              AppointmentDetailId = a.AppointmentID,
                                              ServiceName = a.Service.ServiceName,
                                              Beautician = a.Employee.Nickname,
                                              AppointmentStart = a.AppointmentStart,
                                              Status = a.AppointmentStatus
                                          })
                                          .ToList();

                var appointmentVm = appointmentDetails.Select(a => new AppointmentVm
                {
                    AppointmentDetailId = a.AppointmentDetailId,
                    ServiceName = a.ServiceName,
                    Beautician = a.Beautician,
                    Date = a.AppointmentStart.ToString("yyyy/MM/dd"), 
                    TimeSlot = a.AppointmentStart.ToString("HH:mm"),
                    Status = a.Status
                }).ToList();

                return Ok(appointmentVm);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("CancelAppointment/{appointmentId}")]
        public IHttpActionResult CancelAppointment(int appointmentId)
        {
            var service = new AppointmentService();
            service.CancelAppointment(appointmentId);

            int orderDetailId = service.GetOrderDetailId(appointmentId);
            service.UpdateQuantity(orderDetailId);

            return Ok(new { success = true });
        }

        [HttpGet]
        [Route("GetAvailableBeauticians/{orderDetailId}")]
        public IHttpActionResult GetAvailableBeauticians(int orderDetailId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return BadRequest("帳號未授權");
            }
            try
            {
                var service = new AppointmentService();

                var beautician = service.GetAvailableBeautician(orderDetailId);
                return Ok(beautician);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("GetAvailableTimeSlots")]
        public IHttpActionResult GetAvailableTimeSlots(int beauticianId, DateTime date, int orderDetailId)
        {
            if (date == default(DateTime) || orderDetailId <= 0)
            {
                return BadRequest("Invalid date or order detail ID.");
            }

            try
            {

                var service = new AppointmentService();

                var duration = service.GetDuration(orderDetailId);
                if (duration <= 0)
                {
                    return BadRequest("Invalid appointment duration.");
                }

                var availableTimeSlots = service.GetAvailableTimeSlots(beauticianId, date, duration);
                var bookedTimeSlots = service.GetBookedTimeSlots(beauticianId, date);

                var allTimeSlots = new List<TimeSlotResponse>();

                var bookedTimes = new HashSet<string>();
                foreach (var bookedSlot in bookedTimeSlots)
                {
                    bookedTimes.Add(bookedSlot.Start.ToString("HH:mm"));
                }

                var displayedTimeSlots = new HashSet<string>();

                foreach (var slot in availableTimeSlots)
                {
                    bool isAvailable = slot.Duration >= duration;

                    string timeSlotKey = $"{slot.StartTime}-{slot.EndTime}";

                    if (!displayedTimeSlots.Contains(timeSlotKey))
                    {
                        allTimeSlots.Add(new TimeSlotResponse
                        {
                            TimeSlot = slot.StartTime,
                            IsBooked = bookedTimes.Contains(slot.StartTime),
                            IsAvailable = isAvailable && !bookedTimes.Contains(slot.StartTime),
                            Duration = duration
                        });

                        displayedTimeSlots.Add(timeSlotKey);
                    }
                }

                if (!allTimeSlots.Any())
                {
                    return NotFound();
                }

                return Ok(allTimeSlots); 
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpGet]
        [Route("GetServiceDetail/{orderDetailId}")]
        public IHttpActionResult GetServiceDetail(int orderDetailId)
        {
            var db = new AppDbContext();
            var detail = db.OrderDetails
                           .Include(o => o.Service)
                           .Where(s => s.OrderDetailID == orderDetailId)
                           .Select(s => new AppointmentVm
                           {
                               ServiceName = s.Service.ServiceName,
                               Duration = s.Service.Duration
                           })
                           .ToList();

            return Ok(new { success = true, data = detail });
        }

        [HttpPost]
        [Route("CreateAppointment")]
        public IHttpActionResult CreateAppointment(AppointmentVm vm)
        {
            try
            {
                var service = new AppointmentService();
                AppointmentDto dto = vm.ToDto();
                int orderId = service.GetOrderId(dto.OrderDetailId);
                int serviceId = service.GetServiceId(dto.OrderDetailId);
                dto.MemberId = User.Identity.GetUserId<int>();
                dto.OrderId = orderId;
                dto.ServiceId = serviceId;

                service.CreateAppointment(dto);
                return Ok(new { message = "Appointment created successfully"});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
