namespace api.DTOs.ReviewDTO
{
    public class CreateReviewDTO
    {
        public string Comment { get; set; } = string.Empty;
        public int? Rating { get; set; }
        public int PetId { get; set; }

    }
}