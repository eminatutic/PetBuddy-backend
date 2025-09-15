
using api.DTOs.ReviewDTO;
using api.Models;
using AutoMapper;

namespace api.Mappers
{
    public class ReviewMapper : Profile
    {
        public ReviewMapper()
        {
            CreateMap<Review, CreateReviewDTO>();
            CreateMap<CreateReviewDTO, Review>();
        }
    }
}