namespace Final_Project_Farrukh_Rakhmanov.Models
{
    public class ProfilePicture
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContentType { get; set; } // MIME type (e.g., "image/jpeg")
        public string ImagePath { get; set; } // Path to the image file in the folder
    }
}
