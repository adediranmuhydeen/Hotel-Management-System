﻿using HotelManagement.Core.Domains;
using HotelManagement.Core.DTOs;
using HotelManagement.Core.DTOs.ReviewDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagement.Core.IServices
{
    public interface IReviewService 
    {
         Task<Response<Review>> UpdateReview(string Id, UpdateReviewDto updateReviewDto);

        Task<Response<AddReviewsDTO>> AddReviewAsync(AddReviewsDTO model, string customerId);

        //basic comment.
        Task<Response<GetReviewsDTO>> GetHotelReviews(string hotelId);

        //Task<Response<GetReviewsDTO>> GetHotelReviews(string hotelId);
    }
}
