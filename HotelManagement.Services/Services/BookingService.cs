﻿using AutoMapper;
using HotelManagement.Core.Domains;
using HotelManagement.Core.IRepositories;
using HotelManagement.Core.IServices;
using HotelManagement.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelManagement.Core.DTOs.BookingDtos;
using System.Net.NetworkInformation;

namespace HotelManagement.Services.Services
{
    public class BookingService : IBookingService
    {

        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BookingService> _logger;
        private readonly IBookingRepository _bookingRepository;
        public BookingService(IMapper mapper, IUnitOfWork unitOfWork, ILogger<BookingService> logger, IBookingRepository bookingRepository)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _bookingRepository = bookingRepository;
        }
        public async Task<Response<string>> CreateHotelBooking(BookingRequestDto bookingRequestDto)
        {
            _logger.LogInformation($"Attempt to create hotel bookings for customer with id {bookingRequestDto.CustomerId}");
            var bookingRequest = _mapper.Map<Booking>(bookingRequestDto);
            bookingRequest.Id = Guid.NewGuid().ToString();
            // bookingRequest.CheckIn = DateTime.ParseExact($"{bookingRequest.CheckIn}","MM/dd/yy",CultureInfo.GetCultureInfo("en-NG"));
            // bookingRequest.CheckOut = DateTime.ParseExact($"{bookingRequest.CheckOut}", "MM/dd/yy", CultureInfo.GetCultureInfo("en-NG"));
            try
            {
                await _unitOfWork.bookingRepository.AddAsync(bookingRequest);
                _unitOfWork.SaveChanges();
                return Response<string>.Success("Created Successfully", bookingRequest.Id, statusCode: 200);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the booking");

                // Return a failure response
                return Response<string>.Fail("An error occurred while creating the booking", statusCode: 500);
            }
           
        }
        public async Task<Response<List<BookingResponseDto>>> GetBookingPerManager(string managerId)
        {
            
           try
            {
               var bookings =   _unitOfWork.managerRepository.GetByIdAsync(x=>x.Id==managerId).Result.Hotels.SelectMany(x=>x.Bookings);
                var mappedBookings = _mapper.Map<List<BookingResponseDto>>(bookings);
                if(mappedBookings == null) return Response<List<BookingResponseDto>>.Fail("No Booking Found");
                return Response<List<BookingResponseDto>>.Success("Booking Successfully Loaded", mappedBookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the booking");

                // Return a failure response
                return Response<List<BookingResponseDto>>.Fail("An error occurred while Loading the booking");
            }

        }

    }
}
