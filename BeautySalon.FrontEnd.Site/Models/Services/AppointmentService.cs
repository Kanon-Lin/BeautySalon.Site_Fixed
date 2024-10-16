using BeautySalon.FrontEnd.Site.Models.DTO;
using BeautySalon.FrontEnd.Site.Models.Interfaces;
using BeautySalon.FrontEnd.Site.Models.Repositories;
using BeautySalon.FrontEnd.Site.Models.ViewModels;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BeautySalon.FrontEnd.Site.Models.Services
{
    public class AppointmentService
    {
        private IAppointmentRepository _repo;
        public AppointmentService()
        {
            _repo = new AppointmentRepository();
        }

        public AppointmentService(IAppointmentRepository repo)
        {
            _repo = repo;
        }
        public List<TimeSlot> GenerateTimeSlot(DateTime startTime, DateTime endTime, int duration)
        {
            var slots = new List<TimeSlot>();

            DateTime currentTime = startTime;

            while (currentTime.AddMinutes(duration) <= endTime)
            {
                slots.Add(new TimeSlot
                {
                    Start = currentTime,
                    End = currentTime.AddMinutes(duration)
                });

                currentTime = currentTime.AddMinutes(duration);
            }

            return slots;
        }

        public List<TimeSlotVm> GetAvailableTimeSlots(int beauticianId, DateTime date, int duration)
        {
            var allSlots = GenerateTimeSlot(new DateTime(date.Year, date.Month, date.Day, 10, 0, 0),
                                             new DateTime(date.Year, date.Month, date.Day, 20, 0, 0), 30);

            var bookedAppointments = _repo.GetAppointment(beauticianId, date);
            var timeSlotVm = new List<TimeSlotVm>();
            int requiredSlots = (duration + 30 - 1) / 30;

            foreach (var slot in allSlots)
            {
                DateTime currentStartTime = slot.Start;
                DateTime requiredEndTime = currentStartTime.AddMinutes(duration);

                if (requiredEndTime > new DateTime(date.Year, date.Month, date.Day, 20, 0, 0))
                {
                    timeSlotVm.Add(new TimeSlotVm
                    {
                        StartTime = currentStartTime.ToString("HH:mm"),
                        EndTime = requiredEndTime.ToString("HH:mm"),
                        IsBooked = false,
                        IsAvailable = false
                    });
                    continue;
                }

                bool isAvailable = !bookedAppointments.Any(a =>
                    a.AppointmentStart < requiredEndTime && a.AppointmentEnd > currentStartTime
                );

                if (!isAvailable)
                {
                    timeSlotVm.Add(new TimeSlotVm
                    {
                        StartTime = currentStartTime.ToString("HH:mm"),
                        EndTime = requiredEndTime.ToString("HH:mm"),
                        IsBooked = true,
                        IsAvailable = false
                    });
                }
                else
                {
                    timeSlotVm.Add(new TimeSlotVm
                    {
                        StartTime = currentStartTime.ToString("HH:mm"),
                        EndTime = requiredEndTime.ToString("HH:mm"),
                        Duration = duration,
                        IsBooked = false,
                        IsAvailable = true
                    });
                }
            }

            return timeSlotVm;
        }


        public List<EmployeeVm> GetAvailableBeautician(int orderDetailId)
        {
            var serviceId = _repo.GetServiceIdByDetailId(orderDetailId);
            if (serviceId <= 0)
            {
                throw new Exception("無法找到指定項目的ID");
            }

            var categoryId = _repo.GetCategoryIdByServiceId(serviceId);
            if (categoryId <= 0)
            {
                throw new Exception("無法找到指定類別的ID");
            }

            var beauticians = _repo.GetBeauticiansByCategoryId(categoryId);

            return beauticians;
        }

        internal int GetDuration(int orderDetailId)
        {
            int duration = _repo.GetDuration(orderDetailId);

            return duration;
        }

        internal void CancelAppointment(int appointmentId)
        {
            _repo.CancelAppointment(appointmentId);
        }

        internal int GetOrderDetailId(int appointmentId)
        {
            var detailId = _repo.GetDetailIdByAppointmentId(appointmentId);

            return detailId;
        }

        internal void UpdateQuantity(int orderDetailId)
        {
            _repo.UpdateQuantity(orderDetailId);
            _repo.UpdateTotalQuantity(orderDetailId);
        }

        internal void CreateAppointment(AppointmentDto dto)
        {
            _repo.CreateAppointment(dto);
            _repo.UseQuantity(dto.OrderDetailId);
            _repo.SumQuantity(dto.OrderId);
        }

        public List<BookedSlot> GetBookedTimeSlots(int beauticianId, DateTime date)
        {
            return _repo.GetBookedTimeSlots(beauticianId, date);
        }

        internal int GetOrderId(int orderDetailId)
        {
            int orderId = _repo.GetOrderId(orderDetailId);

            return orderId;
        }

        internal int GetServiceId(int orderDetailId)
        {
            int serviceId = _repo.GetServiceIdByDetailId(orderDetailId);

            return serviceId;
        }
    }
}